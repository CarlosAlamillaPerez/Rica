namespace bepensa_models.ApiResponse;

public class ResponseRastreoGuia
{
    public int? Success { get; set; }

    public string? Mensaje { get; set; }

    public string? Numero_guia { get; set; }

    public string? Folio_estatus_codigo { get; set; }

    public string? Folio_estatus_descripcion { get; set; }

    public string? Tipo_envio { get; set; }

    public string? Paqueteria { get; set; }

    public string? Paqueteria_url { get; set; }

    public List<object>? Rastreo { get; set; }

    public DocumentoPruebaEntrega? Documento_prueba_entrega { get; set; }
}
