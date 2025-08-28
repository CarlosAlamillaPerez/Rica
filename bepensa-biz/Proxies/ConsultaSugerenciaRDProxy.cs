using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.modelsRD;
using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;

namespace bepensa_biz.Proxies
{
    public class ConsultaSugerenciaRDProxy : ProxyBase, IConsultasSugerenciaProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaSugerenciaRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }


        public Respuesta<ResponseSugerencia> RegistraSugerencia(RequestClienteSugerencia data)
        {
            int motivoContacto = Convert.ToInt16(_configuration["SugerenciasSettings:IdMotivoContacto"]);
            string comentarios = "";

            Respuesta<ResponseSugerencia> resultado = new();
            resultado.Data = new();

            try
            {
                var valida = Extensions.Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;
                    goto final;
                }


                if (!DBContextRD.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContextRD.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                comentarios = data.Comentarios.ToString();

                Contactano contactano = new()
                {
                    IdUsuario = usuario.Id,
                    IdMotivoContactanos = motivoContacto,
                    Mensaje = comentarios,
                    FechaReg = DateTime.Now,
                    IdEstatus = 1
                };
                usuario.Contactanos.Add(contactano);
                DBContextRD.SaveChanges();
                resultado.Data.success = 1;
                resultado.Data.mensaje = "Tus comentarios han sido enviados";


            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
                //
            }
        final:
            return resultado;
        }
    }
}