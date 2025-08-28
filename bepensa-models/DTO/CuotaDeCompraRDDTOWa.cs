using bepensa_data.modelsRD;

namespace bepensa_models.DTO
{
    public class CuotaDeCompraRDDTOWa
    {
        public long IdNegocio {  get; set; }
        public int IdPeriodo { get; set; }
        public string Nombre { get; set; }
        public decimal Cuota { get; set; }
        public decimal? CompraTotal {get; set; }
        public int? avance { get; set; }

        public static implicit operator CuotaDeCompraRDDTOWa(DetalleDeMetaDeCompra data)
        {            
            if (data == null) return new CuotaDeCompraRDDTOWa();
            return new CuotaDeCompraRDDTOWa
            {
                IdNegocio = data.IdNegocio,
                IdPeriodo = data.IdPeriodo,
                Nombre = data.Nombre,
                Cuota = data.Cuota,
                CompraTotal=data.Compra,
                avance=data.Avance
            };
        }
    }
}
