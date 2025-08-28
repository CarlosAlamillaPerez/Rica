using bepensa_data.modelsRD;

namespace bepensa_models.DTO
{
    public class OrigeneRDDTOWa
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public static implicit operator OrigeneRDDTOWa(Origene data)
        {
            if (data == null) return new OrigeneRDDTOWa();
            return new OrigeneRDDTOWa
            {
                Id = data.Id,
                Nombre = data.Nombre
            };
        }
    }
}
