using System.Net.NetworkInformation;

namespace bepensa_models.DTO
{
    public class FuerzaDeVentaDTO
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string? Email { get; set; }

        //public byte[]? Password { get; set; }

        public string Sesion { get; set; } = null!;

        public bool Bloqueado { get; set; }

        public int IdEstatus { get; set; }

        public DateTime FechaReg { get; set; }

        public int IdOperadorReg { get; set; }

        public DateTime? FechaMod { get; set; }

        public int? IdOperadorMod { get; set; }

        //public virtual ICollection<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; } = new List<FuerzasDeVentaNegocio>();

        //public virtual EstatusDTO IdEstatusNavigation { get; set; } = null!;

        //public virtual OperadorDTO IdOperadorRegNavigation { get; set; } = null!;

        //public virtual OperadorDTO IdOperadorModNavigation { get; set; }

    }
}
