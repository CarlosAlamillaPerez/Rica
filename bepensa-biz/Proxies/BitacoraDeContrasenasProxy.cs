using bepensa_biz.Proxies;
using bepensa_data.data;
using bepensa_data.models;
using System.Text;

namespace bepensa_biz.Interfaces;

public class BitacoraDeContrasenasProxy : ProxyBase, IBitacoraDeContrasenas
    {
        public BitacoraDeContrasenasProxy(BepensaContext context)
        {
            DBContext = context;
        }


        public bool ValidarUltimasContrasenas(List<BitacoraDeContrasena> datos, byte[] Password, int limite)
        {
            bool valido = true;
            int intentos = limite;

            foreach (var dato in datos.OrderBy(d => d.Id).TakeLast(intentos))
            {
                if (Encoding.UTF8.GetString(dato.Password) == Encoding.UTF8.GetString(Password))
                {
                    valido = false;
                    break;
                }
            }
            return valido;
        }

        public bool ValidarUltimasContrasenas(List<BitacoraDeContrasena> datos, string Password, int limite)
        {
            bool valido = true;
            int intentos = limite;

            foreach (var dato in datos.OrderBy(d => d.Id).TakeLast(intentos))
            {
                if (Encoding.UTF8.GetString(dato.Password) == Password)
                {
                    valido = false;
                    break;
                }
            }
            return valido;
        }

    }
