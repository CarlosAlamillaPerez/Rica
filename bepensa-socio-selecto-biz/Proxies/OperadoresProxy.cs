using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Security;
using bepensa_data.data;
using bepensa_data.models;
using bepensa_models.CRM;
using bepensa_models.DataModels;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_biz.Proxies
{
    public class OperadoresProxy : ProxyBase, IOperador
    {
        private readonly IMapper mapper;

        public OperadoresProxy(BepensaContext context, IMapper mapper)
        {
            DBContext = context;
            this.mapper = mapper;
        }

        public async Task<Respuesta<OperadorDTO>> ValidaAcceso(LoginCRMRequest pCredenciales)
        {
            Respuesta<OperadorDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid()
            };

            try
            {
                var valida = Extensiones.ValidateRequest(pCredenciales);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var passwordHash = new Hash(pCredenciales.Password);

                var _password = passwordHash.ToSha512();

                var password = _password;

                if (!await DBContext.Operadores.AnyAsync(u => u.Email == pCredenciales.Email && u.Password == password))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!await DBContext.Operadores.AnyAsync(u => u.Email == pCredenciales.Email && u.IdEstatus == (int)TipoEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                var operador = await DBContext.Operadores.Include(u => u.BitacoraDeOperadoreIdOperadorNavigations).FirstOrDefaultAsync(u => u.Email == pCredenciales.Email);

                if (operador == null)
                {
                    resultado.Codigo = (int)CodigoDeError.EmailInvalido;
                    resultado.Mensaje = CodigoDeError.EmailInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var fechaActual = DateTime.Now;

                BitacoraDeOperadore bdo = new()
                {
                    IdOperador = operador.Id,
                    IdTipoDeOperacion = (int)TipoOperacion.InicioSesion,
                    FechaReg = fechaActual,
                    Notas = TipoOperacion.InicioSesion.GetDescription()
                };

                operador.SessionId = resultado.IdTransaccion.ToString();

                operador.BitacoraDeOperadoreIdOperadorNavigations.Add(bdo);

                var operadorActualizado = await Update(operador);

                resultado.Data = mapper.Map<OperadorDTO>(operadorActualizado);
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<Empty>> BloquearOperador(EmailRequest Email)
        {
            Respuesta<Empty> resultado = new()
            {
                IdTransaccion = Guid.NewGuid()
            };

            BitacoraDeOperadore bdo = new()
            {
                IdTipoDeOperacion = (int)TipoOperacion.BloqueoCuenta,
                FechaReg = DateTime.Now,
                Notas = TipoOperacion.BloqueoCuenta.GetDescription()
            };

            try
            {
                if (await DBContext.Operadores.AnyAsync(x => x.Email == Email.Email))
                {
                    var operador = DBContext.Operadores.FirstOrDefault(x => x.Email == Email.Email);

                    if (operador != null)
                    {
                        bdo.IdOperador = operador.Id;
                        operador.IdEstatus = (int)TipoEstatus.Bloqueada;
                        operador.BitacoraDeOperadoreIdOperadorNavigations.Add(bdo);

                        await Update(operador);
                    }
                    else
                    {
                        throw new InvalidOperationException(CodigoDeError.ConexionFallida.GetDescription());
                    }
                }
                else
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
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

        #region Método con uso recomendable en web
        public async Task<Respuesta<List<SeccionDTO>>> ConsultaMenuOperador(int idrol)
        {
            Respuesta<List<SeccionDTO>> respuesta = new();

            try
            {
                var secciones = await DBContext.SeccionesPorRols
                    .Where(x => x.Idrol == idrol)
                    .Select(x => x.IdseccionNavigation)
                    .ToListAsync();

                respuesta.Codigo = 0;
                respuesta.Mensaje = string.Empty;
                respuesta.Exitoso = true;
                respuesta.Data = mapper.Map<List<SeccionDTO>>(secciones);
            }
            catch (Exception ex)
            {
                respuesta.Data = null;
                respuesta.Codigo = (int)CodigoDeError.Excepcion;
                respuesta.Exitoso = false;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }
        #endregion

        #region Métodos Privados
        private async Task<Operadore> Update(Operadore operador)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            await strategy.Execute(async () =>
            {
                await using (var transaction = await DBContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        DBContext.Update(operador);
                        await DBContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });

            return operador;
        }
        #endregion
    }
}
