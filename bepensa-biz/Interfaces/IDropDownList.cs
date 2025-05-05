using Microsoft.AspNetCore.Mvc.Rendering;

namespace bepensa_biz.Interfaces
{
    public interface IDropDownList
    {
        List<SelectListItem> TiposLlamada();

        List<SelectListItem> CategoriasLlamada();

        List<SelectListItem> SubcategoriasLlamada(int idCategoria);

        List<SelectListItem> EstatusLlamada();

        public List<SelectListItem> PeriodosEdoCta(int idUsuario);

        List<SelectListItem> SuperLlamadas(int idUsuario);
    }
}
