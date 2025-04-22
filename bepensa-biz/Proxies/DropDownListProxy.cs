using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class DropDownListProxy : ProxyBase, IDropDownList
    {
        private readonly GlobalSettings _ajustes;

        public DropDownListProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes)
        {
            DBContext = context;
            _ajustes = ajustes.Value;
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

        public List<SelectListItem> PeriodosEdoCta(int idUsuario)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var primerDiaDelMesActual = new DateOnly(hoy.Year, hoy.Month, 1);
            var primerDiaDelProximoMes = primerDiaDelMesActual.AddMonths(1);

            DateOnly? fechaMax = DBContext.Movimientos
                .Where(x => x.IdUsuario == idUsuario)
                .Select(x => (DateOnly?)x.IdPeriodoNavigation.Fecha)
                .ToList()
                .DefaultIfEmpty(null)
                .Max();

            var periodos = DBContext.Periodos
                .Where(x => x.Fecha >= _ajustes.PeriodoInicial && ((fechaMax != null && x.Fecha <= fechaMax) || x.Fecha < primerDiaDelProximoMes));

            return periodos.OrderByDescending(x => x.Fecha).Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = x.Id.ToString()
            })
            .ToList();
        }

        public List<SelectListItem> SuperLlamadas(int idUsuario) => DBContext.Llamadas
            .Where(x => x.IdUsuario != null && x.IdUsuario == idUsuario && x.IdPadre == null && x.IdEstatusLlamada != (int)TipoLlamada.Cerrada)
            .OrderByDescending(x => x.Id)
            .Select(x => new SelectListItem
            {
                Text = x.Id.ToString() + "-" + x.Tema,
                Value = x.Id.ToString()
            })
            .ToList();
    }
}
