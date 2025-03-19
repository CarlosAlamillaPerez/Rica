using bepensa_models.Enums;


namespace bepensa_models.General;

public class CorreoDTO
    {
        public TipoDeEnvio TipoDeEnvio { get; set; }

        public long? IdUsuario { get; set; }

        public long? IdRedencion { get; set; }

        public string? Password { get; set; }

        public long? IdOperador { get; set; }

        public string? EmailUsuario { get; set; }

        public string? EmailContacto { get; set; }

        public string? Mensaje { get; set; }

        public Guid? Token { get; set; }

    }
