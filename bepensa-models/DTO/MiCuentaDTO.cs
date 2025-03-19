namespace bepensa_models.DTO
{
    public class MiCuentaDTO
    {
        public UsuarioDTO Propietario { get; set; } = new UsuarioDTO();
        public NegocioDTO Negocio { get; set; } = new NegocioDTO();
    }
}