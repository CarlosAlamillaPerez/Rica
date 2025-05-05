using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Security;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace bepensa_biz.Proxies
{
    public class FuerzaVentaProxy : ProxyBase, IFuerzaVenta
    {
        private readonly IMapper mapper;
        public FuerzaVentaProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public async Task<Respuesta<FuerzaVentaDTO>> ValidaAcceso(LoginApp credenciales, int idCanal)
        {
            Respuesta<FuerzaVentaDTO> resultado = new();

            BitacoraFuerzaVentum bdu = new() { FechaReg = DateTime.Now };

            byte[] password;

            try
            {
                if (credenciales.Sesion != null)
                {
                    goto reconexion;
                }

                var valida = Extensiones.ValidateRequest(credenciales);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

            reconexion:

                var fdv = credenciales.Sesion != null ?
                    await DBContext.FuerzaVenta.Include(x => x.IdCanalNavigation).FirstOrDefaultAsync(u => u.SesionId == credenciales.Sesion.ToString()) :
                    await DBContext.FuerzaVenta.Include(x => x.IdCanalNavigation).FirstOrDefaultAsync(u => u.Usuario == credenciales.Usuario && u.IdCanal == idCanal);

                if (fdv == null || fdv.Password == null)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();

                    return resultado;
                }

                if (credenciales.Sesion != null)
                {
                    password = fdv.Password;
                }
                else
                {
                    var hash = new Hash(credenciales.Password);
                    password = hash.Sha512();
                }

                if (!(Encoding.UTF8.GetString(fdv.Password) == Encoding.UTF8.GetString(password)))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();

                    return resultado;
                }

                //if (fdv.Bloqueado)
                //{
                //    resultado.Exitoso = false;
                //    resultado.Codigo = (int)CodigoDeError.UsuarioBloqueado;
                //    resultado.Mensaje = CodigoDeError.UsuarioBloqueado.GetDescription();

                //    return resultado;
                //}

                if (!(fdv.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                //if (!string.IsNullOrEmpty(credenciales.TokenDispositivo))
                //{
                //    fdv.TokenMovil = credenciales.TokenDispositivo;
                //}

                bdu.IdFdv = fdv.Id;
                bdu.IdTipoDeOperacion = (int)TipoDeOperacion.InicioSesion;
                bdu.Notas = TipoDeOperacion.InicioSesion.GetDescription();

                fdv.SesionId = Guid.NewGuid().ToString();

                fdv.BitacoraFuerzaVenta.Add(bdu);

                fdv = Update(fdv);

                resultado.Data = mapper.Map<FuerzaVentaDTO>(fdv);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<List<UsuarioDTO>>> ConsultarUsuarios(BuscarFDVRequest pBuscar)
        {
            Respuesta<List<UsuarioDTO>> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pBuscar);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var fdv = await DBContext.FuerzaVenta
                    .Include(x => x.IdRolFdvNavigation)
                    .FirstOrDefaultAsync(x => x.Id == pBuscar.IdFDV);

                if (fdv == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (fdv.IdRolFdvNavigation.Id == (int)TipoRolFDV.Corporativo)
                {
                    resultado.Data = await BuscarUsuarioPorZona(fdv.IdCanal, fdv.IdBusqueda ?? 0, pBuscar.Buscar);
                }
                else if (fdv.IdRolFdvNavigation.Id == (int)TipoRolFDV.Jefe_Venta)
                {
                    resultado.Data = await BuscarUsuarioPorCedi(fdv.IdCanal, fdv.IdBusqueda ?? 0, pBuscar.Buscar);
                }
                else if (fdv.IdRolFdvNavigation.Id == (int)TipoRolFDV.Jefe_Venta)
                {
                    resultado.Data = await BuscarUsuarioPorSup(fdv.IdCanal, fdv.IdBusqueda ?? 0, pBuscar.Buscar);
                }
                else
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                }

                if (resultado.Data == null || resultado.Data.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;
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

        #region Métodos Privados
        public async Task<Respuesta<UsuarioDTO>> ConsultarUsuario(int idUsuario, int idCanal)
        {
            Respuesta<UsuarioDTO> resultado = new();

            try
            {
                var consultar = await DBContext.Usuarios
                    .Include(us => us.IdEstatusNavigation)
                    .Include(us => us.IdProgramaNavigation)
                        .ThenInclude(us => us.IdCanalNavigation)
                    .Include(us => us.IdCediNavigation)
                        .ThenInclude(ced => ced.IdZonaNavigation)
                            .ThenInclude(ced => ced.IdEmbotelladoraNavigation)
                    .Include(us => us.IdSupervisorNavigation)
                    .Include(us => us.IdRutaNavigation)
                    .FirstOrDefaultAsync(us => us.IdEstatus == (int)TipoEstatus.Activo && us.Id == idUsuario && us.IdProgramaNavigation.IdCanal == idCanal);

                if (consultar == null)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<UsuarioDTO>(consultar);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        private async Task<List<UsuarioDTO>> BuscarUsuarioPorZona(int idCanal, int idBusqueda, string buscar)
        {
            var consultar = await DBContext.Usuarios
                .Include(us => us.IdEstatusNavigation)
                .Include(us => us.IdProgramaNavigation)
                    .ThenInclude(us => us.IdCanalNavigation)
                .Include(us => us.IdCediNavigation)
                    .ThenInclude(ced => ced.IdZonaNavigation)
                        .ThenInclude(ced => ced.IdEmbotelladoraNavigation)
                .Include(us => us.IdSupervisorNavigation)
                .Include(us => us.IdRutaNavigation)
                .Where(us =>
                    us.IdProgramaNavigation.IdCanal == idCanal &&
                    us.IdCediNavigation.IdZonaNavigation.BitValue != null &&
                    us.IdEstatus == (int)TipoEstatus.Activo &&
                    (idBusqueda & us.IdCediNavigation.IdZonaNavigation.BitValue) == us.IdCediNavigation.IdZonaNavigation.BitValue &&
                    (
                        us.Cuc.Contains(buscar) ||
                        (us.Nombre != null && us.Nombre.Contains(buscar)) ||
                        (us.ApellidoPaterno != null && us.ApellidoPaterno.Contains(buscar)) ||
                        (us.ApellidoMaterno != null && us.ApellidoMaterno.Contains(buscar)) ||
                        us.RazonSocial.Contains(buscar)
                    )
                )
                .ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(consultar);
        }

        private async Task<List<UsuarioDTO>> BuscarUsuarioPorCedi(int idCanal, int idBusqueda, string buscar)
        {
            var consultar = await DBContext.Usuarios
                .Include(us => us.IdEstatusNavigation)
                .Include(us => us.IdProgramaNavigation)
                    .ThenInclude(us => us.IdCanalNavigation)
                .Include(us => us.IdCediNavigation)
                    .ThenInclude(ced => ced.IdZonaNavigation)
                        .ThenInclude(ced => ced.IdEmbotelladoraNavigation)
                .Include(us => us.IdSupervisorNavigation)
                .Include(us => us.IdRutaNavigation)
                .Where(us =>
                    us.IdProgramaNavigation.IdCanal == idCanal &&
                    us.IdEstatus == (int)TipoEstatus.Activo &&
                    us.IdCediNavigation.BitValue != null &&
                    (idBusqueda & us.IdCediNavigation.BitValue) == us.IdCediNavigation.BitValue &&
                    (
                        us.Cuc.Contains(buscar) ||
                        (us.Nombre != null && us.Nombre.Contains(buscar)) ||
                        (us.ApellidoPaterno != null && us.ApellidoPaterno.Contains(buscar)) ||
                        (us.ApellidoMaterno != null && us.ApellidoMaterno.Contains(buscar)) ||
                        us.RazonSocial.Contains(buscar)
                    )
                )
                .ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(consultar);
        }

        private async Task<List<UsuarioDTO>> BuscarUsuarioPorSup(int idCanal, int idBusqueda, string buscar)
        {
            var consultar = await DBContext.Usuarios
                .Include(us => us.IdEstatusNavigation)
                .Include(us => us.IdProgramaNavigation)
                    .ThenInclude(us => us.IdCanalNavigation)
                .Include(us => us.IdCediNavigation)
                    .ThenInclude(ced => ced.IdZonaNavigation)
                        .ThenInclude(ced => ced.IdEmbotelladoraNavigation)
                .Include(us => us.IdSupervisorNavigation)
                .Include(us => us.IdRutaNavigation)
                .Where(us =>
                    us.IdProgramaNavigation.IdCanal == idCanal &&
                    us.IdEstatus == (int)TipoEstatus.Activo &&
                    us.IdSupervisor == idBusqueda &&
                    (
                        us.Cuc.Contains(buscar) ||
                        (us.Nombre != null && us.Nombre.Contains(buscar)) ||
                        (us.ApellidoPaterno != null && us.ApellidoPaterno.Contains(buscar)) ||
                        (us.ApellidoMaterno != null && us.ApellidoMaterno.Contains(buscar)) ||
                        us.RazonSocial.Contains(buscar)
                    )
                )
                .ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(consultar);
        }

        private FuerzaVentum Update(FuerzaVentum fdv)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var transaction = DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        DBContext.Update(fdv);
                        DBContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return fdv;
        }
        #endregion
    }
}
