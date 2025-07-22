using bepensa_models.General;

namespace bepensa_biz.Interfaces
{
    public interface IOperacion
    {
        Task<Respuesta<Empty>> ActualizarEstatusRedenciones();
    }
}
