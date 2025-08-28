using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models;
using bepensa_models.App;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace bepensa_biz;

public class AppProxy : ProxyBase, IApp
{
    private readonly GlobalSettings _config;

    private readonly Serilog.ILogger _logger;

    private readonly IMapper mapper;

    public AppProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> config, Serilog.ILogger logger, IMapper mapper)
    {
        DBContext = context;
        _config = config.Value;
        _logger = logger;
        this.mapper = mapper;
    }

    public async Task<Respuesta<string>> ConsultaParametro(int pParametro)
    {
        Respuesta<string> resultado = new();

        try
        {
            var img = await DBContext.Parametros.Where(p => p.Id == pParametro).FirstOrDefaultAsync();


            if (img == null)
            {
                resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = img.Valor;

        }
        catch (Exception)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;
        }

        return resultado;
    }

    public async Task<Respuesta<List<ImagenesPromocionesDTO>>> ConsultaImgPromociones(int pParametro)
    {
        Respuesta<List<ImagenesPromocionesDTO>> resultado = new();

        try
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);

            int idPeriodoMax = await DBContext.ImagenesPromociones
                .Where(p => p.IdPeriodoNavigation.Fecha <= fechaActual)
                .Select(p => p.IdPeriodo)
                .MaxAsync();

            var img = await DBContext.ImagenesPromociones.Where(i => i.IdCanal == pParametro && i.IdPeriodo == idPeriodoMax).ToListAsync();

            if (img == null)
            {
                resultado.Codigo = (int)CodigoDeError.NonExistentRecord;
                resultado.Mensaje = CodigoDeError.NonExistentRecord.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = mapper.Map<List<ImagenesPromocionesDTO>>(img);

        }
        catch (Exception)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;
        }

        return resultado;
    }

    public async Task<Respuesta<Empty>> SeguimientoVistas(SegVistaRequest data, int idOrigen)
    {
        Respuesta<Empty> resultado = new();

        try
        {
            var valida = Extensiones.ValidateRequest(data);

            if (!valida.Exitoso)
            {
                resultado.Codigo = valida.Codigo;
                resultado.Mensaje = valida.Mensaje;
                resultado.Exitoso = false;

                return resultado;
            }

            var usuario = await DBContext.Usuarios
                .Include(x => x.SeguimientoVista.Where(x => x.FechaFin == null && x.IdOrigen == idOrigen && x.IdVistaNavigation.RequiereFechaFin))
                .FirstOrDefaultAsync(x => x.Id == data.IdUsuario);

            if (usuario == null)
            {
                resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            Guid sesion = Guid.NewGuid();

            DateTime fechaFin = DateTime.Now;

            if (Guid.TryParse(usuario.Sesion, out Guid guid))
                sesion = guid;

            if (data.IdFDV != null && data.IdFDV > 0)
            {
                var fdv = await DBContext.FuerzaVenta.FindAsync(data.IdFDV);

                if (Guid.TryParse(fdv?.SesionId, out Guid guidFDV))
                    sesion = guidFDV;
            }

            var strategy = DBContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await DBContext.Database.BeginTransactionAsync();

                try
                {
                    if (usuario.SeguimientoVista.Count > 0)
                    {
                        usuario.SeguimientoVista.ToList().ForEach(x =>
                        {
                            if (sesion != x.IdTransaccionLog)
                            {
                                TimeSpan diferencia = fechaFin - x.FechaReg;

                                if (diferencia.TotalMinutes > _config.Sesion.Expiracion)
                                {
                                    x.FechaFin = x.FechaReg.AddMinutes((int)_config.Sesion.Expiracion);
                                }
                                else
                                {
                                    x.FechaFin = fechaFin;
                                }
                            }
                            else
                            {
                                x.FechaFin = fechaFin;
                            }
                        });
                    }

                    usuario.SeguimientoVista.Add(new SeguimientoVista
                    {
                        IdVista = data.IdVista,
                        IdFdvaftd = data.IdFDV,
                        FechaReg = DateTime.Now,
                        IdOrigen = idOrigen,
                        IdTransaccionLog = sesion
                    });

                    await DBContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();

                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;

            _logger.Error(ex, "SeguimientoVistas(SegVistaRequest, int32) => IdUsuario::{usuario}", data.IdUsuario);
        }

        return resultado;
    }

    public Respuesta<List<CanalDTO>> ConsultarCanales()
    {
        Respuesta<List<CanalDTO>> resultado = new();

        try
        {
            resultado.Data = mapper.Map<List<CanalDTO>>(DBContext.Canales.ToList());
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
