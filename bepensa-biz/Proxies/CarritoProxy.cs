using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_biz.Extensions;
using Microsoft.EntityFrameworkCore;
using bepensa_models.DTO;

namespace bepensa_biz.Proxies
{
    public class CarritoProxy : ProxyBase, ICarrito
    {
        public CarritoProxy(BepensaContext context)
        {
            DBContext = context;
        }

        public Respuesta<Empty> AgregarPremio(AgregarPremioRequest pPremio, int idOrigen)
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

                if (!DBContext.Usuarios.Any(u => u.Id == pPremio.IdUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo && u.Bloqueado == false))
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

                using var transaction = DBContext.Database.BeginTransaction();

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
                            Puntos = premio.Puntos,
                            IdOrigen = idOrigen
                        };

                        usuario.Carritos.Add(carrito);
                    }

                    DBContext.SaveChanges();
                    transaction.Commit();

                    resultado.Mensaje = MensajeApp.PremioAgregado.GetDescription();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    resultado.Codigo = (int)CodigoDeError.FalloAgregarPremio;
                    resultado.Mensaje = CodigoDeError.FalloAgregarPremio.GetDescription();
                    resultado.Exitoso = false;
                }

            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            }

            return resultado;
        }

        public Respuesta<CarritoDTO> ConsultarCarrito(RequestByIdUsuario pPremio)
        {
            Respuesta<CarritoDTO> resultado = new();

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
    }
}
