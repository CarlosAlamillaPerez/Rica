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

namespace bepensa_biz.Proxies
{
    public class CarritoProxy : ProxyBase, ICarrito
    {
        private readonly GlobalSettings _ajustes;

        private readonly PremiosSettings _premio;

        public CarritoProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premio)
        {
            DBContext = context;
            _ajustes = ajustes.Value;
            _premio = premio.Value;
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

                        BitacoraDeUsuario bdu = new BitacoraDeUsuario()
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

        public Respuesta<RespuestaCanjeDTO> ProcesarCarrito(ProcesarCarritoRequest pPremio)
        {
            Respuesta<RespuestaCanjeDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid(),
                Data = new()
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

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo && u.Bloqueado == false))
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

                var usuario = DBContext.Usuarios
                    .Include(us => us.Carritos.Where(x => x.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso))
                        .ThenInclude(us => us.IdPremioNavigation)
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
                var saldoActual = DBContext.Movimientos.Where(S => S.IdUsuario == pPremio.IdUsuario)
                                            .OrderByDescending(s => s.Id)
                                            .Select(s => s.Saldo)
                                            .Take(1)
                                            .FirstOrDefault();

                int puntosCarrito = usuario.Carritos.Sum(c => c.Puntos);


                if (!(puntosCarrito <= saldoActual))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.SaldoInsuficiente;
                    resultado.Mensaje = CodigoDeError.SaldoInsuficiente.GetDescription();

                    return resultado;
                }



                resultado.Data.SaldoUtilizado = puntosCarrito;

                var idsCarrito = DBContext.Carritos.Where(c => c.IdEstatusCarrito == (int)TipoEstatusCarrito.EnProceso).Select(c => c.Id);

                BitacoraDeUsuario bdu = new BitacoraDeUsuario()
                {
                    IdUsuario = pPremio.IdUsuario,
                    FechaReg = DateTime.Now,
                    IdTipoDeOperacion = (int)TipoDeOperacion.ProcesarCarrito,
                    Notas = TipoDeOperacion.ProcesarCarrito.GetDescription() + ": " + string.Join(", ", idsCarrito),
                };

                //Iniciamos Transacción
                ////Procesamos los premios que haya en el carrito
                //List<RedencionDePremio> redenciones = AddRedencion(pPremio, idsCarrito.ToList(), idOrigen);

                //if (redenciones == null || redenciones.Count == 0)
                //{
                //    resultado.Exitoso = false;
                //    resultado.Codigo = (int)CodigoDeError.FalloAgregarRedencion;
                //    resultado.Mensaje = CodigoDeError.FalloAgregarRedencion.GetDescription();

                //    return resultado;
                //}

                //usuario.BitacoraDeUsuarios.Add(bdu);


                //foreach (var redencion in redenciones)
                //{
                //    resultado.Data.Codigos.Add(redencion.Folio);
                //    resultado.Data.SaldoUtilizado = redencion.Puntos;
                //    resultado.Data.SaldoActual = redencion.SaldoActual;
                //    try
                //    {
                //        _enviarCorreo.EnviarCorreo(new CorreoDTO
                //        {
                //            TipoDeEnvio = TipoDeEnvio.RealizasteCanje,
                //            IdUsuario = pPremio.IdUsuario,
                //            IdRedencion = redencion.IdRedencion,

                //            Token = Guid.NewGuid()
                //        });
                //    }
                //    catch (Exception)
                //    {
                //        //No se envío correo
                //    }
                //}
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
