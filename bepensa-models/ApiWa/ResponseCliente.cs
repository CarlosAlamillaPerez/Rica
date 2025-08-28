namespace bepensa_models.ApiWa
{
    public class ResponseCliente
    {
        public string Codigo { get; set; }
        public string Programa { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Nombre { get; set; }
        public string Segundonombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Email { get; set; }        
        public string Telefono { get; set; }
        public string MobilNumber { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int EstadoCivilId { get; set; }
        public string genero { get; set; }
        public int nivelId { get; set; }
        public string EstadoCivil { get; set; }
        public bool EntregoBoletos { get; set; }
        public bool MostroIdentificacion { get; set; }
        public int boletos { get; set; }
        public string Estatus { get; set; }
        public string Saldo { get; set; }
        public bool redime { get; set; }
    }
}
