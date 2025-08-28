using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models;
using bepensa_models.ApiResponse;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Net;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bepensa_biz.Proxies
{
    public class CarritoProxy : ProxyBase, ICarrito
    {
        private readonly GlobalSettings _ajustes;

        private readonly PremiosSettings _premio;

        private readonly Serilog.ILogger _logger;

        private readonly IApi _api;

        private readonly IAppEmail appEmail;

        private readonly IBitacora bitacora;

        private readonly IOpenPay _openPay;

        private readonly string UrlPremios;

        private readonly IMapper mapper;

        public CarritoProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premio,
                            IApi api, IAppEmail appEmail, IBitacora bitacora, IOpenPay openPay,
                            IMapper mapper, Serilog.ILogger logger)
        {
            DBContext = context;
            _ajustes = ajustes.Value;
            _premio = premio.Value;
            _logger = logger;

            _api = api;
            this.appEmail = appEmail;
            this.bitacora = bitacora;
            _openPay = openPay;
            this.mapper = mapper;

            UrlPremios = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;
        }

        public async Task<Respuesta<Empty>> AgregarPremio(AgregarPremioRequest pPremio, int idOrigen)
        {
            Respuesta<Empty> resultado = new()
            {
                IdTransaccion = Guid.NewGuid()
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Premios.Any(p => p.Id == pPremio.IdPremio && p.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                {
                    resultado.Codigo = (int)CodigoDeError.RedencionPendienteCarrito;
                    resultado.Mensaje = CodigoDeError.RedencionPendienteCarrito.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                            .Include(x => x.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                                .ThenInclude(x => x.IdPremioNavigation)
                            .First(x => x.Id == pPremio.IdUsuario);

                var premio = DBContext.Premios.First(x => x.Id == pPremio.IdPremio);

                if (premio.RequiereTarjeta && pPremio.IdTarjeta == null)
                {
                    resultado.Codigo = (int)CodigoDeError.TarjetaNoSeleccionada;
                    resultado.Mensaje = CodigoDeError.TarjetaNoSeleccionada.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (premio.IdTipoTransaccion == (int)TipoTransaccion.Recarga)
                {
                    if (pPremio.Cantidad != 1)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloUnaRecarga;
                        resultado.Mensaje = CodigoDeError.SoloUnaRecarga.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (string.IsNullOrEmpty(pPremio.TelefonoRecarga))
                    {
                        resultado.Codigo = (int)CodigoDeError.IngresaTelefono;
                        resultado.Mensaje = CodigoDeError.IngresaTelefono.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var validarSiHayRecarga = usuario.Carritos.Any(x => x.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Recarga);

                    if (validarSiHayRecarga)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloUnaRecargaXCanje;
                        resultado.Mensaje = CodigoDeError.SoloUnaRecargaXCanje.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }
                }

                if (!pPremio.ForzarRegistro)
                {
                    // Calculamos costo total del premio de acuerdo a la cantidad.
                    int totalPremio = premio.Puntos * pPremio.Cantidad;

                    //Obtenemos el saldo actual del usuario
                    int saldoActual = DBContext.Movimientos.OrderByDescending(s => s.Id).Where(s => s.IdUsuario == pPremio.IdUsuario).Select(s => s.Saldo).Take(1).FirstOrDefault();

                    int puntosCarrito = usuario.Carritos.Sum(c => c.Puntos);

                    //Calculamos el total de puntos utilizados con base al carrito y al premio nuevo a adquirir
                    int puntosTotales = totalPremio + puntosCarrito;

                    if (!(puntosTotales <= saldoActual))
                    {
                        resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                        resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }
                }

                var carrito = usuario.Carritos;

                if (carrito.Any(x => x.IdTransaccionLog != null))
                {
                    resultado.IdTransaccion = carrito.Where(x => x.IdTransaccionLog != null).Select(x => x.IdTransaccionLog).First();
                }

                var strategy = DBContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await DBContext.Database.BeginTransactionAsync();

                    try
                    {
                        DateTime fechaSolicitud = DateTime.Now;

                        BitacoraDeUsuario bdu = new()
                        {
                            IdUsuario = pPremio.IdUsuario,
                            IdTipoDeOperacion = (int)TipoOperacion.AgregaCarrito,
                            FechaReg = fechaSolicitud,
                            Notas = TipoOperacion.AgregaCarrito.GetDescription() + " SKU: " + premio.Sku,
                            IdOperdorReg = pPremio.IdOperador,
                            IdOrigen = idOrigen
                        };

                        usuario.BitacoraDeUsuarios.Add(bdu);

                        for (int i = 0; i < pPremio.Cantidad; i++)
                        {
                            Carrito carrito = new()
                            {
                                IdPremio = pPremio.IdPremio,
                                Cantidad = 1,
                                IdEstatusCarrito = (int)TipoEstatusCarrito.EnProceso,
                                FechaReg = fechaSolicitud,
                                TelefonoRecarga = pPremio.TelefonoRecarga,
                                IdTarjeta = pPremio.IdTarjeta,
                                Puntos = premio.Puntos,
                                IdOrigen = idOrigen,
                                IdTransaccionLog = resultado.IdTransaccion
                            };

                            usuario.Carritos.Add(carrito);
                        }

                        await DBContext.SaveChangesAsync();

                        await transaction.CommitAsync();

                        resultado.Mensaje = MensajeApp.PremioAgregado.GetDescription();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();

                        resultado.Codigo = (int)CodigoDeError.FalloAgregarPremio;
                        resultado.Mensaje = CodigoDeError.FalloAgregarPremio.GetDescription();
                        resultado.Exitoso = false;
                    }

                    if (resultado.Codigo == (int)CodigoDeError.OK && pPremio.IdOperador != null) bitacora.BitacoraDeOperadores(pPremio.IdOperador.Value, (int)TipoOperacion.AgregaCarrito, pPremio.IdUsuario);
                });
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "AgregarPremio(AgregarPremioRequest, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }

            return resultado;
        }

        public async Task<Respuesta<CarritoDTO>> EliminarPremio(RequestByIdPremio pPremio, int idOrigen)
        {
            Respuesta<CarritoDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                {
                    resultado.Codigo = (int)CodigoDeError.ProductoRedencionNoEditable;
                    resultado.Mensaje = CodigoDeError.ProductoRedencionNoEditable.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var validaCarrito = DBContext.Carritos.Any(c => c.IdUsuario == pPremio.IdUsuario && c.IdPremio == pPremio.IdPremio && c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);

                if (!validaCarrito)
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = await DBContext.Usuarios.FirstAsync(u => u.Id == pPremio.IdUsuario);

                var carrito = DBContext.Carritos
                    .Include(x => x.IdPremioNavigation)
                    .Include(x => x.IdEstatusCarritoNavigation)
                    .Include(x => x.IdTarjetaNavigation)
                    .Where(x => x.IdUsuario == usuario.Id && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)
                    .ToList();

                foreach (var premio in carrito.Where(x => x.IdPremio == pPremio.IdPremio))
                {
                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Cancelado;
                }

                usuario.BitacoraDeUsuarios.Add(new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.QuitaCarrito,
                    FechaReg = DateTime.Now,
                    IdOperdorReg = pPremio.IdOperador,
                    Notas = TipoOperacion.QuitaCarrito.GetDescription(),
                    IdOrigen = idOrigen
                });

                DBContext.SaveChanges();

                resultado.Mensaje = "El premio ha sido eliminado del carrito";

                resultado.Data = GetCarrito(mapper.Map<List<CarritoModelDTO>>(carrito.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).ToList()));

                if (resultado.Codigo == (int)CodigoDeError.OK && pPremio.IdOperador != null) bitacora.BitacoraDeOperadores(pPremio.IdOperador.Value, (int)TipoOperacion.QuitaCarrito, pPremio.IdUsuario);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "EliminarPremio(RequestByIdPremio, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }

            return resultado;
        }

        public async Task<Respuesta<CarritoDTO>> ModificarPremio(ActPremioRequest pPremio, int idOrigen)
        {
            Respuesta<CarritoDTO> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                {
                    resultado.Codigo = (int)CodigoDeError.ProductoRedencionNoEditable;
                    resultado.Mensaje = CodigoDeError.ProductoRedencionNoEditable.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = await DBContext.Usuarios.
                    Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(x => x.IdPremioNavigation)
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                            .ThenInclude(x => x.IdEstatusCarritoNavigation)
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(x => x.IdTarjetaNavigation)
                    .FirstAsync(u => u.Id == pPremio.IdUsuario);

                var validaCarrito = usuario.Carritos
                    .Any(c => c.IdUsuario == pPremio.IdUsuario &&
                            c.IdPremio == pPremio.IdPremio &&
                            c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);

                if (!validaCarrito)
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Premios.Any(p => p.Id == pPremio.IdPremio && p.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                DateTime fechaSolicitud = DateTime.Now;

                if (pPremio.AumentarCantidad)
                {
                    var premio = DBContext.Premios.Find(pPremio.IdPremio);

                    if (premio == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                        resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (premio.RequiereTarjeta)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloDesdeSeccionPremios;
                        resultado.Mensaje = CodigoDeError.SoloDesdeSeccionPremios.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (premio.IdTipoTransaccion == (int)TipoTransaccion.Recarga)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloUnaRecarga;
                        resultado.Mensaje = CodigoDeError.SoloUnaRecarga.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (premio.IdTipoDePremio == (int)TipoPremio.Digital)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloDesdeSeccionPremios;
                        resultado.Mensaje = CodigoDeError.SoloDesdeSeccionPremios.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (!pPremio.ForzarRegistro)
                    {
                        // calculamos costo total del premio de acuerdo a la cantidad.
                        int totalPremio = premio.Puntos * 1;

                        //Obtenemos el saldo actual del usuario
                        int saldoActual = DBContext.Movimientos.OrderByDescending(s => s.Id).Where(s => s.IdUsuario == pPremio.IdUsuario).Select(s => s.Saldo).Take(1).FirstOrDefault();

                        //Calculamos la suma de puntos de los premios que están en el carrito con estatus "En Proceso"
                        int puntosCarrito = DBContext.Carritos.Where(c => c.IdUsuario == pPremio.IdUsuario && c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).Sum(c => c.Puntos);

                        //Calculamos el total de puntos utilizados con base al carrito y al premio nuevo a adquirir
                        int puntosTotales = totalPremio + puntosCarrito;

                        if (!(puntosTotales <= saldoActual))
                        {
                            resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                            resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                            resultado.Exitoso = false;

                            return resultado;
                        }
                    }

                    try
                    {
                        BitacoraDeUsuario bdu = new()
                        {
                            IdUsuario = pPremio.IdUsuario,
                            IdTipoDeOperacion = (int)TipoOperacion.ModificaCarrito,
                            FechaReg = fechaSolicitud,
                            IdOperdorReg = pPremio.IdOperador,
                            Notas = TipoOperacion.ModificaCarrito.GetDescription() + " SKU: " + premio.Sku,
                            IdOrigen = idOrigen
                        };

                        usuario.BitacoraDeUsuarios.Add(bdu);

                        Carrito carritoAdd = new()
                        {
                            IdUsuario = pPremio.IdUsuario,
                            IdPremio = pPremio.IdPremio,
                            Cantidad = 1,
                            FechaReg = fechaSolicitud,
                            IdEstatusCarrito = (int)TipoEstatusCarrito.EnProceso,
                            Puntos = premio.Puntos,
                            IdOrigen = DBContext.Carritos.Where(c => c.IdPremio == pPremio.IdPremio).Select(c => c.IdOrigen).FirstOrDefault()
                        };

                        usuario.Carritos.Add(carritoAdd);

                        DBContext.SaveChanges();

                        resultado.Mensaje = "Agregaste el premio al carrito correctamente.";
                    }
                    catch (Exception ex)
                    {
                        resultado.Codigo = (int)CodigoDeError.FalloAgregarPremio;
                        resultado.Mensaje = CodigoDeError.FalloAgregarPremio.GetDescription();
                        resultado.Exitoso = false;

                        _logger.Error(ex, "ModificarPremio(ActPremioRequest, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);

                        return resultado;
                    }
                }
                else
                {
                    var premios = usuario.Carritos.Count(c => c.IdPremio == pPremio.IdPremio);

                    var premio = usuario.Carritos
                        .OrderBy(c => c.Id)
                        .FirstOrDefault(c => c.IdPremio == pPremio.IdPremio);

                    if (premio == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                        resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (premio.IdPremioNavigation.RequiereTarjeta)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloEliminacion;
                        resultado.Mensaje = CodigoDeError.SoloEliminacion.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Cancelado;

                    usuario.BitacoraDeUsuarios.Add(new()
                    {
                        IdUsuario = pPremio.IdUsuario,
                        IdTipoDeOperacion = (int)TipoOperacion.ModificaCarrito,
                        FechaReg = fechaSolicitud,
                        IdOperdorReg = pPremio.IdOperador,
                        Notas = TipoOperacion.ModificaCarrito.GetDescription() + " SKU: " + premio.IdPremioNavigation.Sku,
                        IdOrigen = idOrigen
                    });

                    DBContext.SaveChanges();


                    if (premios == 1)
                    {
                        resultado.Mensaje = "El premio ha sido eliminado del carrito";
                    }
                    else
                    {
                        resultado.Mensaje = "Se ha eliminado un premio del carrito";
                    }
                }

                var carrito = usuario.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).ToList();

                resultado.Data = GetCarrito(mapper.Map<List<CarritoModelDTO>>(carrito.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).ToList()));

                if (resultado.Codigo == (int)CodigoDeError.OK && pPremio.IdOperador != null) bitacora.BitacoraDeOperadores(pPremio.IdOperador.Value, (int)TipoOperacion.ModificaCarrito, pPremio.IdUsuario);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                _logger.Error(ex, "ModificarPremio(ActPremioRequest, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }
            return resultado;
        }

        public async Task<Respuesta<CarritoDTO>> EliminarCarrito(RequestByIdUsuario pPremio, int idOrigen)
        {
            Respuesta<CarritoDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var validaCarrito = DBContext.Carritos.Any(c => c.IdUsuario == pPremio.IdUsuario
                                                            && c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);
                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                {
                    resultado.Codigo = (int)CodigoDeError.ProductoRedencionNoEditable;
                    resultado.Mensaje = CodigoDeError.ProductoRedencionNoEditable.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!validaCarrito)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = await DBContext.Usuarios
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                    .FirstAsync(u => u.Id == pPremio.IdUsuario);


                var carrito = usuario.Carritos.ToList();

                foreach (var premio in carrito)
                {
                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Cancelado;
                }

                usuario.BitacoraDeUsuarios.Add(new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.EliminoCarrito,
                    FechaReg = DateTime.Now,
                    IdOperdorReg = pPremio.IdOperador,
                    Notas = TipoOperacion.QuitaCarrito.GetDescription(),
                    IdOrigen = idOrigen
                });

                DBContext.SaveChanges();

                resultado.Mensaje = "Carrito vacío";

                var url = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;

                resultado.Data = GetCarrito(mapper.Map<List<CarritoModelDTO>>(carrito.Where(x => x.IdEstatusCarrito != (int)TipoEstatusCarrito.Cancelado).ToList()));

                if (resultado.Codigo == (int)CodigoDeError.OK && pPremio.IdOperador != null) bitacora.BitacoraDeOperadores(pPremio.IdOperador.Value, (int)TipoOperacion.EliminoCarrito, pPremio.IdUsuario);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "EliminarCarrito(RequestByIdUsuario, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<CarritoDTO> ConsultarCarrito(RequestByIdUsuario pPremio)
        {
            Respuesta<CarritoDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Exitoso = valida.Exitoso;
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo && u.Bloqueado == false))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var carrito = DBContext.Carritos
                            .Include(x => x.IdPremioNavigation)
                            .Include(x => x.IdEstatusCarritoNavigation)
                            .Include(x => x.IdTarjetaNavigation)
                            .Where(x => x.IdUsuario == pPremio.IdUsuario
                                && (x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso
                                    || x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion
                                    || x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                            .ToList();

                if (carrito == null || carrito.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = GetCarrito(mapper.Map<List<CarritoModelDTO>>(carrito).ToList());
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "ConsultarCarrito(RequestByIdUsuario, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<EvaluacionPagoDTO> EvaluacionPago(int idUsuario, int idOrigen)
        {
            Respuesta<EvaluacionPagoDTO> resultado = new();

            try
            {
                var usuario = DBContext.Usuarios
                    .Include(x => x.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso || x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                    .FirstOrDefault(x => x.Id == idUsuario);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.Carritos.Any(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                int puntosDisponibles = DBContext.Movimientos.Where(x => x.IdUsuario == idUsuario).OrderByDescending(x => x.Id).Take(1).Select(x => x.Saldo).FirstOrDefault();

                int puntosCarrito = usuario.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).Sum(x => x.Puntos);

                int diferencia = puntosDisponibles - puntosCarrito;

                int puntosFaltantes = diferencia < 0 ? Math.Abs(diferencia) : 0;

                double deposito = puntosFaltantes > 0 ? puntosFaltantes * 0.0033 : 0;

                double tarjeta = puntosFaltantes > 0 ? (puntosFaltantes * 0.0033) + ((puntosFaltantes * 0.0033) * 0.029) + 2.5 + (((puntosFaltantes * 0.0033) * 0.029) + 2.5) * 0.16 : 0;

                resultado.Data = new EvaluacionPagoDTO
                {
                    PuntosDisponibles = puntosDisponibles,
                    PuntosCarrito = puntosCarrito,
                    PuntosFaltantes = puntosFaltantes,

                    Deposito = Convert.ToDecimal(deposito.ToString("N2")),
                    Tarjeta = Convert.ToDecimal(tarjeta.ToString("N2"))
                };
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<List<ProcesaCarritoResultado>>> ProcesarCarrito(ProcesarCarritoRequest pPremio, int idOrigen)
        {
            Respuesta<List<ProcesaCarritoResultado>> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
                Data = []
            };

            try
            {
                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo && u.Bloqueado == false))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioSinAcceso;
                    resultado.Mensaje = CodigoDeError.UsuarioSinAcceso.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                var usuario = DBContext.Usuarios
                    .Include(us => us.IdProgramaNavigation)
                    .Include(us => us.IdCediNavigation)
                        .Include(us => us.IdColoniaNavigation)
                    .First(u => u.Id == pPremio.IdUsuario);

                var carritoPagoPendiente = await DBContext.Carritos
                    .FirstOrDefaultAsync(x => x.IdUsuario == pPremio.IdUsuario
                        && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente);

                if (carritoPagoPendiente != null)
                {
                    var consultaPago = await DBContext.HistorialCompraPuntos
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync(x => x.IdTransaccionLog != null
                            && x.IdTransaccionLog == carritoPagoPendiente.IdTransaccionLog);

                    if (consultaPago == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.PagoPuntosNoEncontrado;
                        resultado.Mensaje = CodigoDeError.PagoPuntosNoEncontrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (consultaPago.IdEstatusPago != (int)TipoEstatusPago.Confirmado)
                    {
                        resultado.Codigo = (int)CodigoDeError.PagoPuntosNoEncontrado;
                        resultado.Mensaje = CodigoDeError.PagoPuntosNoEncontrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    pPremio.Nombre = consultaPago.Nombre ?? usuario.NombreCompleto ?? string.Empty;
                    pPremio.Email = consultaPago.Email ?? usuario.Email ?? string.Empty;
                    pPremio.Telefono = consultaPago.Telefono ?? usuario.Celular ?? string.Empty;
                    pPremio.Direccion = new DireccionRequest();
                    pPremio.Direccion.Telefono = consultaPago?.TelefonoAlterno ?? usuario.Telefono ?? string.Empty;
                    pPremio.Direccion.Calle = consultaPago?.Calle ?? usuario.Calle ?? string.Empty;
                    pPremio.Direccion.NumeroExterior = consultaPago?.NumeroExterior ?? usuario.NumeroExterior ?? string.Empty;
                    pPremio.Direccion.NumeroInterior = consultaPago?.NumeroInterior;
                    pPremio.Direccion.CodigoPostal = consultaPago?.CodigoPostal ?? usuario.IdColoniaNavigation?.Cp ?? string.Empty;
                    pPremio.Direccion.IdColonia = consultaPago?.IdColonia ?? usuario.IdColonia ?? 30645;
                    pPremio.Direccion.Ciudad = consultaPago?.Ciudad ?? usuario.IdColoniaNavigation?.Ciudad ?? string.Empty;
                    pPremio.Direccion.CalleInicio = consultaPago?.CalleInicio ?? usuario.CalleInicio ?? string.Empty;
                    pPremio.Direccion.CalleFin = consultaPago?.CalleFin ?? usuario.CalleFin ?? string.Empty;
                    pPremio.Direccion.Referencias = consultaPago?.Referencias ?? usuario.Referencias ?? string.Empty;
                    pPremio.IdTransaccionLog = consultaPago?.IdTransaccionLog;
                }

                var valida = Extensiones.ValidateRequest(pPremio);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                if (DBContext.Carritos.Any(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion && pPremio.IdTransaccionLog == null))
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoPendienteLiberacion;
                    resultado.Mensaje = CodigoDeError.DepositoPendienteLiberacion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var carrito = pPremio.IdTransaccionLog != null
                    ? await DBContext.Carritos
                        .Include(us => us.IdPremioNavigation)
                        .Include(x => x.IdTarjetaNavigation)
                        .Where(x => x.IdUsuario == usuario.Id && x.IdTransaccionLog == pPremio.IdTransaccionLog && (x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion || x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                        .ToListAsync()
                    : await DBContext.Carritos
                        .Include(us => us.IdPremioNavigation)
                        .Include(x => x.IdTarjetaNavigation)
                        .Where(x => x.IdUsuario == usuario.Id && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)
                        .ToListAsync();

                if (carrito == null || carrito.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (carrito.Any(x => x.IdTransaccionLog != null))
                {
                    resultado.IdTransaccion = carrito.Where(x => x.IdTransaccionLog != null).Select(x => x.IdTransaccionLog).First();
                }

                bool carritoConPremiofisico = carrito.Any(x => x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && x.IdPremioNavigation.IdTipoDeEnvio == (int)TipoDeEnvio.Normal);

                if (carritoConPremiofisico)
                {
                    if (pPremio.Direccion == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.DireccionRequerida;
                        resultado.Mensaje = CodigoDeError.DireccionRequerida.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var validaDireccion = Extensiones.ValidateRequest(pPremio.Direccion);

                    if (!validaDireccion.Exitoso)
                    {
                        resultado.Codigo = validaDireccion.Codigo;
                        resultado.Mensaje = validaDireccion.Mensaje;
                        resultado.Exitoso = validaDireccion.Exitoso;

                        return resultado;
                    }
                }

                string prefijoRMS = await DBContext.PrefijosRms.Where(x => x.IdCanal == usuario.IdProgramaNavigation.IdCanal && x.IdZona == usuario.IdCediNavigation.IdZona).Select(x => x.Prefijo).FirstAsync();

                int idPeriodo = await DBContext.Periodos.Where(x => x.Fecha.Year == DateTime.Now.Year && x.Fecha.Month == DateTime.Now.Month).Select(x => x.Id).FirstAsync();

                int idSda = await DBContext.SubconceptosDeAcumulacions.Where(x => x.IdConceptoDeAcumulacionNavigation.IdCanal == usuario.IdProgramaNavigation.IdCanal && x.IdConceptoDeAcumulacionNavigation.Codigo.Equals("R9")).Select(x => x.Id).FirstAsync();

                int puntosCarrito = carrito.Sum(c => c.Puntos);

                var saldoActual = DBContext.Movimientos.Where(S => S.IdUsuario == pPremio.IdUsuario).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefault();

                if (!(puntosCarrito <= saldoActual))
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                int countRedenciones = 0;

                int puntosPremio = 0;

                foreach (var premio in carrito)
                {
                    var estrategia = DBContext.Database.CreateExecutionStrategy();

                    await estrategia.ExecuteAsync(async () =>
                    {
                        await using var transaction = await DBContext.Database.BeginTransactionAsync();

                        try
                        {
                            //Obtenemos el saldo actual del usuario
                            saldoActual = await DBContext.Movimientos.Where(S => S.IdUsuario == pPremio.IdUsuario).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefaultAsync();

                            int saldoNuevo = saldoActual - premio.Puntos;

                            puntosPremio += premio.Puntos;

                            usuario.BitacoraDeUsuarios.Add(new()
                            {
                                IdUsuario = pPremio.IdUsuario,
                                IdTipoDeOperacion = (int)TipoOperacion.ProcesarCarrito,
                                FechaReg = DateTime.Now,
                                IdOperdorReg = pPremio.IdOperador,
                                Notas = TipoOperacion.ProcesarCarrito.GetDescription(),
                                IdOrigen = idOrigen
                            });

                            premio.IdTransaccionLog = resultado.IdTransaccion;

                            // Registramos el movimiento
                            Movimiento regMovimeinto = new()
                            {
                                IdUsuario = pPremio.IdUsuario,
                                IdPeriodo = idPeriodo,
                                IdSda = idSda,
                                Puntos = premio.Puntos,
                                Saldo = saldoNuevo,
                                Comentario = null,
                                FechaReg = DateTime.Now,
                                IdOrigen = idOrigen,
                                IdOperadorReg = null,
                                IdTransaccionLog = resultado.IdTransaccion
                            };

                            usuario.Movimientos.Add(regMovimeinto);

                            await DBContext.SaveChangesAsync();

                            Redencione regRedencion = new()
                            {
                                IdUsuario = pPremio.IdUsuario,
                                IdMovimiento = regMovimeinto.Id,
                                IdPremio = premio.IdPremio,
                                Puntos = premio.Puntos,
                                Solicitante = pPremio.Nombre,
                                Cantidad = premio.Cantidad,
                                Email = pPremio.Email,
                                Telefono = pPremio.Telefono,
                                FechaPromesa = DateOnly.FromDateTime(DateTime.Now.AddDays(premio.IdPremioNavigation.Diaspromesa ?? 20)),
                                IdEstatusRedencion = (int)TipoEstatusRedencion.Solicitado,
                                IdOrigen = idOrigen,
                                IdTransaccionLog = resultado.IdTransaccion,
                                FolioTarjeta = premio.IdTarjetaNavigation?.Folio,
                                NoTarjeta = premio.IdTarjetaNavigation?.NoTarjeta
                            };

                            if (pPremio.Direccion != null && premio.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico)
                            {
                                regRedencion.TelefonoAlterno = pPremio.Direccion.Telefono;
                                regRedencion.Calle = pPremio.Direccion.Calle;
                                regRedencion.NumeroExterior = pPremio.Direccion.NumeroExterior;
                                regRedencion.NumeroInterior = pPremio.Direccion.NumeroInterior;
                                regRedencion.CodigoPostal = pPremio.Direccion.CodigoPostal;
                                regRedencion.IdColonia = pPremio.Direccion.IdColonia;
                                regRedencion.Ciudad = pPremio.Direccion.Ciudad;
                                regRedencion.CalleInicio = pPremio.Direccion.CalleInicio;
                                regRedencion.CalleFin = pPremio.Direccion.CalleFin;
                                regRedencion.Referencias = pPremio.Direccion.Referencias;
                            }

                            usuario.Redenciones.Add(regRedencion);

                            await DBContext.SaveChangesAsync();

                            //Asignación de Folio RMS.
                            var folioRMS = string.Empty;

                            folioRMS = prefijoRMS + (regRedencion.Id + 100000000).ToString().Substring(1);
                            regRedencion.FolioRms = folioRMS;

                            await DBContext.SaveChangesAsync();

                            //Si el premio es digital consume API de MarketPlace
                            //Solo de recarga y de código
                            if (premio.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital
                                && (premio.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Recarga || premio.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Codigo))
                            {
                                RequestApiCPD requestApiCPD = new()
                                {
                                    IdUsuario = premio.IdUsuario,
                                    IdCarrito = premio.Id,
                                    IdPremio = premio.IdPremio,
                                    IdTipoDePremio = premio.IdPremioNavigation.IdTipoDePremio,
                                    IdTransaccion = resultado.IdTransaccion ?? Guid.NewGuid(),
                                    Transaccion = new()
                                    {
                                        sku = premio.IdPremioNavigation.Sku,
                                        cantidad = premio.Cantidad,
                                        id_cliente = premio.IdUsuario.ToString(),
                                        id_compra = premio.Id.ToString(),
                                        correo_e = premio.IdUsuarioNavigation.Email,
                                        numero_recarga = premio.TelefonoRecarga
                                    }
                                };

                                var CPD = await _api.RedimePremiosDigitales(requestApiCPD);

                                if (!CPD.Exitoso || CPD.Data == null || CPD.Data.Count == 0)
                                {
                                    throw new Exception(TipoExcepcion.MKTError.GetDescription());
                                }

                                foreach (var apiResult in CPD.Data)
                                {
                                    ProcesaCarritoResultado procesar = apiResult;

                                    procesar.Premio = premio.IdPremioNavigation.Nombre;
                                    procesar.FechaPromesa = DateTime.Now.AddDays(premio.IdPremioNavigation.Diaspromesa ?? 20);

                                    resultado.Data.Add(procesar);

                                    CodigosRedimido detalleRedencion = new()
                                    {
                                        IdCarrito = premio.Id,
                                        IdRedencion = regRedencion.Id,
                                        FechaReg = fechaActual,
                                        Codigo = procesar.Codigo,
                                        Folio = procesar.Folio,
                                        Pin = procesar.Pin,
                                        FolioRms = folioRMS,
                                        Motivo = procesar.Motivo,
                                        TelefonoRecarga = premio.TelefonoRecarga,
                                        IdTransaccionLog = resultado.IdTransaccion
                                    };

                                    usuario.CodigosRedimidos.Add(detalleRedencion);
                                }

                                if (!CPD.Data.Any(x => x.Success == 1))
                                {
                                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.NoProcesado;
                                    regRedencion.IdEstatusRedencion = (int)TipoEstatusRedencion.Cancelado;
                                    regMovimeinto.Puntos = 0;
                                    regMovimeinto.Saldo = saldoActual;

                                }
                                else
                                {
                                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Procesado;
                                    regRedencion.IdEstatusRedencion = (int)TipoEstatusRedencion.Entregado;
                                }

                                await DBContext.SaveChangesAsync();
                            }
                            else
                            {
                                CodigosRedimido detalleRedencion = new()
                                {
                                    IdCarrito = premio.Id,
                                    IdRedencion = regRedencion.Id,
                                    FechaReg = fechaActual,
                                    FolioRms = folioRMS,
                                    IdTransaccionLog = resultado.IdTransaccion,
                                    TelefonoRecarga = premio.TelefonoRecarga
                                };

                                usuario.CodigosRedimidos.Add(detalleRedencion);

                                premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Procesado;

                                await DBContext.SaveChangesAsync();

                                resultado.Data.Add(new ProcesaCarritoResultado
                                {
                                    IdCarrito = premio.Id,
                                    IdPremio = premio.IdPremio,
                                    Folio = folioRMS,
                                    Premio = premio.IdPremioNavigation.Nombre,
                                    Motivo = "Canje exitosa",
                                    FechaPromesa = DateTime.Now.AddDays(premio.IdPremioNavigation.Diaspromesa ?? 20),
                                    Success = 1
                                });

                            }

                            await transaction.CommitAsync();

                            countRedenciones++;

                            if (regRedencion.IdEstatusRedencion != (int)TipoEstatusRedencion.Cancelado)
                            {
                                await appEmail.ComprobanteDeCanje(TipoMensajeria.Email, TipoUsuario.Usuario, usuario.Id, regRedencion.Id, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Fatal(ex, "ProcesarCarrito(ProcesarCarritoRequest, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);

                            await transaction.RollbackAsync();
                        }
                    });
                }

                if (countRedenciones > 0)
                {
                    resultado.Mensaje = MensajeApp.CanjeExitoso.GetDescription();
                }
                else
                {
                    resultado.Codigo = (int)CodigoDeError.FalloAgregarRedencion;
                    resultado.Mensaje = CodigoDeError.FalloAgregarPremio.GetDescription();
                    resultado.Exitoso = false;
                }

                if (resultado.Codigo == (int)CodigoDeError.OK && pPremio.IdOperador != null) bitacora.BitacoraDeOperadores(pPremio.IdOperador.Value, (int)TipoOperacion.ProcesarCarrito, pPremio.IdUsuario);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ProcesarCarrito(ProcesarCarritoRequest, int32) => IdUsuario::{usuario}", pPremio.IdUsuario);
            }

            return resultado;
        }

        public async Task<Respuesta<List<ProcesaCarritoResultado>, OpenPayDetails>> ProcesarCarritoConTarjeta(PasarelaCarritoRequest pPuntos, int idOrigen)
        {
            Respuesta<List<ProcesaCarritoResultado>, OpenPayDetails> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pPuntos);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var validarUsuario = DBContext.Usuarios.Any(x => x.Id == pPuntos.IdUsuario);

                if (!validarUsuario)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                bool carritoConPremiofisico = await DBContext.Carritos
                    .AnyAsync(x => x.IdUsuario == pPuntos.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso
                        && x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico
                        && x.IdPremioNavigation.IdTipoDeEnvio == (int)TipoDeEnvio.Normal);

                if (carritoConPremiofisico)
                {
                    if (pPuntos.Direccion == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.DireccionRequerida;
                        resultado.Mensaje = CodigoDeError.DireccionRequerida.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var validaDireccion = Extensiones.ValidateRequest(pPuntos.Direccion);

                    if (!validaDireccion.Exitoso)
                    {
                        resultado.Codigo = validaDireccion.Codigo;
                        resultado.Mensaje = validaDireccion.Mensaje;
                        resultado.Exitoso = validaDireccion.Exitoso;

                        return resultado;
                    }
                }

                var validarPuntos = EvaluacionPago(pPuntos.IdUsuario, idOrigen);

                if (!validarPuntos.Exitoso || validarPuntos.Data == null)
                {
                    resultado.Codigo = validarPuntos.Codigo;
                    resultado.Mensaje = validarPuntos.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                EvaluacionPagoDTO infoPuntos = validarPuntos.Data;

                if (infoPuntos.PuntosFaltantes <= 0)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorDesconocido;
                    resultado.Mensaje = CodigoDeError.ErrorDesconocido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios.Include(x => x.IdProgramaNavigation).First(x => x.Id == pPuntos.IdUsuario);

                var fechaActual = DateTime.Now;

                var idPeriodo = DBContext.Periodos.First(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month).Id;

                int idSDA = DBContext.SubconceptosDeAcumulacions
                    .Where(x => x.IdConceptoDeAcumulacionNavigation.Codigo.Equals("C17")
                        && x.IdConceptoDeAcumulacionNavigation.IdCanal == usuario.IdProgramaNavigation.IdCanal)
                    .First().Id;

                var comprar = await _openPay.CreditCard(new CargoOpenPayRequest
                {
                    SourceId = pPuntos.Token_id,
                    Amount = infoPuntos.Tarjeta,
                    DeviceSessionId = pPuntos.DeviceSessionId,
                    Customer = new CustomerRequest
                    {
                        Id = usuario.Id,
                        Code = usuario.Cuc,
                        Name = usuario.Nombre ?? string.Empty,
                        LastName = (usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno).Trim(),
                        PhoneNumber = usuario.Celular,
                        Email = usuario.Email
                    }
                });

                if (!comprar.Exitoso)
                {
                    resultado.Codigo = comprar.Codigo;
                    resultado.Mensaje = comprar.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                var carrito = await DBContext.Carritos.Where(x => x.IdUsuario == pPuntos.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).ToListAsync();

                var idTran = carrito.Select(x => x.IdTransaccionLog).FirstOrDefault();

                await DBContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    await using var transaction = await DBContext.Database.BeginTransactionAsync();

                    try
                    {
                        usuario.HistorialCompraPuntos.Add(new HistorialCompraPunto
                        {
                            IdTipoPago = (int)TipoPago.Tarjeta,
                            PuntosTotales = infoPuntos.PuntosCarrito,
                            PuntosFaltantes = infoPuntos.PuntosFaltantes,
                            Monto = infoPuntos.Tarjeta,
                            FechaReg = fechaActual,
                            IdOrigen = idOrigen,
                            IdTransaccionLog = idTran,
                            IdEstatusPago = (int)TipoEstatusPago.EnProceso,
                            IdOpenPay = comprar?.Data?.Id,

                            Nombre = pPuntos.Nombre,
                            Email = pPuntos.Email,
                            Telefono = pPuntos.Telefono,
                            TelefonoAlterno = pPuntos.Direccion?.Telefono,
                            Calle = pPuntos?.Direccion?.Calle,
                            NumeroExterior = pPuntos?.Direccion?.NumeroExterior,
                            NumeroInterior = pPuntos?.Direccion?.NumeroInterior,
                            CodigoPostal = pPuntos?.Direccion?.CodigoPostal,
                            IdColonia = pPuntos?.Direccion?.IdColonia,
                            Ciudad = pPuntos?.Direccion?.Ciudad,
                            CalleInicio = pPuntos?.Direccion?.CalleInicio,
                            CalleFin = pPuntos?.Direccion?.CalleFin,
                            Referencias = pPuntos?.Direccion?.Referencias
                        });

                        carrito.ForEach(x =>
                        {
                            x.IdEstatusCarrito = (int)TipoEstatusCarrito.PagoPendiente;
                        });

                        await DBContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();

                        throw;
                    }
                });

                if (comprar?.Data?.PaymentMethod != null)
                {
                    resultado.Exitoso = true;
                    resultado.Mensaje = comprar.Mensaje;
                    resultado.Details = new OpenPayDetails
                    {
                        Id = comprar.Data.Id,
                        Type = comprar.Data.PaymentMethod.Type,
                        Url = comprar.Data.PaymentMethod.Url
                    };

                    return resultado;
                }

                var estrategia = DBContext.Database.CreateExecutionStrategy();

                await estrategia.ExecuteAsync(async () =>
                {
                    await using var transaction = await DBContext.Database.BeginTransactionAsync();

                    try
                    {
                        usuario.BitacoraDeUsuarios.Add(new()
                        {
                            IdTipoDeOperacion = (int)TipoOperacion.CompraPuntos,
                            FechaReg = fechaActual,
                            Notas = TipoOperacion.CompraPuntos.GetDescription(),
                            IdOrigen = idOrigen
                        });

                        int saldoActual = await DBContext.Movimientos.Where(S => S.IdUsuario == pPuntos.IdUsuario).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefaultAsync();

                        usuario.Movimientos.Add(new Movimiento
                        {
                            IdPeriodo = idPeriodo,
                            IdSda = idSDA,
                            Puntos = infoPuntos.PuntosFaltantes,
                            Saldo = saldoActual + infoPuntos.PuntosFaltantes,
                            FechaReg = fechaActual,
                            IdOrigen = idOrigen,
                            IdTransaccionLog = idTran
                        });

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();

                        throw;
                    }
                });

                var procesarCarrito = await ProcesarCarrito(pPuntos, idOrigen);

                if (!procesarCarrito.Exitoso)
                {
                    resultado.Codigo = procesarCarrito.Codigo;
                    resultado.Mensaje = MensajeApp.CompraPuntosConErrorEnCanje.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = procesarCarrito.Data;

                resultado.Mensaje = MensajeApp.CompraPuntosExitosa.GetDescription();

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<Empty>> ProcesarCarritoPorDeposito(ProcesarCarritoRequest pUsuario, int idOrigen)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var validarUsuario = DBContext.Usuarios.Any(x => x.Id == pUsuario.IdUsuario);

                if (!validarUsuario)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                bool carritoConPremiofisico = await DBContext.Carritos
                    .AnyAsync(x => x.IdUsuario == pUsuario.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso
                        && x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico
                        && x.IdPremioNavigation.IdTipoDeEnvio == (int)TipoDeEnvio.Normal);

                if (carritoConPremiofisico)
                {
                    if (pUsuario.Direccion == null)
                    {
                        resultado.Codigo = (int)CodigoDeError.DireccionRequerida;
                        resultado.Mensaje = CodigoDeError.DireccionRequerida.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var validaDireccion = Extensiones.ValidateRequest(pUsuario.Direccion);

                    if (!validaDireccion.Exitoso)
                    {
                        resultado.Codigo = validaDireccion.Codigo;
                        resultado.Mensaje = validaDireccion.Mensaje;
                        resultado.Exitoso = validaDireccion.Exitoso;

                        return resultado;
                    }
                }

                var validarPuntos = EvaluacionPago(pUsuario.IdUsuario, idOrigen);

                if (!validarPuntos.Exitoso || validarPuntos.Data == null)
                {
                    resultado.Codigo = validarPuntos.Codigo;
                    resultado.Mensaje = validarPuntos.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                EvaluacionPagoDTO infoPuntos = validarPuntos.Data;

                if (infoPuntos.PuntosFaltantes <= 0)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorDesconocido;
                    resultado.Mensaje = CodigoDeError.ErrorDesconocido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                    .Include(x => x.IdProgramaNavigation)
                    .Include(x => x.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                    .First(x => x.Id == pUsuario.IdUsuario);

                var fechaActual = DateTime.Now;

                var estrategia = DBContext.Database.CreateExecutionStrategy();

                await estrategia.ExecuteAsync(async () =>
                {
                    await using var transaction = await DBContext.Database.BeginTransactionAsync();

                    try
                    {
                        var idTran = usuario.Carritos.Where(x => x.IdUsuario == pUsuario.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).Select(x => x.IdTransaccionLog).FirstOrDefault() ?? Guid.NewGuid();

                        usuario.BitacoraDeUsuarios.Add(new()
                        {
                            IdTipoDeOperacion = (int)TipoOperacion.SolicitudCompraPuntos,
                            FechaReg = fechaActual,
                            Notas = TipoOperacion.SolicitudCompraPuntos.GetDescription(),
                            IdOrigen = idOrigen
                        });

                        usuario.HistorialCompraPuntos.Add(new HistorialCompraPunto
                        {
                            IdTipoPago = (int)TipoPago.Deposito,
                            PuntosTotales = infoPuntos.PuntosCarrito,
                            PuntosFaltantes = infoPuntos.PuntosFaltantes,
                            Monto = infoPuntos.Deposito,
                            FechaReg = fechaActual,
                            IdOrigen = idOrigen,
                            IdTransaccionLog = idTran,
                            IdEstatusPago = (int)TipoEstatusPago.EnProceso,

                            Nombre = pUsuario.Nombre,
                            Email = pUsuario.Email,
                            Telefono = pUsuario.Telefono,
                            TelefonoAlterno = pUsuario.Direccion?.Telefono,
                            Calle = pUsuario?.Direccion?.Calle,
                            NumeroExterior = pUsuario?.Direccion?.NumeroExterior,
                            NumeroInterior = pUsuario?.Direccion?.NumeroInterior,
                            CodigoPostal = pUsuario?.Direccion?.CodigoPostal,
                            IdColonia = pUsuario?.Direccion?.IdColonia,
                            Ciudad = pUsuario?.Direccion?.Ciudad,
                            CalleInicio = pUsuario?.Direccion?.CalleInicio,
                            CalleFin = pUsuario?.Direccion?.CalleFin,
                            Referencias = pUsuario?.Direccion?.Referencias
                        });

                        usuario.Carritos.ToList().ForEach(x =>
                        {
                            x.IdEstatusCarrito = (int)TipoEstatusCarrito.EnValidacion;
                            x.IdTransaccionLog = idTran;
                        });

                        await DBContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();

                        throw;
                    }
                });

                resultado.Mensaje = MensajeApp.CompraPuntosExitosaDepostio.GetDescription();

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<HistorialCompraPuntosDTO>> ConsultarDepositos(int? idUsuario)
        {
            Respuesta<List<HistorialCompraPuntosDTO>> resultado = new();

            try
            {
                var historial = DBContext.HistorialCompraPuntos
                    .Where(x => x.IdTipoPago == (int)TipoPago.Deposito
                        && x.IdEstatusPago == (int)TipoEstatusPago.EnProceso
                        && (idUsuario == null || x.IdUsuario == idUsuario))
                    .ToList();

                resultado.Data = mapper.Map<List<HistorialCompraPuntosDTO>>(historial);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<List<ProcesaCarritoResultado>>> LiberarDeposito(RequestByIdUsuario pUsuario, string folio, bool? liberar)
        {
            Respuesta<List<ProcesaCarritoResultado>> resultado = new();

            try
            {
                if (liberar == null)
                {
                    resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
                    resultado.Mensaje = CodigoDeError.PropiedadInvalida.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var operador = await DBContext.Operadores.FirstOrDefaultAsync(x => x.Id == pUsuario.IdOperador);

                if (operador == null)
                {
                    resultado.Codigo = (int)CodigoDeError.OperadorInvalido;
                    resultado.Mensaje = CodigoDeError.OperadorInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var compraPuntos = await DBContext.HistorialCompraPuntos
                    .Include(x => x.IdUsuarioNavigation)
                        .ThenInclude(x => x.IdColoniaNavigation)
                            .ThenInclude(x => x.IdMunicipioNavigation)
                                .ThenInclude(x => x.IdEstadoNavigation)
                    .Include(x => x.IdUsuarioNavigation.IdProgramaNavigation)
                    .Where(x => x.IdUsuario == pUsuario.IdUsuario
                        && x.IdTipoPago == (int)TipoPago.Deposito
                        && x.IdEstatusPago == (int)TipoEstatusPago.EnProceso)
                    .FirstOrDefaultAsync();

                if (compraPuntos == null)
                {
                    resultado.Codigo = (int)CodigoDeError.DepositoNoEncontrado;
                    resultado.Mensaje = CodigoDeError.DepositoNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = compraPuntos.IdUsuarioNavigation;

                var fechaActual = DateTime.Now;

                if (liberar.Value)
                {
                    if (string.IsNullOrEmpty(usuario.Email))
                    {
                        resultado.Codigo = (int)CodigoDeError.EmailNoRegistrado;
                        resultado.Mensaje = CodigoDeError.EmailNoRegistrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (string.IsNullOrEmpty(usuario.Celular))
                    {
                        resultado.Codigo = (int)CodigoDeError.CelularNoRegistrado;
                        resultado.Mensaje = CodigoDeError.CelularNoRegistrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (string.IsNullOrEmpty(usuario.Calle)
                        || string.IsNullOrEmpty(usuario.NumeroExterior)
                        || string.IsNullOrEmpty(usuario.IdColoniaNavigation?.Cp)
                        || usuario.IdColonia == null
                        || string.IsNullOrEmpty(usuario.IdColoniaNavigation.Ciudad)
                        || string.IsNullOrEmpty(usuario.CalleInicio)
                        || string.IsNullOrEmpty(usuario.CalleFin)
                        || string.IsNullOrEmpty(usuario.Referencias))
                    {
                        resultado.Codigo = (int)CodigoDeError.DireccionInvalida;
                        resultado.Mensaje = CodigoDeError.DireccionInvalida.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var idPeriodo = DBContext.Periodos.First(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month).Id;

                    int idSDA = DBContext.SubconceptosDeAcumulacions
                        .Where(x => x.IdConceptoDeAcumulacionNavigation.Codigo.Equals("C17")
                            && x.IdConceptoDeAcumulacionNavigation.IdCanal == usuario.IdProgramaNavigation.IdCanal)
                        .First().Id;

                    var estrategia = DBContext.Database.CreateExecutionStrategy();

                    await estrategia.ExecuteAsync(async () =>
                    {
                        await using var transaction = await DBContext.Database.BeginTransactionAsync();

                        try
                        {
                            operador.BitacoraDeOperadoreIdOperadorNavigations.Add(new BitacoraDeOperadore
                            {
                                IdTipoDeOperacion = (int)TipoOperacion.CompraPuntos,
                                FechaReg = fechaActual,
                                Notas = TipoOperacion.CompraPuntos.GetDescription(),
                                IdUsuarioAftd = pUsuario.IdUsuario
                            });

                            usuario.BitacoraDeUsuarios.Add(new BitacoraDeUsuario
                            {
                                IdTipoDeOperacion = (int)TipoOperacion.CompraPuntos,
                                FechaReg = fechaActual,
                                Notas = TipoOperacion.CompraPuntos.GetDescription(),
                                IdOperdorReg = pUsuario.IdOperador
                            });

                            var saldoActual = await DBContext.Movimientos.Where(S => S.IdUsuario == usuario.Id).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefaultAsync();

                            usuario.Movimientos.Add(new Movimiento
                            {
                                IdPeriodo = idPeriodo,
                                IdSda = idSDA,
                                Puntos = compraPuntos.PuntosFaltantes,
                                Saldo = saldoActual + compraPuntos.PuntosFaltantes,
                                FechaReg = fechaActual,
                                IdOrigen = (int)TipoOrigen.Web,
                                IdTransaccionLog = compraPuntos.IdTransaccionLog
                            });

                            compraPuntos.IdEstatusPago = (int)TipoEstatusPago.Confirmado;
                            compraPuntos.FechaMod = fechaActual;
                            compraPuntos.IdOperadorMod = pUsuario.IdOperador;
                            compraPuntos.Referencia = folio;

                            await DBContext.SaveChangesAsync();

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    });

                    var procesarCarrito = await ProcesarCarrito(new ProcesarCarritoRequest
                    {
                        IdUsuario = usuario.Id,
                        Nombre = compraPuntos.Nombre ?? (usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno).Trim(),
                        Email = compraPuntos.Email ?? usuario.Email ?? string.Empty,
                        Telefono = compraPuntos.Telefono ?? usuario.Celular ?? string.Empty,
                        Direccion = new DireccionRequest
                        {
                            Calle = compraPuntos.Calle ?? usuario.Calle ?? string.Empty,
                            NumeroExterior = compraPuntos.NumeroExterior ?? usuario.NumeroExterior ?? string.Empty,
                            NumeroInterior = compraPuntos.NumeroInterior ?? usuario.NumeroInterior ?? string.Empty,
                            CodigoPostal = compraPuntos?.IdColoniaNavigation?.Cp ?? usuario.IdColoniaNavigation.Cp ?? string.Empty,
                            IdColonia = compraPuntos?.IdColonia != null ? compraPuntos.IdColonia.Value : 30645,
                            Ciudad = compraPuntos?.IdColoniaNavigation?.Ciudad ?? usuario.IdColoniaNavigation.Ciudad ?? string.Empty,
                            CalleInicio = compraPuntos?.CalleInicio ?? usuario.CalleInicio ?? string.Empty,
                            CalleFin = compraPuntos?.CalleFin ?? usuario.CalleFin ?? string.Empty,
                            Referencias = compraPuntos?.Referencias ?? usuario.Referencias ?? string.Empty,
                            Telefono = compraPuntos?.Telefono ?? usuario.Telefono ?? string.Empty,
                        },
                        IdOperador = pUsuario.IdOperador,
                        IdTransaccionLog = compraPuntos?.IdTransaccionLog
                    }, (int)TipoOrigen.CallCenter);

                    resultado = procesarCarrito;

                    if (resultado.Exitoso)
                    {
                        resultado.Mensaje = CodigoDeError.PuntosLiberados.GetDescription();
                    }
                    else
                    {
                        resultado.Mensaje = CodigoDeError.PuntosLiberadosSinCanje.GetDescription();
                    }

                    return resultado;
                }
                else
                {
                    var carrito = await DBContext.Carritos
                        .Where(x => x.IdUsuario == pUsuario.IdUsuario
                            && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion
                            && x.IdTransaccionLog == compraPuntos.IdTransaccionLog)
                        .ToListAsync();

                    var estrategia = DBContext.Database.CreateExecutionStrategy();

                    await estrategia.ExecuteAsync(async () =>
                    {
                        await using var transaction = await DBContext.Database.BeginTransactionAsync();

                        try
                        {
                            compraPuntos.IdEstatusPago = (int)TipoEstatusPago.Cancelado;
                            compraPuntos.FechaMod = fechaActual;
                            compraPuntos.IdOperadorMod = pUsuario.IdOperador;

                            operador.BitacoraDeOperadoreIdOperadorNavigations.Add(new BitacoraDeOperadore
                            {
                                IdTipoDeOperacion = (int)TipoOperacion.CancelacionCompraPuntos,
                                FechaReg = fechaActual,
                                Notas = TipoOperacion.CancelacionCompraPuntos.GetDescription(),
                                IdUsuarioAftd = pUsuario.IdUsuario
                            });

                            carrito.ForEach(x =>
                            {
                                x.IdEstatusCarrito = (int)TipoEstatusCarrito.EnProceso;
                            });

                            await DBContext.SaveChangesAsync();

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    });
                }
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<int> ValidarOrigenTranferencia(string idOpenPay)
        {
            Respuesta<int> resultado = new();

            try
            {
                resultado.Data = DBContext.HistorialCompraPuntos
                    .Where(x => x.IdOpenPay != null && x.IdOpenPay.Equals(idOpenPay))
                    .Select(x => x.IdUsuarioNavigation.IdProgramaNavigation.IdCanal)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ValidarOrigenTranferencia(string) => IdOpenPay::{usuario}", idOpenPay);
            }

            return resultado;
        }

        public async Task<Respuesta<List<ProcesaCarritoResultado>>> LiberarTranferencia(string id)
        {
            Respuesta<List<ProcesaCarritoResultado>> resultado = new();

            try
            {
                var compraPuntos = await DBContext.HistorialCompraPuntos
                    .Include(x => x.IdUsuarioNavigation)
                        .ThenInclude(x => x.IdColoniaNavigation)
                            .ThenInclude(x => x.IdMunicipioNavigation)
                                .ThenInclude(x => x.IdEstadoNavigation)
                    .Include(x => x.IdUsuarioNavigation.IdProgramaNavigation)
                    .Where(x => x.IdOpenPay != null && x.IdOpenPay.Equals(id))
                    .FirstOrDefaultAsync();

                if (compraPuntos == null)
                {
                    resultado.Codigo = (int)CodigoDeError.LigaExpirada;
                    resultado.Mensaje = CodigoDeError.LigaExpirada.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (compraPuntos.IdEstatusPago == (int)TipoEstatusPago.Confirmado
                        && await DBContext.Carritos.AnyAsync(x => x.IdUsuario == compraPuntos.IdUsuario
                            && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                {
                    resultado.Mensaje = MensajeApp.CompraPuntosConErrorEnCanje.GetDescription();

                    return resultado;
                }

                if (compraPuntos.IdEstatusPago == (int)TipoEstatusPago.Confirmado)
                {
                    resultado.Mensaje = MensajeApp.CompraPuntosExitosa.GetDescription();

                    return resultado;
                }

                if (compraPuntos.IdEstatusPago != (int)TipoEstatusPago.EnProceso)
                {
                    resultado.Codigo = (int)CodigoDeError.LigaExpirada;
                    resultado.Mensaje = CodigoDeError.LigaExpirada.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var cargo = await _openPay.Charge(id);

                if (cargo.Data != null && cargo.Data.Status.Contains("completed"))
                {
                    var usuario = compraPuntos.IdUsuarioNavigation;

                    var fechaActual = DateTime.Now;

                    if (string.IsNullOrEmpty(usuario.Email))
                    {
                        resultado.Codigo = (int)CodigoDeError.EmailNoRegistrado;
                        resultado.Mensaje = CodigoDeError.EmailNoRegistrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (string.IsNullOrEmpty(usuario.Celular))
                    {
                        resultado.Codigo = (int)CodigoDeError.CelularNoRegistrado;
                        resultado.Mensaje = CodigoDeError.CelularNoRegistrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    if (string.IsNullOrEmpty(usuario.Calle)
                        || string.IsNullOrEmpty(usuario.NumeroExterior)
                        || string.IsNullOrEmpty(usuario.IdColoniaNavigation?.Cp)
                        || usuario.IdColonia == null
                        || string.IsNullOrEmpty(usuario.Ciudad)
                        || string.IsNullOrEmpty(usuario.CalleInicio)
                        || string.IsNullOrEmpty(usuario.CalleFin)
                        || string.IsNullOrEmpty(usuario.Referencias))
                    {
                        resultado.Codigo = (int)CodigoDeError.DireccionInvalida;
                        resultado.Mensaje = CodigoDeError.DireccionInvalida.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var idPeriodo = DBContext.Periodos.First(x => x.Fecha.Year == fechaActual.Year && x.Fecha.Month == fechaActual.Month).Id;

                    int idSDA = DBContext.SubconceptosDeAcumulacions
                        .Where(x => x.IdConceptoDeAcumulacionNavigation.Codigo.Equals("C17")
                            && x.IdConceptoDeAcumulacionNavigation.IdCanal == usuario.IdProgramaNavigation.IdCanal)
                        .First().Id;

                    await DBContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                    {
                        await using var transaction = await DBContext.Database.BeginTransactionAsync();

                        try
                        {
                            usuario.BitacoraDeUsuarios.Add(new BitacoraDeUsuario
                            {
                                IdTipoDeOperacion = (int)TipoOperacion.CompraPuntos,
                                FechaReg = fechaActual,
                                Notas = TipoOperacion.CompraPuntos.GetDescription()
                            });

                            var saldoActual = await DBContext.Movimientos.Where(S => S.IdUsuario == usuario.Id).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefaultAsync();

                            usuario.Movimientos.Add(new Movimiento
                            {
                                IdPeriodo = idPeriodo,
                                IdSda = idSDA,
                                Puntos = compraPuntos.PuntosFaltantes,
                                Saldo = saldoActual + compraPuntos.PuntosFaltantes,
                                FechaReg = fechaActual,
                                IdOrigen = (int)TipoOrigen.Web,
                                IdTransaccionLog = compraPuntos.IdTransaccionLog
                            });

                            compraPuntos.IdEstatusPago = (int)TipoEstatusPago.Confirmado;
                            compraPuntos.FechaMod = fechaActual;

                            await DBContext.SaveChangesAsync();

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    });

                    var procesarCarrito = await ProcesarCarrito(new ProcesarCarritoRequest
                    {
                        IdUsuario = usuario.Id,
                        Nombre = compraPuntos.Nombre ?? (usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno).Trim(),
                        Email = compraPuntos.Email ?? usuario.Email ?? string.Empty,
                        Telefono = compraPuntos.Telefono ?? usuario.Celular ?? string.Empty,
                        Direccion = new DireccionRequest
                        {
                            Calle = compraPuntos.Calle ?? usuario.Calle ?? string.Empty,
                            NumeroExterior = compraPuntos.NumeroExterior ?? usuario.NumeroExterior ?? string.Empty,
                            NumeroInterior = compraPuntos.NumeroInterior ?? usuario.NumeroInterior ?? string.Empty,
                            CodigoPostal = compraPuntos?.IdColoniaNavigation?.Cp ?? usuario.IdColoniaNavigation.Cp ?? string.Empty,
                            IdColonia = compraPuntos?.IdColonia != null ? compraPuntos.IdColonia.Value : 30645,
                            Ciudad = compraPuntos?.Ciudad ?? usuario.IdColoniaNavigation.Ciudad ?? string.Empty,
                            CalleInicio = compraPuntos?.CalleInicio ?? usuario.CalleInicio ?? string.Empty,
                            CalleFin = compraPuntos?.CalleFin ?? usuario.CalleFin ?? string.Empty,
                            Referencias = compraPuntos?.Referencias ?? usuario.Referencias ?? string.Empty,
                            Telefono = compraPuntos?.Telefono ?? usuario.Telefono ?? string.Empty,
                        },
                        IdTransaccionLog = compraPuntos?.IdTransaccionLog
                    }, (int)TipoOrigen.Web);

                    resultado = procesarCarrito;

                    if (!resultado.Exitoso)
                    {
                        resultado.Mensaje = CodigoDeError.PuntosLiberadosSinCanje.GetDescription();
                    }
                }
                else
                {
                    await DBContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                    {
                        await using var transaction = await DBContext.Database.BeginTransactionAsync();

                        try
                        {
                            var carrito = await DBContext.Carritos
                            .Where(x => x.IdTransaccionLog != null && x.IdTransaccionLog == compraPuntos.IdTransaccionLog
                                && x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente)
                            .ToListAsync();

                            var guid = Guid.NewGuid();

                            carrito.ForEach(x =>
                            {
                                x.IdEstatusCarrito = (int)TipoEstatusCarrito.EnProceso;
                                x.IdTransaccionLog = guid;
                            });

                            compraPuntos.IdEstatusPago = (int)TipoEstatusPago.Fallido;

                            await DBContext.SaveChangesAsync();

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    });

                    resultado.Codigo = (int)CodigoDeError.LigaExpirada;
                    resultado.Mensaje = CodigoDeError.LigaExpirada.GetDescription();
                    resultado.Exitoso = false;
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "LiberarTranferencia(int32) => IdOpenPay::{usuario}", id);
            }

            return resultado;
        }

        public Respuesta<Empty> ExistePremioFisico(int idUsuario)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                resultado.Exitoso = DBContext.Carritos
                    .Where(x => x.IdUsuario == idUsuario
                        && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso
                        && x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico
                        && x.IdPremioNavigation.IdTipoDeEnvio == (int)TipoDeEnvio.Normal)
                    .Any();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ExistePremioFisico(int32) => IdUsuario::{usuario}", idUsuario);
            }

            return resultado;
        }

        public Respuesta<int> ConsultarTotalPremios(int idUsuario)
        {
            Respuesta<int> resultado = new();

            try
            {
                resultado.Data = DBContext.Carritos
                    .Where(x => x.IdUsuario == idUsuario
                        && (x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso
                        || x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnValidacion
                        || x.IdEstatusCarrito == (int)TipoEstatusCarrito.PagoPendiente))
                    .Count();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = 0;
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarTotalPremios(int32) => IdUsuario::{usuario}", idUsuario);
            }

            return resultado;
        }

        private CarritoDTO GetCarrito(List<CarritoModelDTO> carrito)
        {
            return new CarritoDTO
            {
                Total = carrito.Sum(x => x.Puntos),
                Carrito = carrito
                    .GroupBy(x => x.IdPremio)
                    .Select(g => new PremioCarritoDTO
                    {
                        IdPremio = g.Key,
                        IdTipoDeEnvio = g.First().IdTipoDeEnvio,
                        IdTipoPremio = g.First().IdTipoDePremio,
                        IdTipoTransaccion = g.FirstOrDefault()?.IdTipoTransaccion,
                        Sku = g.First().Sku,
                        Nombre = g.First().Nombre,
                        Imagen = g.FirstOrDefault()?.Imagen != null ? UrlPremios + g.First().Imagen : null,
                        TelefonoRecarga = g.FirstOrDefault()?.TelefonoRecarga,
                        Tarjeta = string.Join(", ",
                            g.Select(p => p?.NoTarjeta)
                            .Where(t => !string.IsNullOrEmpty(t))),
                        Cantidad = g.Sum(p => p.Cantidad),
                        Puntos = g.Sum(p => p.Puntos)
                    })
                    .ToList(),
                MontoPendiente = carrito.Any() ? DBContext.HistorialCompraPuntos
                                    .Where(x => x.IdUsuario == carrito.Select(x => x.IdUsuario).First()
                                            && x.IdTipoPago == (int)TipoPago.Deposito
                                            && x.IdEstatusPago == (int)TipoEstatusPago.EnProceso
                                    ).Select(x => x.Monto).FirstOrDefault()
                                    : 0
            };
        }
    }
}
