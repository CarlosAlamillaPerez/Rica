using AutoMapper;
using bepensa_socio_selecto_biz.Extensions;
using bepensa_socio_selecto_biz.Interfaces;
using bepensa_socio_selecto_biz.Security;
using bepensa_socio_selecto_data.data;
using bepensa_socio_selecto_data.models;
using bepensa_socio_selecto_models.CRM;
using bepensa_socio_selecto_models.DataModels;
using bepensa_socio_selecto_models.DTO;
using bepensa_socio_selecto_models.Enums;
using bepensa_socio_selecto_models.General;
using Microsoft.EntityFrameworkCore;

namespace bepensa_socio_selecto_biz.Proxies
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

                var operador = await DBContext.Operadores.FirstOrDefaultAsync(u => u.Email == pCredenciales.Email);

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
                    IdTipoOperacion = (int)TipoOperacion.InicioSesion,
                    Notas = TipoOperacion.InicioSesion.GetDescription(),
                    FechaReg = fechaActual
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
                IdTipoOperacion = (int)TipoOperacion.BloqueoCuenta,
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
        public Respuesta<List<SeccionDTO>> ConsultaMenuOperador(int idrol)
        {
            Respuesta<List<SeccionDTO>> respuesta = new();

            try
            {
                var secciones = DBContext.SeccionesPorRols
                    .Include(x => x.IdseccionNavigation)
                    .Where(x => x.Idrol == idrol)
                    .Select(x => x.IdseccionNavigation);

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
