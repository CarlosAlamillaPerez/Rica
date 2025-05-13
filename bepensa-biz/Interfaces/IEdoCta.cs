using bepensa_models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.General;

namespace bepensa_biz.Interfaces;

public interface IEdoCta
{
    Task<Respuesta<HeaderEdoCtaDTO>> Header(int pIdUsuario, int pIdPeriodo);

    Task<Respuesta<EdoCtaDTO>> MisPuntos(int pIdUsuario, int pIdPeriodo);

    Respuesta<DetalleCanjeDTO> ConsultarCanje(RequestByIdCanje pUsuario);

    Task<Respuesta<EstadoDeCuentaDTO>> ConsultarEstatdoCuenta(UsuarioPeriodoRequest pUsuario);

    Task<Respuesta<CanjeDTO>> ConsultarCanjes(UsuarioByEmptyPeriodoRequest pUsuario);

    Task<Respuesta<int>> SaldoActual(int pIdUsuario);
}
