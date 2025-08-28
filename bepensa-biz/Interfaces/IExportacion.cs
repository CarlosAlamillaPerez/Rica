namespace bepensa_biz.Interfaces
{
    public interface IExportacion
    {
        byte[] GeneraExportacion<T>(string requerimiento, string NameSheet, FileInfo logo, List<T> data, int StyleTable, DateTime? PeriodoI, DateTime? PeriodoF, string reporte);

        byte[] GeneraExportacionDinamica<T>(int IdReporte, FileInfo logo, List<T> data, DateTime? PeriodoI, DateTime? PeriodoF);

        Task<byte[]> GeneraExportacionDinamicaAsync<T>(int IdReporte, FileInfo logo, List<T> data, DateTime? PeriodoI, DateTime? PeriodoF);
    }
}
