using bepensa_models.ApiWa;
using bepensa_models.DTO;
using bepensa_models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_biz.Interfaces
{
    public interface IConsultasProgramasRDProxy
    {
        Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaMecanicas(RequestCliente data);
        Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaBonos(RequestCliente data);
        Respuesta<List<OrigeneRDDTOWa>> ConsultaCanalesComunicacion(RequestCliente data);
        Respuesta<List<ConceptosDeAcumulacionRDDTOWa>> ConsultaVigencia(RequestCliente data);
    }
}
