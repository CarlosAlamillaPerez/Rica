using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bepensa_models.ApiWa
{
    public class ResponseMeta
    {
        public string mes { get; set; }
        public decimal objetivo { get; set; }
        public decimal objetivoavance {  get; set; }
        public string avance {  get; set; }
        public decimal tefalta {  get; set; }
    }
}
