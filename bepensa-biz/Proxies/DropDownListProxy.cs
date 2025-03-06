using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bepensa_biz.Proxies
{
    public class DropDownListProxy : ProxyBase, IDropDownList
    {
        public DropDownListProxy(BepensaContext context)
        {
            DBContext = context;
        }

        public List<SelectListItem> TiposLlamada() => DBContext.TiposLlamada
            .Where(tl => tl.IdEstatus == (int)TipoEstatus.Activo)
            .OrderBy(tl => tl.Nombre)
            .Select(tl => new SelectListItem
            {
                Text = tl.Nombre,
                Value = tl.Id.ToString()
            })
            .OrderBy(cl => cl.Text)
            .ToList();

        public List<SelectListItem> CategoriasLlamada() => DBContext.CategoriasLlamada
            .Where(cl => cl.IdEstatus == (int)TipoEstatus.Activo)
            .OrderBy(cl => cl.Nombre)
            .Select(cl => new SelectListItem
            {
                Text = cl.Nombre,
                Value = cl.Id.ToString()
            })
            .OrderBy(cl => cl.Text)
            .ToList();

        public List<SelectListItem> SubcategoriasLlamada(int idCategoria) => DBContext.SubcategoriasLlamada
            .Where(sc => sc.IdCategoriaLlamada == idCategoria && sc.IdEstatus == (int)TipoEstatus.Activo)
            .OrderBy(sc => sc.Nombre)
            .Select(sc => new SelectListItem
            {
                Text = sc.Nombre,
                Value = sc.Id.ToString()
            })
            .OrderBy(cl => cl.Text)
            .ToList();

        public List<SelectListItem> EstatusLlamada() => DBContext.EstatusDeLlamada
            .Select(sl => new SelectListItem
            {
                Text = sl.Nombre,
                Value = sl.Id.ToString()
            })
            .OrderBy(sl => sl.Value)
            .ToList();
    }
}
