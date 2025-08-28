using bepensa_data.modelsRD;

namespace bepensa_models.DTO
{
    public class ConceptosDeAcumulacionRDDTOWa
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdTipoDeMovimiento { get; set; }

        public static implicit operator ConceptosDeAcumulacionRDDTOWa(SubconceptosDeAcumulacion data)
        {
            if (data == null) return new ConceptosDeAcumulacionRDDTOWa();
            return new ConceptosDeAcumulacionRDDTOWa
            {
                Id = data.Id,
                Nombre = data.Nombre
            };
        }
    }
}
