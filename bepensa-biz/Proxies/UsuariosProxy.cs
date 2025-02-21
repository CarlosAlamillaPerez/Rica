using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class UsuariosProxy : ProxyBase, IUsuario
    {
        private readonly IMapper mapper;

        public UsuariosProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }


        #region CRM
        public async Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar)
        {
            Respuesta<List<UsuarioDTO>> resultado = new();

            try
            {
                pBuscar.Buscar = pBuscar.Buscar.Trim();

                var valida = Extensiones.ValidateRequest(pBuscar);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }
                List<Usuario>? consulta = null;

                if (long.TryParse(pBuscar.Buscar, out long clvUsuario))
                {
                    consulta = await DBContext.Usuarios.Where(us => us.Id == clvUsuario || us.Cuc.Equals(pBuscar.Buscar)).ToListAsync();
                }
                else
                {
                    consulta = await DBContext.Usuarios
                                    .Where(us =>
                                        us.Id == clvUsuario ||
                                        us.Cuc.Contains(pBuscar.Buscar) ||
                                        (us.Nombre != null && us.Nombre.Contains(pBuscar.Buscar)) ||
                                        (us.ApellidoPaterno != null && us.ApellidoPaterno.Contains(pBuscar.Buscar)) ||
                                        (us.ApellidoMaterno != null && us.ApellidoMaterno.Contains(pBuscar.Buscar)) ||
                                        (us.RazonSocial != null && us.RazonSocial.Contains(pBuscar.Buscar))
                                    ).ToListAsync();
                }

                if (consulta == null || consulta.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<List<UsuarioDTO>>(consulta);

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
        #endregion
    }
}
