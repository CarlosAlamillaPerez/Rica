using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_data.StoredProcedures.Models;
using bepensa_models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace bepensa_biz.Proxies;

public class EdoCtaProxy : ProxyBase, IEdoCta
{
    private readonly IMapper mapper;
    private readonly GlobalSettings _ajustes;

    public EdoCtaProxy(BepensaContext context, IOptionsSnapshot<GlobalSettings> ajustes, IOptionsSnapshot<PremiosSettings> premiosSettings, IMapper mapper)
    {
        DBContext = context;
        this.mapper = mapper;

        _ajustes = ajustes.Value;
        //_premiosSettings = premiosSettings.Value;
    }

    public async Task<Respuesta<HeaderEdoCtaDTO>> Header(int pIdUsuario, int pIdPeriodo)
    {
        HeaderEdoCtaDTO _header = new HeaderEdoCtaDTO();
        _header.PuntosGanados = 0;
        _header.PuntosCanjeados = 0;
        _header.CanjesRealizados = 0;

        Respuesta<HeaderEdoCtaDTO> _respuesta = new Respuesta<HeaderEdoCtaDTO>();

        _respuesta.Codigo = 0;
        _respuesta.Mensaje = string.Empty;
        _respuesta.Exitoso = true;
        _respuesta.Data = _header;

        return _respuesta;
    }

    public async Task<Respuesta<EdoCtaDTO>> MisPuntos(int pIdUsuario, int pIdPeriodo)
    {
        var pdo = await DBContext.Periodos.Where(p => p.Id == pIdPeriodo).FirstOrDefaultAsync();

        EdoCtaDTO _edocta = new EdoCtaDTO();

        _edocta.fecha = new DateTime(pdo.Fecha.Year, pdo.Fecha.Month, pdo.Fecha.Day);
        _edocta.puntosObjetivo = 0;
        _edocta.puntosEjecucion = 0;
        _edocta.puntosPortafolio = 0;
        _edocta.puntosFotoExito = 0;
        _edocta.puntosComprasApp = 0;
        _edocta.puntosPromociones = 0;
        _edocta.puntosBienvenida = 0;
        _edocta.puntosCumpleanios = 0;
        _edocta.puntosNivel = 0;
        _edocta.puntosCanje = 0;
        _edocta.puntosCancelaCanje = 0;
        _edocta.puntosAjustePositivo = 0;
        _edocta.puntosAjusteNegativo = 0;
        _edocta.puntosTotal = 0;
        _edocta.puntosComprado = 0;
        _edocta.esAntesRegistro = false;

        Respuesta<EdoCtaDTO> _respuesta = new Respuesta<EdoCtaDTO>();

        _respuesta.Codigo = 0;
        _respuesta.Mensaje = string.Empty;
        _respuesta.Exitoso = true;
        _respuesta.Data = _edocta;

        return _respuesta;
    }

    public async Task<Respuesta<List<DetalleCanjeDTO>>> DetalleCanje(int pIdUsuario, int pIdPeriodo)
    {
        Respuesta<List<DetalleCanjeDTO>> _respuesta = new Respuesta<List<DetalleCanjeDTO>>();

        List<DetalleCanjeDTO> _lstDetalleCanje = new List<DetalleCanjeDTO>();

        _respuesta.Codigo = 0;
        _respuesta.Mensaje = string.Empty;
        _respuesta.Exitoso = true;
        _respuesta.Data = _lstDetalleCanje;

        return _respuesta;
    }

    public async Task<Respuesta<EstadoDeCuentaDTO>> ConsultarEstatdoCuenta(UsuarioPeriodoRequest pUsuario)
    {
        Respuesta<EstadoDeCuentaDTO> resultado = new();

        try
        {
            var valida = Extensiones.ValidateRequest(pUsuario);

            if (!valida.Exitoso)
            {
                resultado.Codigo = valida.Codigo;
                resultado.Mensaje = valida.Mensaje;
                resultado.Exitoso = false;

                return resultado;
            }

            ConceptosEdoCtaCTE consultarEncabezados = await ConsultarEncabezados(pUsuario.IdUsuario, pUsuario.IdPeriodo) ?? new ConceptosEdoCtaCTE();

            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                pUsuario.IdUsuario,
                pUsuario.IdPeriodo
            });

            var consultar = await DBContext.EstadoCuenta
                .FromSqlRaw("EXEC Movimientos_ConsultarConceptosAcumulacion @IdUsuario, @IdPeriodo", parametros)
                .ToListAsync();

            var edoCta = new EstadoDeCuentaDTO
            {
                SaldoAnterior = consultarEncabezados.SaldoAnterior,
                AcumuladoActual = consultarEncabezados.AcumuladoActual,
                PuntosDisponibles = consultarEncabezados.PuntosDisponibles,
                PuntosCanjeados = consultarEncabezados.PuntosCanjeados,
                ConceptosAcumulacion = consultar
                    .Select(c => new AcumulacionEdoCtaDTO
                    {
                        Concepto = c.Concepto,
                        Puntos = c.Puntos
                    }).ToList()
            };

            resultado.Data = edoCta;
        }
        catch (Exception)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;
        }

        return resultado;
    }

    public async Task<Respuesta<CanjeDTO>> ConsultarCanjes(UsuarioPeriodoRequest pUsuario)
    {
        Respuesta<CanjeDTO> resultado = new();

        try
        {
            var valida = Extensiones.ValidateRequest(pUsuario);

            if (!valida.Exitoso)
            {
                resultado.Codigo = valida.Codigo;
                resultado.Mensaje = valida.Mensaje;
                resultado.Exitoso = false;

                return resultado;
            }

            ConceptosEdoCtaCTE consultarEncabezados = await ConsultarEncabezados(pUsuario.IdUsuario, pUsuario.IdPeriodo) ?? new ConceptosEdoCtaCTE();

            var parametros = Extensiones.CrearSqlParametrosDelModelo(new
            {
                pUsuario.IdUsuario,
                pUsuario.IdPeriodo
            });

            var consultar = await DBContext.Canje
                .FromSqlRaw("EXEC Redenciones_ConsultarCanjes @IdUsuario,  @IdPeriodo", parametros)
                .ToListAsync();

            var canjes = new CanjeDTO
            {
                AcumuladoActual = consultarEncabezados.AcumuladoActual,
                PuntosCanjeados = consultarEncabezados.PuntosCanjeados,
                CanjesRealizados = consultarEncabezados.CanjesRealizados,
                Canjes = consultar
                   .Select(c => new DetalleCanjeDTO
                   {
                       Id = c.Id,
                       Folio = c.Folio,
                       FechaCanje = c.FechaCanje,
                       Puntos = c.Puntos,
                       Premio = c.Premio,
                       Nombre = c.Titular,
                       Estatus = c.Estatus,
                       Solicitante = c.Solicitante,
                       Direccion = c.Direccion,
                       Cantidad = c.Cantidad,
                       Observaciones = c.Observaciones,
                       Referencias = c.Referencias,
                   }).ToList()
            };

            resultado.Data = canjes;
        }
        catch (Exception)
        {
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            resultado.Exitoso = false;
        }

        return resultado;
    }

    #region Accesos para Stored Procedure
    private async Task<ConceptosEdoCtaCTE> ConsultarEncabezados(int pIdUsuario, int? pIdPeriodo)
    {
        var parametros = Extensiones.CrearSqlParametrosDelModelo(new
        {
            IdUsuario = pIdUsuario,
            IdPeriodo = pIdPeriodo
        });

        var consultar = await DBContext.EstadoCuentaGeneral
            .FromSqlRaw("EXEC Movimientos_ConsultarEncabezados @IdUsuario,  @IdPeriodo", parametros)
            .ToListAsync();

        return consultar.First();
    }
    #endregion
}
