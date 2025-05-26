using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class PremiosProxy : ProxyBase, IPremio
    {
        private readonly IMapper mapper;

        private readonly GlobalSettings _ajustes;

        private readonly PremiosSettings _premiosSettings;

        private string UrlPremio { get; } // Recuerda añadir el nombre de la imagen y extesión a la cual apuntas.

        private string UrlCategoria { get; } // Recuerda añadir el nombre de la imagen y extesión a la cual apuntas.

        public PremiosProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premiosSettings, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;

            _ajustes = ajustes.Value;
            _premiosSettings = premiosSettings.Value;

            UrlPremio = _ajustes.Produccion ? _premiosSettings.MultimediaPremio.UrlProd : _premiosSettings.MultimediaPremio.UrlQA;
            UrlCategoria = _ajustes.Produccion ? _premiosSettings.MultimediaCategoria.UrlProd : _premiosSettings.MultimediaCategoria.UrlQA;
        }

        public Respuesta<List<CategoriaDePremioDTO>> ConsultarCategorias()
        {
            Respuesta<List<CategoriaDePremioDTO>> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                if (!DBContext.CategoriasDePremios.Any(x => x.IdEstatus == (int)TipoDeEstatus.Activo && x.Visible == true))
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var categorias = DBContext.CategoriasDePremios.Where(x => x.IdEstatus == (int)TipoDeEstatus.Activo && x.Visible == true).ToList();

                categorias.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.Imgurl))
                    {
                        c.Imgurl = UrlCategoria + c.Imgurl;
                    }
                });

                resultado.Data = mapper.Map<List<CategoriaDePremioDTO>>(categorias);
            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            }

            return resultado;
        }

        public Respuesta<List<PremioDTO>> ConsultarPremios(int pIdCategoriaDePremio)
        {
            Respuesta<List<PremioDTO>> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                if (!DBContext.CategoriasDePremios.Any(c => c.Id == pIdCategoriaDePremio))
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription() + " Categoría no encontrada";
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!DBContext.Premios.Any(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.IdCategoriaDePremio == pIdCategoriaDePremio))
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var premios = DBContext.Premios
                                    .Include(p => p.IdMetodoDeEntregaNavigation)
                                    .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.IdCategoriaDePremio == pIdCategoriaDePremio)
                                    .ToList();

                premios.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.Imagen))
                    {
                        p.Imagen = $"{UrlPremio}{p.Imagen}";
                    }
                });

                resultado.Data = mapper.Map<List<PremioDTO>>(premios);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<PremioDTO> ConsultarPremioById(int pId)
        {
            Respuesta<PremioDTO> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                if (!DBContext.Premios.Any(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.Id == pId))
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var premio = DBContext.Premios
                                    .Include(p => p.IdMetodoDeEntregaNavigation)
                                    .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.Id == pId)
                                    .First();

                if (!string.IsNullOrEmpty(premio.Imagen))
                {
                    premio.Imagen = $"{UrlPremio}{premio.Imagen}";
                }

                resultado.Data = mapper.Map<PremioDTO>(premio);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<PremioDTO>> ConsultarPremiosByPuntos(int pPuntos)
        {
            Respuesta<List<PremioDTO>> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                int totalPremios = 3;

                var premios = DBContext.Premios
                    .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo
                        && p.Visible == true
                        && p.Puntos <= pPuntos)
                    .Take(totalPremios)
                    .Include(p => p.IdMetodoDeEntregaNavigation)
                    .ToList();

                if (premios.Count < totalPremios)
                {
                    totalPremios = totalPremios - premios.Count;

                    var idsYaIncluidos = premios.Select(p => p.Id).ToList();

                    var premiosAdicionales = DBContext.Premios
                        .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo
                            && p.Visible == true
                            && !idsYaIncluidos.Contains(p.Id))
                        .OrderBy(x => x.Precio)
                        .Take(totalPremios)
                        .Include(p => p.IdMetodoDeEntregaNavigation)
                        .ToList();

                    premios.AddRange(premiosAdicionales);
                }

                premios.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.Imagen))
                    {
                        p.Imagen = $"{UrlPremio}{p.Imagen}";
                    }
                });

                resultado.Data = mapper.Map<List<PremioDTO>>(premios);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
    }
}
