namespace bepensa_socio_selecto_models.DTO
{
    public class AccessDTO
    {
        public int Intentos { get; set; } = 0;
        public DateTime FechaAcceso { get; set; } = DateTime.Now;
        public double TiempoDesbloqueo { get; set; } = 0;
        public bool CambiaPassword { get; set; } = false;
        public string? Usuario { get; set; } = string.Empty;
        public bool Bloqueado { get; set; } = false;
    }
}
