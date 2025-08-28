using bepensa_data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.DTO
{
    public class PromocionesDTOWa
    {
        public int Id { get; set; }

        public int IdCanal { get; set; }

        public int IdPeriodo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public int IdEstatus { get; set; }

        public DateTime FechaReg { get; set; }


        public static implicit operator PromocionesDTOWa(ImagenesPromocione data)
        {
            if (data == null) return new PromocionesDTOWa();
            return new PromocionesDTOWa
            {
                Id = data.Id,
                IdCanal = data.IdCanal,
                IdPeriodo = data.IdPeriodo, 
                Nombre = data.Nombre,
                Url = data.Url,
                Tipo = data.Tipo,
                IdEstatus = data.IdEstatus,
                FechaReg = data.FechaReg                
            };
        }
    }
}
