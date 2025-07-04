using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bepensa_biz.Proxies
{
    public class PremiosProxy : ProxyBase, IPremio
    {
        private readonly GlobalSettings _ajustes;

        private readonly PremiosSettings _premiosSettings;

        private readonly Serilog.ILogger _logger;

        private readonly IMapper mapper;

        private readonly IApi _api;

        private string UrlPremio { get; } // Recuerda añadir el nombre de la imagen y extesión a la cual apuntas.

        private string UrlCategoria { get; } // Recuerda añadir el nombre de la imagen y extesión a la cual apuntas.

        public PremiosProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premiosSettings,
                            Serilog.ILogger logger, IMapper mapper, IApi api)
        {
            DBContext = context;
            _logger = logger;
            this.mapper = mapper;

            _ajustes = ajustes.Value;
            _premiosSettings = premiosSettings.Value;

            UrlPremio = _ajustes.Produccion ? _premiosSettings.MultimediaPremio.UrlProd : _premiosSettings.MultimediaPremio.UrlQA;
            UrlCategoria = _ajustes.Produccion ? _premiosSettings.MultimediaCategoria.UrlProd : _premiosSettings.MultimediaCategoria.UrlQA;
            _api = api;
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
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "ConsultarCategorias() => Empty");
            }

            return resultado;
        }

        public async Task<Respuesta<List<PremioDTO>>> ConsultarPremios(int pIdCategoriaDePremio, int? idUsuario)
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
                                    .Include(p => p.Tarjeta.Where(x => idUsuario != null && x.IdUsuario == idUsuario && x.IdEstatus == (int)TipoEstatus.Activo))
                                    .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.IdCategoriaDePremio == pIdCategoriaDePremio)
                                    .ToList();


                resultado.Data = mapper
                    .Map<List<PremioDTO>>(premios
                    .Where(x => !x.RequiereTarjeta
                        || (
                            x.RequiereTarjeta
                            && x.Tarjeta.Count > 0
                        )));

                resultado.Data.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.Imagen))
                    {
                        p.Imagen = $"{UrlPremio}{p.Imagen}";
                    }
                });

                var premiosDigitales = premios.Where(x => x.IdTipoDePremio == (int)TipoPremio.Digital).Select(x => x.Sku).ToList();

                if (premiosDigitales.Count > 0)
                {
                    var validaSku = await _api.Disponibilidad(premiosDigitales);

                    if (validaSku.Data != null)
                    {
                        var skuEliminar = validaSku.Data?.Resultado?.Where(x => x.Disponibilidad == 0).Select(x => x.Sku).ToList();

                        if (skuEliminar != null && skuEliminar.Count != 0)
                        {
                            resultado.Data.RemoveAll(item => skuEliminar.Contains(item.Sku) && item.IdTipoDePremio == (int)TipoPremio.Digital);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPremios(int32, int32?) => IdCategoriaDePremio::{usuario}", pIdCategoriaDePremio);
            }

            return resultado;
        }

        public async Task<Respuesta<PremioDTO>> ConsultarPremioById(int pId, int? idUsuario)
        {
            Respuesta<PremioDTO> resultado = new() { IdTransaccion = Guid.NewGuid() };

            try
            {
                if (!DBContext.Premios.Any(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.Id == pId))
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var premio = DBContext.Premios
                    .Include(p => p.IdMetodoDeEntregaNavigation)
                    .Include(p => p.Tarjeta.Where(x => idUsuario != null && x.IdUsuario == idUsuario && x.IdEstatus == (int)TipoEstatus.Activo))
                    .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo && p.Visible == true && p.Id == pId)
                    .First();

                if (premio == null)
                {
                    resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                    resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (premio.RequiereTarjeta)
                {
                    if (premio.Tarjeta.Count == 0)
                    {
                        resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                        resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }
                }

                resultado.Data = mapper.Map<PremioDTO>(premio);

                if (!string.IsNullOrEmpty(resultado.Data.Imagen))
                {
                    resultado.Data.Imagen = $"{UrlPremio}{resultado.Data.Imagen}";
                }

                if (resultado.Data.IdTipoDePremio == (int)TipoPremio.Digital)
                {
                    var validaSku = await _api.Disponibilidad(resultado.Data.Sku);

                    if (validaSku.Data != null)
                    {
                        var skuEliminar = validaSku.Data?.Resultado?.Where(x => x.Disponibilidad == 0).Select(x => x.Sku).ToList();

                        if (skuEliminar != null && skuEliminar.Count != 0)
                        {
                            if (skuEliminar.Any(x => x == resultado.Data.Sku) && resultado.Data.IdTipoDePremio == (int)TipoPremio.Digital)
                            {
                                resultado.Codigo = (int)CodigoDeError.PremioNoEncontrado;
                                resultado.Mensaje = CodigoDeError.PremioNoEncontrado.GetDescription();
                                resultado.Exitoso = false;

                                return resultado;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPremioById(int32, int32?) => IdPremio::{usuario}", pId);
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

                resultado.Data = mapper.Map<List<PremioDTO>>(premios);

                if (resultado.Data.Count < totalPremios)
                {
                    totalPremios = totalPremios - resultado.Data.Count;

                    var idsYaIncluidos = resultado.Data.Select(p => p.Id).ToList();

                    var premiosAdicionales = DBContext.Premios
                        .Where(p => p.IdEstatus == (int)TipoDeEstatus.Activo
                            && p.Visible == true
                            && !idsYaIncluidos.Contains(p.Id))
                        .OrderBy(x => x.Precio)
                        .Take(totalPremios)
                        .Include(p => p.IdMetodoDeEntregaNavigation)
                        .ToList();

                    resultado.Data.AddRange(mapper.Map<List<PremioDTO>>(premiosAdicionales));
                }

                resultado.Data.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.Imagen))
                    {
                        p.Imagen = $"{UrlPremio}{p.Imagen}";
                    }
                });
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ConsultarPremiosByPuntos(int32) => Puntos::{usuario}", pPuntos);
            }

            return resultado;
        }
    }
}
