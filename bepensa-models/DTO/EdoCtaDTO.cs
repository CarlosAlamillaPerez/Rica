namespace bepensa_models.DTO;

public class EdoCtaDTO
{
    public DateTime fecha {get; set;}
    public int puntosObjetivo { get; set; }

    public int puntosEjecucion { get; set; }

    public int puntosPortafolio { get; set; }

    public int puntosFotoExito { get; set; }

    public int puntosComprasApp { get; set; }

    public int puntosPromociones { get; set; }

    public int puntosBienvenida { get; set; }

    public int puntosCumpleanios { get; set; }

    public int puntosNivel { get; set; }

    public int puntosCanje { get; set; }

    public int puntosCancelaCanje { get; set; }

    public int puntosAjustePositivo { get; set; }

    public int puntosAjusteNegativo { get; set; }

    public int puntosTotal { get; set; }

    public int puntosVencido { get; set; }

    public bool esAntesRegistro { get; set; }

    public int puntosComprado { get; set; }
}
