using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_biz.Settings;
using bepensa_data.data;
using bepensa_models;
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
    
    public Respuesta<HeaderEdoCtaDTO> Header(string pCuc, int pYear, int pMes)
    {
        HeaderEdoCtaDTO _header = new HeaderEdoCtaDTO();
        _header.puntosGanados = 0;
        _header.puntosCanjeados = 0;
        _header.canjesRealizados = 0;

        Respuesta<HeaderEdoCtaDTO> _respuesta = new Respuesta<HeaderEdoCtaDTO>();

        _respuesta.Codigo = 0;
        _respuesta.Mensaje = string.Empty;
        _respuesta.Exitoso = true;
        _respuesta.Data = _header;

        return _respuesta;
    }

    public Respuesta<EdoCtaDTO> MisPuntos(string pCuc, int pYear, int pMes)
    {
        EdoCtaDTO _edocta = new EdoCtaDTO();

        _edocta.fecha = new DateTime(pYear, pMes, DateTime.Now.Day);
        _edocta.puntosObjetivo = 0;
        _edocta.puntosEjecucion = 0;
        _edocta.puntosPortafolio = 0;
        _edocta.puntosFotoExito = 0;
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

    public Respuesta<List<DetalleCanjeDTO>> DetalleCanje(string pCuc, int pYear, int pMes)
    {
        Respuesta<List<DetalleCanjeDTO>> _respuesta = new Respuesta<List<DetalleCanjeDTO>>();

        List<DetalleCanjeDTO> _lstDetalleCanje = new List<DetalleCanjeDTO>();
        
        _respuesta.Codigo = 0;
        _respuesta.Mensaje = string.Empty;
        _respuesta.Exitoso = true;
        _respuesta.Data = _lstDetalleCanje;

        return _respuesta;
    }
}
