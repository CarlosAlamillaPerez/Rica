using AutoMapper;
using bepensa_biz.Interfaces;
using bepensa_data.data;
using bepensa_data.StoredProcedures.Models;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bepensa_biz.Proxies
{
    public class ReportesProxy : ProxyBase, IReporte
    {
        private readonly IMapper mapper;
        public ReportesProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<ReporteDTO> GetDataReporte(int IdReporte)
        {
            Respuesta<ReporteDTO> resultado = new();

            try
            {
                resultado.Data = mapper.Map<ReporteDTO>(DBContext.Reportes.Find(IdReporte));
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<ReporteDTO>> TableroDescargas()
        {
            Respuesta<List<ReporteDTO>> resultado = new();

            try
            {
                resultado.Data = mapper.Map<List<ReporteDTO>>(DBContext.Reportes
                    .Where(x => x.IdEstatus == (int)TipoEstatus.Activo)
                    .ToList());
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public Respuesta<List<dynamic>> ReportesDinamico(ReporteRequest data)
        {
            Respuesta<List<dynamic>> respuesta = new();

            try
            {
                DBContext.Database.SetCommandTimeout(300);

                var sp = DBContext.Reportes.First(x => x.Id == data.IdReporte);

                var name = sp.StoreProcedure;

                int bitCanal = sp.BitCanal;

                int totalCanales = DBContext.Canales.Where(x => (x.BitValue & bitCanal) == x.BitValue).Count();

                var parameters = new List<SqlParameter>
                {
                    new("@FechaInicio", (object?)data.FechaInicial ?? DBNull.Value),
                    new("@FechaFin", (object?)data.FechaFinal ?? DBNull.Value),
                    new("@IdCanal", totalCanales <= 1 ? sp.IdCanal : DBNull.Value)
                };

                var sql = $"EXEC {name} @FechaInicio, @FechaFin, @IdCanal";

                var query = DBContext.ReporteDinamico
                    .FromSqlRaw(sql, parameters.ToArray())
                    .ToList();

                string? jsonString = query?.FirstOrDefault()?.JsonString;

                if (!string.IsNullOrEmpty(jsonString))
                {
                    var dynamicObject = JsonConvert.DeserializeObject<List<JToken>>(jsonString)?.Cast<dynamic>().ToList();

                    respuesta.Data = dynamicObject;
                }
            }
            catch (Exception)
            {
                respuesta.Codigo = (int)CodigoDeError.Excepcion;
                respuesta.Mensaje = CodigoDeError.Excepcion.GetDescription();
                respuesta.Exitoso = false;
            }

            return respuesta;
        }
    }
}
