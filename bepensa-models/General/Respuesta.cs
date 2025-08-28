using bepensa_models.Enums;
namespace bepensa_models.General;

public class Respuesta<T, TDetails>
    where TDetails : class, new()
{
    public T? Data { get; set; }

    public TDetails? Details { get; set; }

    public int Codigo { get; set; } = (int)CodigoDeError.OK;

    public string Mensaje { get; set; } = CodigoDeError.OK.GetDescription();

    public bool Exitoso { get; set; } = true;

    public Guid? IdTransaccion { get; set; }
}


public class Respuesta<T> : Respuesta<T, Empty>
{

}