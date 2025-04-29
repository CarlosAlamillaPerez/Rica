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

namespace bepensa_biz.Proxies
{
    public class CarritoProxy : ProxyBase, ICarrito
    {
        private readonly GlobalSettings _ajustes;

        private readonly PremiosSettings _premio;

        private readonly IApi _api;

        public CarritoProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premio,
                            IApi api)
        {
            DBContext = context;
            _ajustes = ajustes.Value;
            _premio = premio.Value;

            _api = api;
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
                            IdTipoDeOperacion = (int)TipoDeOperacion.AgregaCarrito,
                            FechaReg = fechaSolicitud,
                            Notas = TipoDeOperacion.AgregaCarrito.GetDescription() + " SKU: " + premio.Sku
                        };

                        usuario.BitacoraDeUsuarios.Add(bdu);

                        for (int i = 0; i < pPremio.Cantidad; i++)
                        {
                            Carrito carrito = new Carrito()
                            {
                                IdPremio = pPremio.IdPremio,
                                Cantidad = 1,
                                IdEstatusCarrito = (int)TipoEstatusCarrito.EnProceso,
                                FechaReg = fechaSolicitud,
                                TelefonoRecarga = pPremio.TelefonoRecarga,
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
                });
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            }

            return resultado;
        }

        public async Task<Respuesta<CarritoDTO>> EliminarPremio(RequestByIdPremio pPremio)
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
                    .FirstAsync(u => u.Id == pPremio.IdUsuario);


                var carrito = usuario.Carritos;

                foreach (var premio in carrito.Where(x => x.IdPremio == pPremio.IdPremio))
                {
                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Cancelado;
                }

                usuario.BitacoraDeUsuarios.Add(new BitacoraDeUsuario
                {
                    IdTipoDeOperacion = (int)TipoOperacion.QuitaCarrito,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.QuitaCarrito.GetDescription()
                });

                DBContext.SaveChanges();

                carrito = [.. carrito.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso)];

                resultado.Mensaje = "El premio ha sido eliminado del carrito";

                var url = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;

                resultado.Data = new CarritoDTO
                {
                    Total = carrito.Sum(x => x.Puntos),
                    Carrito = carrito
                    .GroupBy(x => x.IdPremio)
                    .Select(g => new PremioCarritoDTO
                    {
                        IdPremio = g.Key,
                        Sku = g.First().IdPremioNavigation.Sku,
                        Nombre = g.First().IdPremioNavigation.Nombre,
                        Imagen = g.FirstOrDefault()?.IdPremioNavigation.Imagen != null ? url + g.First().IdPremioNavigation.Imagen : null,
                        Cantidad = g.Sum(p => p.Cantidad),
                        TelefonoRecarga = g.FirstOrDefault()?.TelefonoRecarga,
                        Puntos = g.Sum(p => p.Puntos)
                    })
                    .ToList()
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

        public async Task<Respuesta<CarritoDTO>> ModificarPremio(ActPremioRequest pPremio)
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

                    if (premio.IdTipoTransaccion == (int)TipoTransaccion.Recarga)
                    {
                        resultado.Codigo = (int)CodigoDeError.SoloUnaRecarga;
                        resultado.Mensaje = CodigoDeError.SoloUnaRecarga.GetDescription();
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
                            IdTipoDeOperacion = (int)TipoDeOperacion.ModificaCarrito,
                            FechaReg = fechaSolicitud,
                            Notas = TipoDeOperacion.ModificaCarrito.GetDescription() + " SKU: " + premio.Sku
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
                    catch (Exception)
                    {
                        resultado.Codigo = (int)CodigoDeError.FalloAgregarPremio;
                        resultado.Mensaje = CodigoDeError.FalloAgregarPremio.GetDescription();
                        resultado.Exitoso = false;

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

                    premio.IdEstatusCarrito = (int)TipoEstatusCarrito.Cancelado;

                    usuario.BitacoraDeUsuarios.Add(new()
                    {
                        IdUsuario = pPremio.IdUsuario,
                        IdTipoDeOperacion = (int)TipoDeOperacion.ModificaCarrito,
                        FechaReg = fechaSolicitud,
                        Notas = TipoDeOperacion.ModificaCarrito.GetDescription() + " SKU: " + premio.IdPremioNavigation.Sku
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

                var carrito = usuario.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso);

                var url = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;

                resultado.Data = new CarritoDTO
                {
                    Total = carrito.Sum(x => x.Puntos),
                    Carrito = carrito
                    .GroupBy(x => x.IdPremio)
                    .Select(g => new PremioCarritoDTO
                    {
                        IdPremio = g.Key,
                        Sku = g.First().IdPremioNavigation.Sku,
                        Nombre = g.First().IdPremioNavigation.Nombre,
                        Imagen = g.FirstOrDefault()?.IdPremioNavigation.Imagen != null ? url + g.First().IdPremioNavigation.Imagen : null,
                        Cantidad = g.Sum(p => p.Cantidad),
                        TelefonoRecarga = g.FirstOrDefault()?.TelefonoRecarga,
                        Puntos = g.Sum(p => p.Puntos)
                    })
                    .ToList()
                };
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;
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
                            .Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).ToList();

                if (carrito == null || carrito.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.CarritoVacío;
                    resultado.Mensaje = CodigoDeError.CarritoVacío.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var url = _ajustes.Produccion ? _premio.MultimediaPremio.UrlProd : _premio.MultimediaPremio.UrlQA;

                resultado.Data = new CarritoDTO
                {
                    Total = carrito.Sum(x => x.Puntos),
                    Carrito = carrito
                    .GroupBy(x => x.IdPremio)
                    .Select(g => new PremioCarritoDTO
                    {
                        IdPremio = g.Key,
                        Sku = g.First().IdPremioNavigation.Sku,
                        Nombre = g.First().IdPremioNavigation.Nombre,
                        Imagen = g.FirstOrDefault()?.IdPremioNavigation.Imagen != null ? url + g.First().IdPremioNavigation.Imagen : null,
                        TelefonoRecarga = g.FirstOrDefault()?.TelefonoRecarga,
                        Cantidad = g.Sum(p => p.Cantidad),
                        Puntos = g.Sum(p => p.Puntos)
                    })
                    .ToList()
                };
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
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
                    .Include(us => us.IdProgramaNavigation)
                    .Include(us => us.IdCediNavigation)
                    .First(u => u.Id == pPremio.IdUsuario);

                var carrito = usuario.Carritos;

                bool carritoconpremiofisico = carrito.Any(x => x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico);

                bool virtualpremio = carrito.Any(x => x.IdPremioNavigation.IdTipoDeEnvio == 14);

                if (carritoconpremiofisico)
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

                //Obtenemos el saldo actual del usuario
                var saldoActual = DBContext.Movimientos
                    .Where(S => S.IdUsuario == pPremio.IdUsuario)
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.Saldo)
                    .Take(1)
                    .FirstOrDefault();

                int puntosCarrito = usuario.Carritos.Sum(c => c.Puntos);


                if (!(puntosCarrito <= saldoActual))
                {
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdUsuario = pPremio.IdUsuario,
                    FechaReg = DateTime.Now,
                    IdTipoDeOperacion = (int)TipoDeOperacion.ProcesarCarrito,
                    Notas = TipoDeOperacion.ProcesarCarrito.GetDescription()
                };

                string prefijoRMS = await DBContext.PrefijosRms.Where(x => x.IdCanal == usuario.IdProgramaNavigation.IdCanal && x.IdZona == usuario.IdCediNavigation.IdZona).Select(x => x.Prefijo).FirstAsync();

                int idPeriodo = await DBContext.Periodos.Where(x => x.Fecha.Year == DateTime.Now.Year && x.Fecha.Month == DateTime.Now.Month).Select(x => x.Id).FirstAsync();

                int idSda = await DBContext.SubconceptosDeAcumulacions.Where(x => x.IdConceptoDeAcumulacionNavigation.IdCanal == usuario.IdProgramaNavigation.IdCanal && x.IdConceptoDeAcumulacionNavigation.Codigo.Equals("R9")).Select(x => x.Id).FirstAsync();

                DBContext.Database.BeginTransaction();

                foreach (var item in usuario.Carritos)
                {
                    int ContePremios = 0;
                    TipoEstatusRedencion EstatusRedencion = TipoEstatusRedencion.Solicitado;
                    int SaldoNuevo = 0;

                    saldoActual = saldoActual - item.Puntos;

                    SaldoNuevo = saldoActual;

                    bool ActualizaCarrito = true;

                    //Si el premio es digital  consume API de MarketPlace
                    if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && (item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Recarga || item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Codigo))
                    {
                        ActualizaCarrito = false;
                        RequestApiCPD requestApiCPD = new()
                        {
                            IdUsuario = item.IdUsuario,
                            IdCarrito = item.Id,
                            IdPremio = item.IdPremio,
                            IdTransaccion = (Guid)resultado.IdTransaccion,
                            Transaccion = new()
                            {
                                sku = item.IdPremioNavigation.Sku,
                                cantidad = item.Cantidad,
                                id_cliente = item.IdUsuario.ToString(),
                                id_compra = item.Id.ToString(),
                                correo_e = item.IdUsuarioNavigation.Email,
                                numero_recarga = item.TelefonoRecarga
                            }
                        };

                        var CPD = _api.RedimePremiosDigitales(requestApiCPD);

                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Recarga)
                        {
                            List<ProcesaCarritoResultado> preresultado = CPD.Data.Select(x => (ProcesaCarritoResultado)x).ToList();
                            preresultado.ForEach(x =>
                            {
                                string premio = usuario.Carritos.FirstOrDefault(y => y.Id == x.IdCarrito).IdPremioNavigation.Nombre;
                                x.Premio = premio;
                            });
                            resultado.Data.AddRange(preresultado);

                            CPD.Data.ForEach(x =>
                            {
                                usuario.CodigosRedimidos.Add(x);
                            });
                        }

                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Codigo)
                        {
                            List<ProcesaCarritoResultado> preresultado = CPD.Data.Select(x => (ProcesaCarritoResultado)x).ToList();
                            preresultado.ForEach(x =>
                            {
                                string premio = usuario.Carritos.FirstOrDefault(y => y.Id == x.IdCarrito).IdPremioNavigation.Nombre;
                                x.Premio = premio;
                            });
                            resultado.Data.AddRange(preresultado);

                            CPD.Data.ForEach(x =>
                            {
                                usuario.CodigosRedimidos.Add(x);
                            });
                        }

                        if (CPD.Codigo != (int)CodigoDeError.OK)
                        {

                            item.IdEstatusCarrito = (int)TipoEstatusCarrito.NoProcesado;
                            item.FechaRedencion = DateTime.Now;
                            //item.IdTransaccionLog = resultado.IdTransaccion;
                            goto Siguiente;
                        }


                        CPD.Data.ForEach(x =>
                        {
                            if (x.success == 0)
                            {
                                item.IdEstatusCarrito = item.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso ? (int)TipoEstatusCarrito.NoProcesado : item.IdEstatusCarrito;
                            }
                            else
                            {
                                item.IdEstatusCarrito = item.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso ? (int)TipoEstatusCarrito.Procesado : item.IdEstatusCarrito;
                            }
                            item.FechaRedencion = item.FechaRedencion == null ? DateTime.Now : item.FechaRedencion;
                            //item.IdTransaccionLog = item.IdTransaccionLog == null ? resultado.IdTransaccion : item.IdTransaccionLog;

                        });

                        if (item.IdEstatusCarrito == (int)TipoEstatusCarrito.NoProcesado)
                        {
                            goto Siguiente;
                        }

                        EstatusRedencion = TipoEstatusRedencion.Entregado;
                    }
                    else
                    {
                        EstatusRedencion = TipoEstatusRedencion.Solicitado;
                    }

                    //Registra en movimeintos el canje

                    Movimiento regMovimeinto = new()
                    {
                        IdUsuario = item.IdUsuario,
                        IdPeriodo = idPeriodo,
                        IdSda = idSda,
                        Puntos = item.Puntos,
                        Saldo = SaldoNuevo,
                        Comentario = null,
                        FechaReg = DateTime.Now,
                        IdOrigen = idOrigen,
                        IdOperadorReg = null,
                        IdTransaccionLog = resultado.IdTransaccion
                    };

                    usuario.Movimientos.Add(regMovimeinto);

                    DBContext.SaveChanges();

                    //inserta en tabla de Redenciones por unidad (uno por uno segun la cantidad solicitada en el canje)
                    while (ContePremios < item.Cantidad)
                    {
                        Redencione regRedencion = new()
                        {
                            IdUsuario = item.IdUsuario,
                            IdMovimiento = regMovimeinto.Id,
                            IdPremio = item.IdPremio,
                            Puntos = item.IdPremioNavigation.Puntos,
                            Solicitante = pPremio.Nombre,
                            Cantidad = item.Cantidad,
                            Email = pPremio.Email,
                            Telefono = pPremio.Telefono,
                            FechaPromesa = DateOnly.FromDateTime(DateTime.Now.AddDays(item.IdPremioNavigation.Diaspromesa ?? 20)),
                            IdEstatusRedencion = (int)EstatusRedencion,
                            IdOrigen = idOrigen,
                        };

                        if (pPremio.Direccion != null)
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

                        DBContext.SaveChanges();

                        var folioRMS = string.Empty;

                        folioRMS = prefijoRMS + (regRedencion.Id + 100000000).ToString().Substring(1);
                        regRedencion.FolioRms = folioRMS;

                        DBContext.SaveChanges();

                        //Para premios fisicos
                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Envio)
                        {
                            ProcesaCarritoResultado regFisico = new()
                            {
                                IdCarrito = item.Id,
                                IdPremio = item.IdPremio,
                                Folio = folioRMS,
                                Premio = item.IdPremioNavigation.Nombre,
                                TelefonoRecarga = item.TelefonoRecarga,
                                Motivo = "Canje exitosa",
                                FechaPromesa = fechaActual.AddDays(item.IdPremioNavigation.Diaspromesa ?? 20),
                                Success = 1
                            };

                            resultado.Data.Add(regFisico);

                            //Pool Envio de notificacion de canje
                            //if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && item.IdPremioNavigation.IdTipoDeTransacionPremio == (int)TipoTransaccionPremio.Envio && item.IdPremioNavigation.IdTipoDeEnvio == (int)TipoEnvio.FisicoProductos)
                            //{
                            //    EnvioEmail.Add(new() { Tipo = TipoCorreo.CanjeFisicoNormal, EmailCanje = new() { Idusuario = item.IdUsuario, IdPremio = item.IdPremio, FolioRMS = folioRMS, IdRedencion = regRedencion.Id, IdTransaccion = (Guid)resultado.IdTransaccion } });
                            //}
                            //else if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && item.IdPremioNavigation.IdTipoDeTransacionPremio == (int)TipoTransaccionPremio.Envio && item.IdPremioNavigation.IdTipoDeEnvio == (int)TipoEnvio.DigitalCertificados)
                            //{
                            //    EnvioEmail.Add(new() { Tipo = TipoCorreo.CanjeFisicoVirtual, EmailCanje = new() { Idusuario = item.IdUsuario, IdPremio = item.IdPremio, FolioRMS = folioRMS, IdRedencion = regRedencion.Id, IdTransaccion = (Guid)resultado.IdTransaccion } });
                            //}

                        }


                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Envio && item.IdPremioNavigation.Nombre.Contains("2 boletos"))
                        {
                            int n = 0;

                            while (n <= 1)
                            {
                                ProcesaCarritoResultado reg = resultado.Data.Where(x => x.IdCarrito == item.Id).ToList().Skip(n).Take(1).FirstOrDefault();
                                reg.Folio = folioRMS;
                                n++;
                            }
                        }
                        else if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Envio && item.IdPremioNavigation.Nombre.Contains("4 boletos"))
                        {
                            int n = 0;

                            while (n <= 3)
                            {
                                ProcesaCarritoResultado reg = resultado.Data.Where(x => x.IdCarrito == item.Id).ToList().Skip(n).Take(1).FirstOrDefault();
                                reg.Folio = folioRMS;
                                n++;
                            }
                        }
                        else
                        {
                            ProcesaCarritoResultado reg = resultado.Data.Where(x => x.IdCarrito == item.Id).ToList().Skip(ContePremios).Take(1).FirstOrDefault() ?? new();
                            reg.Folio = folioRMS;
                        }

                        //PAra Premios digitales que son recargas telefonicas
                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Recarga)
                        {
                            var regrecargaredimida = usuario.CodigosRedimidos.Where(x => x.IdCarrito == item.Id && x.Folio != null).ToList().Skip(ContePremios).Take(1).FirstOrDefault();
                            regrecargaredimida.IdRedencion = regRedencion.Id;
                            DBContext.SaveChanges();
                        }

                        //Para premios digitales que son certificados(que regresan un codigo/Pin de canje)
                        if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Codigo)
                        {
                            if (item.IdPremioNavigation.Nombre.Contains("2 boletos"))
                            {
                                int n = 0;

                                while (n <= 1)
                                {
                                    CodigosRedimido codigoredimida = usuario.CodigosRedimidos.Where(x => x.IdCarrito == item.Id && x.Folio != null).ToList().OrderBy(x => x.Id).Skip(n).Take(1).FirstOrDefault();
                                    codigoredimida.IdRedencion = regRedencion.Id;
                                    DBContext.SaveChanges();
                                    n++;
                                }
                            }
                            else if (item.IdPremioNavigation.Nombre.Contains("4 boletos"))
                            {
                                int n = 0;

                                while (n <= 3)
                                {
                                    CodigosRedimido codigoredimida = usuario.CodigosRedimidos.Where(x => x.IdCarrito == item.Id && x.Folio != null).ToList().OrderBy(x => x.Id).Skip(n).Take(1).FirstOrDefault();
                                    codigoredimida.IdRedencion = regRedencion.Id;
                                    DBContext.SaveChanges();
                                    n++;
                                }
                            }
                            else
                            {
                                CodigosRedimido codigoredimida = usuario.CodigosRedimidos.Where(x => x.IdCarrito == item.Id && x.Folio != null).ToList().OrderBy(x => x.Id).Skip(ContePremios).Take(1).FirstOrDefault();
                                codigoredimida.IdRedencion = regRedencion.Id;
                                DBContext.SaveChanges();

                            }

                            //Pool Envio de notificacion de canje
                            if (item.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Digital && item.IdPremioNavigation.IdTipoTransaccion == (int)TipoTransaccion.Codigo)
                            {
                                //EnvioEmail.Add(new() { Tipo = TipoCorreo.CanjeDigitalMP, EmailCanje = new() { Idusuario = item.IdUsuario, IdPremio = item.IdPremio, FolioRMS = folioRMS, IdRedencion = regRedencion.Id, IdTransaccion = (Guid)resultado.IdTransaccion } });
                            }

                        }
                        ContePremios++;
                    }

                    if (ActualizaCarrito)
                    {
                        item.IdEstatusCarrito = (int)TipoEstatusCarrito.Procesado;
                        item.FechaRedencion = DateTime.Now;
                        //item.IdTransaccionLog = resultado.IdTransaccion;
                    }

                Siguiente:
                    DBContext.SaveChanges();
                }

                DBContext.Database.CommitTransaction();
            }
            catch (Exception)
            {
                DBContext.Database.RollbackTransaction();

                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
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
                        && x.IdPremioNavigation.IdTipoDePremio == (int)TipoPremio.Fisico)
                    .Any();
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
    }
}
