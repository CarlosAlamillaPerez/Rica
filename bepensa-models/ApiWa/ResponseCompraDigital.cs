using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.ApiWa
{
    public class ResponseCompraDigital
    {
        public decimal objetivoCompra {  get; set; }
        public string objetivocompraAvance {  get; set; }
        public decimal objetivoImporte { get; set; }
        public decimal objetivoImporteAvance { get; set; }
        public decimal objetivoImporteFalta {  get; set; }
    }
}
