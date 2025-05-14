using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.Enums;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class BitacoraProxy : ProxyBase, IBitacora
    {
        public BitacoraProxy(BepensaContext context)
        {
            DBContext = context;
        }

        #region Operadores
        public void BitacoraDeOperadores(int pIdOperador, int pIdTipoDeOperacion, int? pIdUsuarioAftd, int? pIdOperadorAftd)
        {
            try
            {
                var operador = DBContext.Operadores.First(x => x.Id == pIdOperador);

                operador.BitacoraDeOperadoreIdOperadorNavigations.Add(new BitacoraDeOperadore
                {
                    IdTipoDeOperacion = pIdTipoDeOperacion,
                    IdOperadorAftd = pIdOperadorAftd,
                    IdUsuarioAftd = pIdUsuarioAftd,
                    FechaReg = DateTime.Now,
                    Notas = EnumExtensions.GetDescriptionFromValue<TipoOperacion>(pIdTipoDeOperacion)
                });

                Update(operador);
            }
            catch (Exception)
            {
                //logger
            }
        }
        #endregion

        #region Metodos Privados
        private Operadore Update(Operadore operador)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var transaction = DBContext.Database.BeginTransaction();

                try
                {
                    DBContext.Update(operador);
                    DBContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });

            return operador;
        }
        #endregion
    }
}
