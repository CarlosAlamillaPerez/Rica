using AutoMapper;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_biz.Interfaces;

namespace bepensa_biz.Proxies
{
    public class BitacoraEnvioCorreoProxy : ProxyBase, IBitacoraEnvioCorreo
    {
        private readonly IMapper mapper;

        public BitacoraEnvioCorreoProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<BitacoraEnvioCorreoDTO> ConsultaByToken(Guid? token)
        {
            Respuesta<BitacoraEnvioCorreoDTO> resultado = new();

            if (!DBContext.BitacoraEnvioCorreos.Any(x => x.Token == token))
            {
                resultado.Codigo = (int)CodigoDeError.InvalidToken;
                resultado.Mensaje = CodigoDeError.InvalidToken.GetDescription();
                resultado.Exitoso = false;
                
                return resultado;
            }

            var verificarToken = DBContext.BitacoraEnvioCorreos.First(x => x.Token == token);

            if(!(verificarToken.IdEstatus == (int)TipoDeEstatus.CodigoActivo))
            {
                resultado.Codigo = (int)CodigoDeError.LigaPassUtilizada;
                resultado.Mensaje = CodigoDeError.LigaPassUtilizada.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = mapper.Map<BitacoraEnvioCorreoDTO>(verificarToken);

            return resultado;
        }

        /// <summary>
        /// Desactiva el estado del registro para recuperar la contraseña
        /// </summary>
        /// <param name="idBitacoraEnvioCorreo"></param>
        /// <param name="estatus"></param>
        /// <returns>Registro inactivo de la bitacora de envio de correo</returns>
        public Respuesta<BitacoraEnvioCorreoDTO> ActualizaEstatus(long idBitacoraEnvioCorreo, TipoDeEstatus estatus)
        {
            Respuesta<BitacoraEnvioCorreoDTO> resultado = new();
            try
            {
                if (DBContext.BitacoraEnvioCorreos.Any(x => x.Id == idBitacoraEnvioCorreo))
                {
                    var query = DBContext.BitacoraEnvioCorreos.FirstOrDefault(x => x.Id == idBitacoraEnvioCorreo);
                    query.IdEstatus = (int)estatus;


                    resultado.Data = mapper.Map<BitacoraEnvioCorreoDTO>(Update(query));

                }
                else
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

        private BitacoraEnvioCorreo Update(BitacoraEnvioCorreo data)
        {
            try
            {
                DBContext.Database.BeginTransaction();
                //DBContext.BitacoraEnvioCorreos.Attach(data);
                DBContext.Update(data);
                DBContext.SaveChanges();
                DBContext.Database.CommitTransaction();
                return data;
            }
            catch (Exception)
            {
                DBContext.Database.RollbackTransaction();
                throw;
            }
        }
    }
}
