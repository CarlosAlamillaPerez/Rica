using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.modelsRD;
using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;

namespace bepensa_biz.Proxies
{
    public class ConsultaClienteRDProxy : ProxyBase, IConsultasClienteProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaClienteRDProxy(BepensaRD_Context Context, IConfiguration Configuracion)
        {
            DBContextRD = Context;
            _configuration = Configuracion;
        }


        public Respuesta<ResponsePuntos> ConsultaPuntos(RequestClientePeriodo data)
        {
            Respuesta<ResponsePuntos> resultado = new();
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
                
                decimal saldo = usuario.Saldos == null ? 0 :usuario.Saldos.Where(x => x.FechaReg.Date.Month == data.Mes && x.FechaReg.Date.Year== data.Anio).OrderByDescending(x => x.Id).Select(x => x.Saldo1).FirstOrDefault();
                decimal ganados= usuario.Saldos == null ? 0 : usuario.Saldos.Where(x => x.FechaReg.Date.Month == data.Mes && x.FechaReg.Date.Year == data.Anio && x.IdTipoDeMovimiento==1).Sum(x=>x.Puntos);
                decimal canjeados = usuario.Saldos == null ? 0 : usuario.Saldos.Where(x => x.FechaReg.Date.Month == data.Mes && x.FechaReg.Date.Year == data.Anio && x.IdTipoDeMovimiento == 2).Sum(x => x.Puntos);
                decimal realizados = usuario.Redenciones != null ? usuario.Redenciones.Where(x => x.FechaReg.Month == data.Mes && x.FechaReg.Year == data.Anio).Count() : 0;

                resultado.Data.puntosDisponibles = saldo;
                resultado.Data.puntosGanados = ganados;
                resultado.Data.puntosCanjeados = canjeados;
                resultado.Data.canjesRealizados = realizados;

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

        public Respuesta<ResponseCliente> ConsultaCliente(RequestCliente data)
        {
            Respuesta<ResponseCliente> resultado = new();
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
                    resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                    resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContextRD.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                                

                resultado.Data.Codigo = usuario.Cuc;
                //resultado.Data.Programa = canal.Nombre;
                resultado.Data.Nivel = usuario.IdProgramaNavigation.Nombre;
                resultado.Data.FechaAlta = usuario.FechaReg;
                resultado.Data.Nombre = usuario.Nombre;
                resultado.Data.Segundonombre = "";
                resultado.Data.ApellidoPaterno = usuario.ApellidoPaterno;
                resultado.Data.ApellidoMaterno = usuario.ApellidoMaterno;
                resultado.Data.Email = usuario.Email;
                //resultado.Data.Telefono = usuario.Telefono;
                resultado.Data.MobilNumber = usuario.Celular;
                //resultado.Data.FechaNacimiento = usuario.FechaNacimiento;
                resultado.Data.EstadoCivilId = 1;
                resultado.Data.genero = usuario.Sexo;
                resultado.Data.nivelId = 1;
                resultado.Data.EstadoCivil = "Sin Asignación";
                resultado.Data.EntregoBoletos = false;
                resultado.Data.MostroIdentificacion = false;
                resultado.Data.boletos = 0;
                resultado.Data.Estatus = usuario.IdEstatusNavigation.Nombre;


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
