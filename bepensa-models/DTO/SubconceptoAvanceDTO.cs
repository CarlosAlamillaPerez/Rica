using bepensa_data.models;

namespace bepensa_models.DTO
{
    public class SubconceptoAvanceDTO
    {
        public string Nombre {  get; set; }

        public static implicit operator SubconceptoAvanceDTO(SubconceptosDeAcumulacion data)
        {
            if (data == null) return new SubconceptoAvanceDTO();
            return new SubconceptoAvanceDTO
            {                
                Nombre = data.Nombre
            };
        }
    }
}
