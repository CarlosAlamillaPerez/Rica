using bepensa_models.General;

namespace bepensa_models.ApiWa
{
    public class AvanceWa
    {
        public ResponseMeta meta {  get; set; }
        public ResponsePortafolio portafolio { get; set; }
        public ResponseFotoExito fotoexito { get; set; }
        public ResponseCompraDigital compraDigital { get; set; }
        public ResponsePortafolioComidas portafoliocomidas { get; set; }
        //public List<string> porta {  get; set; }
    }
}
