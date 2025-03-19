namespace bepensa_models.DTO
{
    public class BitacoraEnvioCorreoDTO
    {
        public long Id { get; set; }

        public long? IdUsuario { get; set; }

        public string Email { get; set; } = null!;

        public DateTime FechaEnvio { get; set; }

        public string Codigo { get; set; } = null!;

        public int? MailItemId { get; set; }

        public Guid Token { get; set; }

        public int IdEstatus { get; set; }

        public int? Lecturas { get; set; }

        public DateTime? FechaLectura { get; set; }

        public int? IdOperador { get; set; }

        public virtual EstatusDTO IdEstatusNavigation { get; set; } = null!;

        public virtual UsuarioDTO IdUsuarioNavigation { get; set; } = null!;
    }
}
