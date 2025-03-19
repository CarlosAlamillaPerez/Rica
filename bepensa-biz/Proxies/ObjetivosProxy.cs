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
    public class ObjetivosProxy : ProxyBase, IObjetivo
    {
        private readonly IMapper mapper;

        public ObjetivosProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<MetaMensualDTO> ConsultarMeteMensual(UsuarioPeriodoRequest pUsuario)
        {
            Respuesta<MetaMensualDTO> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(pUsuario);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                var valiadPeriodo = DBContext.Periodos.Any(x => x.Id == pUsuario.IdPeriodo);

                if (!valiadPeriodo)
                {
                    resultado.Codigo = (int)CodigoDeError.PeriodoInvalido;
                    resultado.Mensaje = CodigoDeError.PeriodoInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                                .Include(x => x.MetasMensuales.Where(x => x.IdPeriodo == pUsuario.IdPeriodo))
                                .FirstOrDefault(x => x.Id == pUsuario.IdUsuario);

                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.MetasMensuales.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<MetaMensualDTO>(usuario.MetasMensuales.First());
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
