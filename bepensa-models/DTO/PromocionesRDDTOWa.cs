using bepensa_data.modelsRD;


namespace bepensa_models.DTO
{
    public class PromocionesRDDTOWa
    {
        public int Id { get; set; }
        
        public int IdPeriodo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public int IdEstatus { get; set; }

        public DateTime FechaReg { get; set; }


        public static implicit operator PromocionesRDDTOWa(ImagenesPromocione data)
        {
            if (data == null) return new PromocionesRDDTOWa();
            return new PromocionesRDDTOWa
            {
                Id = data.Id,                
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