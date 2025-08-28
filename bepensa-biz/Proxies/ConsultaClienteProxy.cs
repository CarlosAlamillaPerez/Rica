using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Extensions.Configuration;

namespace bepensa_biz.Proxies
{
    public class ConsultaClienteProxy : ProxyBase, IConsultasClienteProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaClienteProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
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


                if (!DBContext.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario

                List<int> positivos = DBContext.ConceptosDeAcumulacions.Where(x => x.IdTipoDeMovimiento == 1).Select(y => y.Id).ToList();
                List<int> negativos = DBContext.ConceptosDeAcumulacions.Where(x => x.IdTipoDeMovimiento == 2).Select(y => y.Id).ToList();
                List<int> conceptospositivos = DBContext.SubconceptosDeAcumulacions.Where(x => positivos.Contains(x.IdConceptoDeAcumulacion)).Select(y =>y.Id).ToList();
                List<int> conceptosnegativos = DBContext.SubconceptosDeAcumulacions.Where(x => negativos.Contains(x.IdConceptoDeAcumulacion)).Select(y =>y.Id).ToList();
                

                decimal saldo = usuario.Movimientos != null ? usuario.Movimientos.Where(x => x.FechaReg.Month == data.Mes && x.FechaReg.Year == data.Anio).OrderByDescending(x => x.Id).Select(x =>x.Saldo).FirstOrDefault():0;
                decimal ganados = usuario.Movimientos != null ? usuario.Movimientos.Where(x => conceptospositivos.Contains(x.IdSda) && x.FechaReg.Month == data.Mes && x.FechaReg.Year == data.Anio && x.IdUsuario==usuario.Id).Sum(x => x.Puntos):0;
                decimal canjeados = usuario.Movimientos != null ? usuario.Movimientos.Where(x => conceptosnegativos.Contains(x.IdSda) && x.FechaReg.Month == data.Mes && x.FechaReg.Year == data.Anio && x.IdUsuario == usuario.Id).Sum(x => x.Puntos):0;
                decimal realizados = usuario.Redenciones != null ? usuario.Redenciones.Where(x=>x.FechaReg.Month==data.Mes && x.FechaReg.Year == data.Anio).Count():0;


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


                if (!DBContext.Usuarios.Any(x => x.Cuc == data.Cliente))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                Usuario usuario = DBContext.Usuarios.FirstOrDefault(x => x.Cuc == data.Cliente);//variable se guarda lo de usuario                
                int idcanal = DBContext.Canales.Where(x => x.Id == usuario.IdProgramaNavigation.IdCanal).Select(x => x.Id).First();
                Canale canal = DBContext.Canales.FirstOrDefault(x => x.Id == idcanal);//variable se guarda lo de usuario                


                decimal saldo = usuario.Movimientos != null ? usuario.Movimientos.OrderByDescending(x => x.Id).Select(x => x.Saldo).FirstOrDefault(): 0;
                var ptsPremio = DBContext.Premios.Where(x => x.IdEstatus == (int)TipoDeEstatus.Activo).Select(x => x.Puntos).Min();


                bool valor;
                if (saldo == 0 | saldo < ptsPremio)
                {
                    valor = false;
                }
                else
                {
                    valor = true;
                }

                resultado.Data.Codigo = usuario.Cuc;                
                resultado.Data.Programa =canal.Nombre;
                resultado.Data.Saldo = saldo.ToString("N");
                resultado.Data.redime =valor;
                resultado.Data.Nivel = usuario.IdProgramaNavigation.Nombre;
                resultado.Data.FechaAlta = usuario.FechaReg;
                resultado.Data.Nombre = usuario.Nombre;
                resultado.Data.Segundonombre = "";
                resultado.Data.ApellidoPaterno = usuario.ApellidoPaterno;
                resultado.Data.ApellidoMaterno = usuario.ApellidoMaterno;
                resultado.Data.Email = usuario.Email;
                resultado.Data.Telefono = usuario.Telefono;
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
