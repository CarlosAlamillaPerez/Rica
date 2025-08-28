using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;

namespace bepensa_biz.Proxies
{
    public class ConsultaAvanceProxy : ProxyBase, IConsultasAvanceProxy
    {
        private readonly IConfiguration _configuration;
        public ConsultaAvanceProxy(BepensaContext Context, IConfiguration Configuracion)
        {
            DBContext = Context;
            _configuration = Configuracion;
        }


        public Respuesta<AvanceWa> ConsultaAvance(RequestCliente data)
        {
            Respuesta<AvanceWa> resultado = new();
            resultado.Data = new();
            resultado.Data.meta = new();            
            resultado.Data.fotoexito = new();
            resultado.Data.compraDigital = new();
            //resultado.Data.porta = new();

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
                int idperiodo = DBContext.Periodos.Where(x => x.Fecha.Month == DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year).Select(x => x.Id).First();
                if (usuario.MetasMensuales.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                if (usuario.CumplimientosPortafolios.Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }

                if (DBContext.PortafolioAvances.Where(x => x.IdUsuario == usuario.Id && x.IdPeriodo == idperiodo).Count() == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinResultados;
                    resultado.Mensaje = CodigoDeError.SinResultados.GetDescription();
                    resultado.Exitoso = false;
                    goto final;
                }
                Periodo periodo = DBContext.Periodos.FirstOrDefault(x => x.Id == idperiodo);

                //int idperiodo = 28;
                decimal metames = usuario.MetasMensuales != null ? usuario.MetasMensuales.Where(x => x.IdPeriodo == idperiodo).OrderByDescending(x => x.IdPeriodo).Select(x => x.Meta).FirstOrDefault() : 0;
                decimal metaobjetivo = usuario.MetasMensuales != null ? usuario.MetasMensuales.Where(x => x.IdPeriodo == idperiodo).Select(x => x.ImporteComprado).FirstOrDefault() : 0;
                decimal metaavance = usuario.MetasMensuales != null ? (metaobjetivo * 100) / metames : 0;
                decimal metarestante = usuario.MetasMensuales != null ? metames - metaobjetivo : 0;


                resultado.Data.meta.mes = periodo.Fecha.ToString("MMMM", new CultureInfo("es-ES"));
                //resultado.Data.meta.objetivo= "$ " +Math.Round(metames,2).ToString("N");
                //resultado.Data.meta.objetivoavance =  "$ " + Math.Round(metaobjetivo,2).ToString("N");
                //resultado.Data.meta.avance = Math.Round(metaavance,2).ToString("N")+"%";
                //resultado.Data.meta.tefalta = "$ " + Math.Round(metarestante,2).ToString("N");

                resultado.Data.meta.objetivo = Math.Round(metames, 2);
                resultado.Data.meta.objetivoavance = Math.Round(metaobjetivo, 2);
                resultado.Data.meta.avance = Math.Round(metaavance, 2).ToString("N") + "%";
                resultado.Data.meta.tefalta = metaobjetivo > metames ? 0 : Math.Round(metarestante, 2);




                //portafolio
                int idcanal = DBContext.Canales.Where(x => x.Id == usuario.IdProgramaNavigation.IdCanal).Select(x => x.Id).FirstOrDefault();
                int idcda = DBContext.ConceptosDeAcumulacions.Where(x => x.IdCanal == idcanal && x.Nombre == "Portafolio Prioritario").Select(x => x.Id).FirstOrDefault();

                List<PortafolioAvanceDTO> avance = DBContext.PortafolioAvances.Where(x => x.IdPeriodo == idperiodo && x.IdUsuario == usuario.Id).Select(x => (PortafolioAvanceDTO)x).ToList();

                if (idcanal == 1)
                {
                    resultado.Data.portafolio = new();

                    resultado.Data.portafolio.Colas = avance.Where(x => x.subconceptoacumulacion == "Colas").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafolio.ColasSinAzucar = avance.Where(x => x.subconceptoacumulacion == "Colas sin azúcar").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafolio.Frutales = avance.Where(x => x.subconceptoacumulacion == "Frutales").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafolio.FrutalesRetornables = avance.Where(x => x.subconceptoacumulacion == "Frutales Retornables").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafolio.Hidratación = avance.Where(x => x.subconceptoacumulacion == "Hidratación").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafolio.Nutrición = avance.Where(x => x.subconceptoacumulacion == "Nutrición").Select(x => x.porcentaje + "%").FirstOrDefault();
                }
                else if(idcanal==2)
                {
                    resultado.Data.portafoliocomidas = new();
                    resultado.Data.portafoliocomidas.ColasNoRetornables = avance.Where(x => x.subconceptoacumulacion == "Colas no Retornables").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.Colasretornables = avance.Where(x => x.subconceptoacumulacion == "Colas Retornables").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.ColasSinAzucar = avance.Where(x => x.subconceptoacumulacion == "Colas sin azúcar").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.FrutalesNoretornables = avance.Where(x => x.subconceptoacumulacion == "Frutales no Retornables").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.FrutalesRetornables = avance.Where(x => x.subconceptoacumulacion == "Frutales Retornables").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.Hidratación = avance.Where(x => x.subconceptoacumulacion == "Hidratación").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.Innovación = avance.Where(x => x.subconceptoacumulacion == "Innovación").Select(x => x.porcentaje + "%").FirstOrDefault();
                    resultado.Data.portafoliocomidas.StillSS = avance.Where(x => x.subconceptoacumulacion == "Stills SS").Select(x => x.porcentaje + "%").FirstOrDefault();
                }

                var portaf = avance.Select(x => x.subconceptoacumulacion +":" + x.porcentaje + "%").ToList();
                                
                //resultado.Data.porta = portaf;

                var array = new[] { "FOTO DE ÉXITO" };
                int idseg = DBContext.SegmentosAcumulacions.Where(x => array.Any(y => x.Nombre == y)).Select(x => x.Id).FirstOrDefault();


                decimal objetivo = DBContext.Empaques.Where(x => x.IdSegAcumulacion == idseg && x.IdPeriodo == idperiodo).Count();
                decimal objetivofinal;
                //decimal objetivoavance= (decimal)(usuario.Movimientos != null ? usuario.Movimientos.Where(x => x.IdUsuario == usuario.Id && x.IdPeriodo==idperiodo && x.IdSda==14).Select(x=>x.Cantidad).FirstOrDefault() : 0);
                decimal objetivoavance = usuario.Movimientos.Where(x => x.IdPeriodo == idperiodo && x.IdSda == 14).Count() > 0 ? (decimal)usuario.Movimientos.Where(x =>x.IdPeriodo == idperiodo && x.IdSda == 14).Select(x => x.Cantidad).FirstOrDefault() : (decimal)0;
                decimal objetivoav;
                decimal porcavance;
                if (idcanal == 1)
                {
                    objetivofinal = objetivo;
                    objetivoav = objetivoavance;
                    porcavance = objetivofinal == 0 && objetivoav == 0 ? 0 : (objetivoav * 100) / objetivofinal;
                }
                else
                {
                    objetivofinal = 0;
                    objetivoav = 0;
                    porcavance = 0;
                }

                //FotoExito
                resultado.Data.fotoexito.Objetivo = objetivofinal;
                resultado.Data.fotoexito.objetivoAvance = objetivoav;
                resultado.Data.fotoexito.avance = Math.Round(porcavance, 2).ToString() + "%";

                int idsda;
                if (idcanal == 1)
                {
                    idsda = DBContext.SubconceptosDeAcumulacions.Where(x => x.Nombre == "Compra por la app").Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    idsda = DBContext.SubconceptosDeAcumulacions.Where(x => x.Nombre == "Compra digital").Select(x => x.Id).FirstOrDefault();
                }

                decimal compradigital = usuario.MetasMensuales != null ? usuario.MetasMensuales.Where(x => x.IdPeriodo == idperiodo).Select(x => x.CompraDigital).FirstOrDefault() : 0;
                decimal totalavance = usuario.Movimientos.Where(x => x.IdPeriodo == idperiodo && x.IdSda == idsda).Count()>0 ? (decimal)usuario.Movimientos.Where(x =>x.IdPeriodo == idperiodo && x.IdSda == idsda).Select(x => x.Cantidad).FirstOrDefault() : 0;
                //List<int> idComprasDigitales = DBContext.SubconceptosDeAcumulacions.Where(x => x.Nombre == "Compra digital").Select(y => y.Id).ToList();
                resultado.Data.compraDigital.objetivoCompra = Math.Round(metames, 2);
                resultado.Data.compraDigital.objetivocompraAvance = usuario.MetasMensuales != null ? Math.Round((compradigital * 100) / metames,2).ToString() + "%" : "0%";
                resultado.Data.compraDigital.objetivoImporte = compradigital;
                resultado.Data.compraDigital.objetivoImporteAvance = totalavance;
                resultado.Data.compraDigital.objetivoImporteFalta = (compradigital-totalavance);

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
