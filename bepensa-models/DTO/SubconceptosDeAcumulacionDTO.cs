using bepensa_data.models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DTO
{
    public class SubConceptosDeAcumulacionDTO
    {
        public int Id { get; set; }
        public string codigo { get; set; }
        public string Nombre { get; set; } = null!;

        public static implicit operator SubConceptosDeAcumulacionDTO(SubconceptosDeAcumulacion data)
        {
            if (data == null) return new SubConceptosDeAcumulacionDTO();
            return new SubConceptosDeAcumulacionDTO
            {
                Id = data.Id,
                codigo = data.IdConceptoDeAcumulacionNavigation.Codigo,
                Nombre = data.Nombre
            };
        }
    }
}
