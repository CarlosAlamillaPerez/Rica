namespace bepensa_biz.Interfaces
{
    public interface IBitacora
    {
        void BitacoraDeOperadores(int pIdOperador, int pIdTipoDeOperacion, int? pIdUsuarioAftd = null, int? pIdOperadorAftd = null);
    }
}
