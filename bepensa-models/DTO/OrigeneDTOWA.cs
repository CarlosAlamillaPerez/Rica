using bepensa_data.models;

namespace bepensa_models.DTO
{
    public class OrigeneDTOWA
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public static implicit operator OrigeneDTOWA(Origene data)
        {
            if (data == null) return new OrigeneDTOWA();
            return new OrigeneDTOWA
            {
                Id = data.Id,
                Nombre = data.Nombre
            };
        }
    }
}
