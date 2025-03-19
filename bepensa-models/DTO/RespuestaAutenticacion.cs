namespace bepensa_models.DTO
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; } = null!;
        public DateTime Expira { get; set; }
    }
}
