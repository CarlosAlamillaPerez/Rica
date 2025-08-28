using bepensa_data.models;
using System.Text.Json.Serialization;

namespace bepensa_models.DTO
{
    public class PortafolioAvanceDTO
    {
        //public string Subconceptodeacumulacion { get; set; } = null!;
        //public string Porcentaje { get; set; }        
        public string subconceptoacumulacion { get; set; }
        public string porcentaje   { get; set; }

        public static implicit operator PortafolioAvanceDTO(PortafolioAvance data)
        {
            if (data == null) return new PortafolioAvanceDTO();
            return new PortafolioAvanceDTO
            {
                //subconceptoacumulacion = string.Concat(data.Subconceptodeacumulacion,": ",data.Porcentaje.ToString(),"%")               
                //subconceptoacumulacion= string.Concat("\"",data.Subconceptodeacumulacion, "\"",":","\"",data.Porcentaje.ToString(),"%")
                subconceptoacumulacion=data.Subconceptodeacumulacion,
                porcentaje=data.Porcentaje.ToString()
            };
        }
    }
}
