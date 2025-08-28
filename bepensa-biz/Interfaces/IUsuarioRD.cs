using bepensa_models.DataModels;
using bepensa_models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_biz.Interfaces
{
    public interface IUsuarioRD
    {
        Task<Respuesta<Empty>> RecuperarContrasenia(RestablecerPassRequest info);
    }
}
