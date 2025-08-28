using bepensa_data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DTO
{
    public class ConceptosDeAcumulacionDTOWa
    {
        public int Id { get; set; }

        public int IdCanal { get; set; }

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public int IdTipoDeMovimiento { get; set; }

        public static implicit operator ConceptosDeAcumulacionDTOWa(ConceptosDeAcumulacion data)
        {
            if (data == null) return new ConceptosDeAcumulacionDTOWa();
            return new ConceptosDeAcumulacionDTOWa
            {
                Id = data.Id,
                Nombre = data.Nombre
            };
        }
    }
}
