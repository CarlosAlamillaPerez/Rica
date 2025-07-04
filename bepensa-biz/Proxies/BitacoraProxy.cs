using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.Enums;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class BitacoraProxy : ProxyBase, IBitacora
    {
        private readonly Serilog.ILogger _logger;

        public BitacoraProxy(BepensaContext context, Serilog.ILogger logger)
        {
            DBContext = context;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.Error(ex, "BitacoraDeOperadores(int32, int32, int32?, int32?) => IdOperador::{usuario}", pIdOperador);
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
