using AutoMapper;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_biz.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json.Linq;
using System;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using bepensa_biz.Extensions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace bepensa_biz.Proxies
{
    public class BitacoraEnvioCorreoProxy : ProxyBase, IBitacoraEnvioCorreo
    {
        private readonly IMapper mapper;

        public BitacoraEnvioCorreoProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public Respuesta<BitacoraEnvioCorreoDTO> ConsultaByToken(Guid? token)
        {
            Respuesta<BitacoraEnvioCorreoDTO> resultado = new();

            if (!DBContext.BitacoraEnvioCorreos.Any(x => x.Token == token))
            {
                resultado.Codigo = (int)CodigoDeError.InvalidToken;
                resultado.Mensaje = CodigoDeError.InvalidToken.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            var verificarToken = DBContext.BitacoraEnvioCorreos.First(x => x.Token == token);

            if (!(verificarToken.IdEstatus == (int)TipoDeEstatus.CodigoActivo))
            {
                resultado.Codigo = (int)CodigoDeError.LigaPassUtilizada;
                resultado.Mensaje = CodigoDeError.LigaPassUtilizada.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = mapper.Map<BitacoraEnvioCorreoDTO>(verificarToken);

            return resultado;
        }

        /// <summary>
        /// Desactiva el estado del registro para recuperar la contraseña
        /// </summary>
        /// <param name="idBitacoraEnvioCorreo"></param>
        /// <param name="estatus"></param>
        /// <returns>Registro inactivo de la bitacora de envio de correo</returns>
        public Respuesta<BitacoraEnvioCorreoDTO> ActualizaEstatus(long idBitacoraEnvioCorreo, TipoDeEstatus estatus)
        {
            Respuesta<BitacoraEnvioCorreoDTO> resultado = new();
            try
            {
                if (DBContext.BitacoraEnvioCorreos.Any(x => x.Id == idBitacoraEnvioCorreo))
                {
                    var query = DBContext.BitacoraEnvioCorreos.FirstOrDefault(x => x.Id == idBitacoraEnvioCorreo);
                    query.IdEstatus = (int)estatus;


                    resultado.Data = mapper.Map<BitacoraEnvioCorreoDTO>(Update(query));

                }
                else
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;
                }
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }
            return resultado;
        }

        public Respuesta<PlantillaCorreoDTO> ConsultarPlantilla(string pCodigo, int idUsuario, int idPeriodo)
        {
            Respuesta<PlantillaCorreoDTO> resultado = new();

            try
            {
                switch (pCodigo)
                {
                    case "edo-cta-ss":
                        var data = new
                        {
                            IdUsuario = idUsuario,
                            IdPeriodo = idPeriodo,
                            HTMLOutPut = (string?)null
                        };

                        var parametros = Extensiones.CrearSqlParametrosDelModelo(data);

                        foreach (var param in parametros)
                        {
                            if (param.ParameterName == "@HTMLOutPut")
                            {
                                param.SqlDbType = System.Data.SqlDbType.NVarChar;
                                param.Size = -1; // Esto equivale a NVARCHAR(MAX)
                                param.Direction = System.Data.ParameterDirection.Output;
                            }
                        }

                        if (!DBContext.HistoricoDeCortesCuenta.Any(x => x.IdUsuario == idUsuario && x.IdPeriodo == idPeriodo))
                        {
                            resultado.Codigo = (int)CodigoDeError.EdoCtaNoEncontrado;
                            resultado.Mensaje = CodigoDeError.EdoCtaNoEncontrado.GetDescription();
                            resultado.Exitoso = false;

                            return resultado;
                        }

                        var exec = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[sp_CatalogoCorreos_ConsultarEstadoCuenta] @IdUsuario, @IdPeriodo, @HTMLOutPut OUTPUT", parametros);

                        var valida = parametros.FirstOrDefault(p => p.ParameterName == "@HTMLOutPut");

                        if (valida == null)
                        {
                            resultado.Codigo = (int)CodigoDeError.ErrorDesconocido;
                            resultado.Mensaje = CodigoDeError.ErrorDesconocido.GetDescription();
                            resultado.Exitoso = false;

                            return resultado;
                        }

                        resultado.Data = new PlantillaCorreoDTO
                        {
                            Html = (string)valida.Value
                        };
                        break;
                    default:
                        resultado.Exitoso = false;
                        break;
                }
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        private BitacoraEnvioCorreo Update(BitacoraEnvioCorreo data)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var transaction = DBContext.Database.BeginTransaction();

                try
                {
                    DBContext.Update(data);
                    DBContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });

            return data;
        }
    }
}
