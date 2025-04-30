using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class DireccionesProxy : ProxyBase, IDireccion
    {
        private readonly IMapper mapper;
        public DireccionesProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public async Task<Respuesta<List<ColoniaDTO>>> ConsultarColonias(string pCP)
        {
            Respuesta<List<ColoniaDTO>> resultado = new();

            try
            {
                var valida = string.IsNullOrEmpty(pCP);

                if (valida)
                {
                    resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
                    resultado.Mensaje = CodigoDeError.PropiedadInvalida.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                pCP = pCP.Trim();

                var consultar = await DBContext.Colonias
                    .Include(x => x.IdMunicipioNavigation)
                        .ThenInclude(x => x.IdEstadoNavigation)
                    .Where(c => c.Cp == pCP).ToListAsync();

                if (consultar == null || consultar.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.CodigoPostalNoEncontrado;
                    resultado.Mensaje = CodigoDeError.CodigoPostalNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<List<ColoniaDTO>>(consultar);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<ColoniaDTO> ConsultarColonias(int pIdColonia)
        {
            Respuesta<ColoniaDTO> resultado = new();

            try
            {

                var consultar = DBContext.Colonias
                    .Include(x => x.IdMunicipioNavigation)
                        .ThenInclude(x => x.IdEstadoNavigation)
                    .Where(c => c.Id == pIdColonia)
                    .FirstOrDefault();

                if (consultar == null)
                {
                    resultado.Codigo = (int)CodigoDeError.CodigoPostalNoEncontrado;
                    resultado.Mensaje = CodigoDeError.CodigoPostalNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<ColoniaDTO>(consultar);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<MunicipioDTO>> ConsultarMunicipio(int pIdColonia)
        {
            Respuesta<MunicipioDTO> resultado = new();

            try

            {
                var valida = pIdColonia <= 0;

                if (valida)
                {
                    resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
                    resultado.Mensaje = CodigoDeError.PropiedadInvalida.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var consultar = await DBContext.Colonias.Where(c => c.Id == pIdColonia).Select(c => c.IdMunicipioNavigation).FirstOrDefaultAsync();

                if (consultar == null)
                {
                    resultado.Codigo = (int)CodigoDeError.CodigoPostalNoEncontrado;
                    resultado.Mensaje = CodigoDeError.CodigoPostalNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<MunicipioDTO>(consultar);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<EstadoDTO>> ConsultarEstado(int pIdMunicipio)
        {
            Respuesta<EstadoDTO> resultado = new();

            try

            {
                var valida = pIdMunicipio <= 0;

                if (valida)
                {
                    resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
                    resultado.Mensaje = CodigoDeError.PropiedadInvalida.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var consultar = await DBContext.Municipios.Where(c => c.Id == pIdMunicipio).Select(c => c.IdEstadoNavigation).FirstOrDefaultAsync();

                if (consultar == null)
                {
                    resultado.Codigo = (int)CodigoDeError.CodigoPostalNoEncontrado;
                    resultado.Mensaje = CodigoDeError.CodigoPostalNoEncontrado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<EstadoDTO>(consultar);
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
