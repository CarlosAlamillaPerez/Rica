using bepensa_data.models;

namespace bepensa_biz.Interfaces;

public interface IBitacoraDeContrasenas
{
    bool ValidarUltimasContrasenas(List<BitacoraDeContrasena> data, byte[] Password, int limit = 3);

    bool ValidarUltimasContrasenas(List<BitacoraDeContrasena> datos, string Password, int limite = 3);

}
