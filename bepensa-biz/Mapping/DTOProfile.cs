using AutoMapper;
using bepensa_data.models;
using bepensa_models.DataModels;
using bepensa_models.DTO;

namespace bepensa_biz.Mapping;

public class DTOProfile : Profile
{
    public DTOProfile()
    {
        CreateMap<Usuario, InscripcionDTO>()
            .ForMember(dest => dest.Programa, opt => opt.MapFrom(src => src.IdProgramaNavigation))
            .ForMember(dest => dest.Ruta, opt => opt.MapFrom(src => src.IdRutaNavigation))
            .ForMember(dest => dest.Cedi, opt => opt.MapFrom(src => src.IdCediNavigation))
            .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.IdSupervisorNavigation))
            .ForMember(dest => dest.TipoRuta, opt => opt.MapFrom(src => src.IdRutaNavigation != null ? src.IdRutaNavigation.Nombre : null))
            .ForMember(dest => dest.CodigoPostal, opt => opt.MapFrom(src => src.IdColoniaNavigation != null ? src.IdColoniaNavigation.Cp : null));

        CreateMap<Canale, CanalDTO>();

        CreateMap<Programa, ProgramaDTO>()
            .ForMember(dest => dest.Canal, opt => opt.MapFrom(src => src.IdCanalNavigation));

        CreateMap<Ruta, RutaDTO>();

        CreateMap<Cedi, CediDTO>()
            .ForMember(dest => dest.Zona, opt => opt.MapFrom(src => src.IdZonaNavigation));

        CreateMap<Supervisore, SupervisorDTO>();

        CreateMap<Zona, ZonaDTO>()
            .ForMember(dest => dest.Embotelladora, opt => opt.MapFrom(src => src.IdEmbotelladoraNavigation));

        CreateMap<Embotelladora, EmbotelladoraDTO>();

        CreateMap<Estado, EstadoDTO>()
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado1));

        CreateMap<Municipio, MunicipioDTO>()
            .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio1))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.IdEstadoNavigation));

        CreateMap<Colonia, ColoniaDTO>()
            .ForMember(dest => dest.Colonia, opt => opt.MapFrom(src => src.Colonia1))
            .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.IdMunicipioNavigation));

        CreateMap<Operadore, OperadorDTO>()
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.IdRolNavigation));

        CreateMap<Role, RolDTO>();

        CreateMap<Seccione, SeccionDTO>();

        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(dest => dest.Programa, opt => opt.MapFrom(src => src.IdProgramaNavigation.Nombre))
            .ForMember(dest => dest.Canal, opt => opt.MapFrom(src => src.IdProgramaNavigation.IdCanalNavigation.Nombre))
            .ForMember(dest => dest.Ruta, opt => opt.MapFrom(src => src.IdRutaNavigation != null ? src.IdRutaNavigation.Nombre : null))
            .ForMember(dest => dest.Cedi, opt => opt.MapFrom(src => src.IdCediNavigation.Nombre))
            .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.IdSupervisorNavigation.Nombre))
            .ForMember(dest => dest.CodigoPostal, opt => opt.MapFrom(src => src.IdColoniaNavigation != null ? src.IdColoniaNavigation.Cp : null));

        CreateMap<Periodo, PeriodoDTO>();

        CreateMap<MetasMensuale, MetaMensualDTO>()
            .ForMember(dest => dest.ImportePorComprar, opt => opt.MapFrom(src => (src.Meta - src.ImporteComprado) < 0 ? 0 : (src.Meta - src.ImporteComprado)));

        CreateMap<SubconceptosDeAcumulacion, PortafolioPrioritarioDTO>()
            .ForMember(dest => dest.EstatusProductosSelectos, opt => opt.MapFrom(src => src.ProductosSelectos));

        CreateMap<ProductosSelecto, EstatusProdSelectDTO>();
            //.ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))
            

        CreateMap<CategoriasDePremio, CategoriaDePremioDTO>();

        CreateMap<Premio, PremioDTO>();

        CreateMap<BitacoraEnvioCorreo, BitacoraEnvioCorreoDTO>();
    }
}
