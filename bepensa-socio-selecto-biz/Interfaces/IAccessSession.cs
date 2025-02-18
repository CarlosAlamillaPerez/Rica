using bepensa_socio_selecto_models.DataModels;

namespace bepensa_socio_selecto_biz.Interfaces
{
    public interface IAccessSession
    {
        LoginInscripcionRequest CredencialesInscripcion { get; set; }

        void Logout();
    }
}
