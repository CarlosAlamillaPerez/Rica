using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_biz.Extensions;
using Microsoft.EntityFrameworkCore;
using bepensa_models.DTO;
using bepensa_biz.Settings;
using Microsoft.Extensions.Options;
using bepensa_models;
using bepensa_models.ApiResponse;
using System.Security.Cryptography;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

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

        private readonly string UrlPremios;

        public CarritoProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premio,
                            Serilog.ILogger logger, IApi api, IAppEmail appEmail, IBitacora bitacora)
        {
            DBContext = context;
            _ajustes = ajustes.Value;
            _premio = premio.Value;
            _logger = logger;

            _api = api;
            this.appEmail = appEmail;
            this.bitacora = bitacora;

            UrlPremios = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;
        }

        public async Task<Respuesta<Empty>> AgregarPremio(AgregarPremioRequest pPremio, int idOrigen)
        {
            Respuesta<Empty> resultado = new();

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
                                IdOrigen = idOrigen
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

                var validaCarrito = DBContext.Carritos.Any(c => c.IdUsuario == pPremio.IdUsuario
                                                            && c.IdPremio == pPremio.IdPremio
                                                            && c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);

                if (!validaCarrito)
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = await DBContext.Usuarios
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(x => x.IdPremioNavigation)
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                            .ThenInclude(x => x.IdEstatusCarritoNavigation)
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(x => x.IdTarjetaNavigation)
                    .FirstAsync(u => u.Id == pPremio.IdUsuario);


                var carrito = usuario.Carritos.ToList();

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

                carrito = [.. carrito.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)];

                resultado.Mensaje = "El premio ha sido eliminado del carrito";

                resultado.Data = GetCarrito(carrito);

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

                resultado.Data = GetCarrito(carrito);

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

                carrito = [.. carrito.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)];

                resultado.Mensaje = "Carrito vacío";

                var url = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;

                resultado.Data = GetCarrito(carrito);

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
                            .Where(x => x.IdUsuario == pPremio.IdUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)
                            .ToList();

                if (carrito == null || carrito.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = GetCarrito(carrito);
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

        public async Task<Respuesta<List<ProcesaCarritoResultado>>> ProcesarCarrito(ProcesarCarritoRequest pPremio, int idOrigen)
        {
            Respuesta<List<ProcesaCarritoResultado>> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
                Data = []
            };

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

                var validaCarrito = DBContext.Carritos.Any(c => c.IdUsuario == pPremio.IdUsuario && c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);

                if (!validaCarrito)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                var usuario = DBContext.Usuarios
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(us => us.IdPremioNavigation)
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(x => x.IdTarjetaNavigation)
                    .Include(us => us.IdProgramaNavigation)
                    .Include(us => us.IdCediNavigation)

                    .First(u => u.Id == pPremio.IdUsuario);

                var carrito = usuario.Carritos;

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

                int puntosCarrito = usuario.Carritos.Sum(c => c.Puntos);

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
                            saldoActual = DBContext.Movimientos.Where(S => S.IdUsuario == pPremio.IdUsuario).OrderByDescending(s => s.Id).Select(s => s.Saldo).Take(1).FirstOrDefault();

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

                                var CPD = _api.RedimePremiosDigitales(requestApiCPD);

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
                    .Where(x => x.IdUsuario == idUsuario && x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)
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

        private CarritoDTO GetCarrito(List<Carrito> carrito)
        {
            return new CarritoDTO
            {
                Total = carrito.Sum(x => x.Puntos),
                Carrito = carrito
                    .GroupBy(x => x.IdPremio)
                    .Select(g => new PremioCarritoDTO
                    {
                        IdPremio = g.Key,
                        IdTipoDeEnvio = g.First().IdPremioNavigation.IdTipoDeEnvio,
                        IdTipoPremio = g.First().IdPremioNavigation.IdTipoDePremio,
                        IdTipoTransaccion = g.FirstOrDefault()?.IdPremioNavigation?.IdTipoTransaccion,
                        Sku = g.First().IdPremioNavigation.Sku,
                        Nombre = g.First().IdPremioNavigation.Nombre,
                        Imagen = g.FirstOrDefault()?.IdPremioNavigation.Imagen != null ? UrlPremios + g.First().IdPremioNavigation.Imagen : null,
                        TelefonoRecarga = g.FirstOrDefault()?.TelefonoRecarga,
                        Tarjeta = string.Join(", ",
                            g.Select(p => p.IdTarjetaNavigation?.NoTarjeta)
                            .Where(t => !string.IsNullOrEmpty(t))),
                        Cantidad = g.Sum(p => p.Cantidad),
                        Puntos = g.Sum(p => p.Puntos)
                    })
                    .ToList()
            };
        }
    }
}
