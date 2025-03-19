using bepensa_models.General;

namespace bepensa_biz.Interfaces;

public interface IEnviarCorreo
    {
        void EnviarCorreo(CorreoDTO correo);

        void EnviarCorreoCRM(CorreoDTO data);
        void Lectura(Guid? Id);
    }
