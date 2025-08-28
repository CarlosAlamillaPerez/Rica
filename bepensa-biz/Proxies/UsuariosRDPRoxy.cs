using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_models.DataModels;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_models.DataModels;
using AutoMapper;
using bepensa_data.data;
using bepensa_data.modelsRD;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class UsuariosRDPRoxy : ProxyBase, IUsuarioRD
    {
        private readonly IAppEmailRD appEmail;
        public UsuariosRDPRoxy(BepensaRD_Context context, IAppEmailRD appEmail)
        {
            DBContextRD = context;
            this.appEmail = appEmail;
        }
        public async Task<Respuesta<Empty>> RecuperarContrasenia(RestablecerPassRequest datos)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(datos);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdTdo = (int)TipoOperacion.RecuperarPassword,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.RecuperarPassword.GetDescription()
                };

                if (!DBContext.Usuarios.Any(u => u.Cuc == datos.Cuc && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                Usuario usuario = DBContextRD.Usuarios.First(u => u.Cuc == datos.Cuc);

                usuario.BitacoraDeUsuarios.Add(bdu);

                resultado.Mensaje = datos.TipoMensajeria switch
                {
                    //TipoMensajeria.Email => MensajeApp.EnlaceCorreoEnviado.GetDescription(),
                    TipoMensajeria.Sms => MensajeApp.EnlaceSMSenviado.GetDescription(),
                    _ => throw new Exception("Mensajería desconocida"),
                };

                Update(usuario);

                await appEmail.RecuperarPassword(datos.TipoMensajeria, TipoUsuario.Usuario, (int)usuario.Id);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        //Actualización de usuario
        private Usuario Update(Usuario usuario)
        {
            var strategy = DBContextRD.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var transaction = DBContextRD.Database.BeginTransaction())
                {
                    try
                    {
                        DBContextRD.Update(usuario);
                        DBContextRD.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return usuario;
        }
    }
}
