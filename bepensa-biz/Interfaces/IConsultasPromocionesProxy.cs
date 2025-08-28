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
    public interface IConsultasPromocionesProxy
    {
        Respuesta<List<PromocionesDTOWa>> ConsultaPromociones(RequestCliente data);
    }
}
