using bepensa_data.models;
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
    public interface IConsultasProgramasProxy
    {
        Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaMecanicas(RequestCliente data);
        Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaBonos(RequestCliente data);
        Respuesta<List<OrigeneDTOWA>> ConsultaCanalesComunicacion(RequestCliente data);
        Respuesta<List<SubConceptosDeAcumulacionDTO>> ConsultaVigencia(RequestCliente data);
    }
}
