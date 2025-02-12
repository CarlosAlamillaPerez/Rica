using bepensa_socio_selecto_models.Enums;
namespace bepensa_socio_selecto_models.General;

public class Respuesta<T>
{
    public T? Data { get; set; }

    public int Codigo { get; set; } = (int)CodigoDeError.OK;

    public string Mensaje { get; set; } = CodigoDeError.OK.GetDescription();

    public bool Exitoso { get; set; } = true;

    public Guid? IdTransaccion { get; set; }
}
