using Microsoft.AspNetCore.Mvc.Rendering;

namespace bepensa_biz.Interfaces
{
    public interface IDropDownList
    {
        List<SelectListItem> TiposLlamada();

        List<SelectListItem> CategoriasLlamada();

        List<SelectListItem> SubcategoriasLlamada(int idCategoria);

        List<SelectListItem> EstatusLlamada();
    }
}
