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
using System.IO.Pipelines;
using System.Text;

namespace bepensa_biz.Proxies
{
    public class UsuariosProxy : ProxyBase, IUsuario
    {
        private readonly AppConfigSettings _config;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper mapper;

        private readonly IBitacoraDeContrasenas _bitacoraDeContrasenas;
        private readonly IEnviarCorreo _enviarCorreo;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;
        private readonly IAppEmail appEmail;

        public UsuariosProxy(BepensaContext context, IOptionsSnapshot<AppConfigSettings> config, Serilog.ILogger logger, IMapper mapper,
                                IEnviarCorreo enviarCorreo, IBitacoraDeContrasenas bitacoraDeContrasenas,
                                IBitacoraEnvioCorreo bitacoraEnvioCorreo, IAppEmail appEmail)
        {
            DBContext = context;
            _config = config.Value;
            _logger = logger;
            this.mapper = mapper;

            _bitacoraDeContrasenas = bitacoraDeContrasenas;
            _enviarCorreo = enviarCorreo;
            _bitacoraEnvioCorreo = bitacoraEnvioCorreo;
            this.appEmail = appEmail;
        }
        #region CRM
        public async Task<Respuesta<List<UsuarioDTO>>> BuscarUsuario(BuscarRequest pBuscar)
        {
            Respuesta<List<UsuarioDTO>> resultado = new();

            try
            {
                pBuscar.Buscar = pBuscar.Buscar.Trim();

                var valida = Extensiones.ValidateRequest(pBuscar);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }
                List<Usuario>? consulta = null;

                if (long.TryParse(pBuscar.Buscar, out long clvUsuario))
                {
                    consulta = await DBContext.Usuarios.Where(us => us.Id == clvUsuario || us.Cuc.Equals(pBuscar.Buscar)).ToListAsync();
                }
                else
                {
                    consulta = await DBContext.Usuarios
                                    .Where(us =>
                                        us.Id == clvUsuario ||
                                        us.Cuc.Contains(pBuscar.Buscar) ||
                                        (us.Nombre != null && us.Nombre.Contains(pBuscar.Buscar)) ||
                                        (us.ApellidoPaterno != null && us.ApellidoPaterno.Contains(pBuscar.Buscar)) ||
                                        (us.ApellidoMaterno != null && us.ApellidoMaterno.Contains(pBuscar.Buscar)) ||
                                        (us.RazonSocial != null && us.RazonSocial.Contains(pBuscar.Buscar))
                                    ).ToListAsync();
                }

                if (consulta == null || consulta.Count == 0)
                {
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<List<UsuarioDTO>>(consulta);

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "BuscarUsuario(BuscarRequest) => Usuario::{usuario}", pBuscar.Buscar);
            }

            return resultado;
        }
        public async Task<Respuesta<UsuarioDTO>> BuscarUsuario(int pUsuario)
        {
            Respuesta<UsuarioDTO> resultado = new();

            try
            {
                //var usuario = await DBContext.Usuarios.Where(us => us.Id == pUsuario).FirstOrDefaultAsync();
                var usuario = await DBContext.Usuarios
                    .Include(x => x.IdProgramaNavigation)
                        .ThenInclude(x => x.IdCanalNavigation)
                    .Include(x => x.IdRutaNavigation)
                    .Include(x => x.IdCediNavigation)
                        .ThenInclude(x => x.IdZonaNavigation)
                            .ThenInclude(x => x.IdEmbotelladoraNavigation)
                    .Include(x => x.IdSupervisorNavigation)
                    .Include(x => x.IdColoniaNavigation)
                    .Where(us => us.Id == pUsuario).FirstOrDefaultAsync();


                if (usuario == null)
                {

                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                UsuarioDTO _usu = mapper.Map<UsuarioDTO>(usuario);

                _usu.Ruta = usuario.IdRutaNavigation != null ? usuario.IdRutaNavigation.Nombre : string.Empty;
                _usu.Cedi = usuario.IdCediNavigation.Nombre != null ? usuario.IdCediNavigation.Nombre : string.Empty;
                _usu.Supervisor = usuario.IdSupervisorNavigation.Nombre != null ? usuario.IdSupervisorNavigation.Nombre : string.Empty;
                _usu.Embotelladora = usuario.IdCediNavigation.IdZonaNavigation.IdEmbotelladoraNavigation != null ? usuario.IdCediNavigation.IdZonaNavigation.IdEmbotelladoraNavigation.Nombre : string.Empty;

                resultado.Data = _usu;

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "BuscarUsuario(int32) => IdUsuario::{usuario}", pUsuario);
            }

            return resultado;
        }

        public async Task<Respuesta<bool>> Actualizar(int pIdUsuario, string pCelular, string pEmail)
        {
            Respuesta<bool> resultado = new();

            try
            {
                //var usuario = await DBContext.Usuarios.Where(us => us.Id == pUsuario).FirstOrDefaultAsync();
                var usuario = await DBContext.Usuarios.FindAsync(pIdUsuario);


                if (usuario == null)
                {
                    resultado.Data = false;
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                usuario.Email = pEmail;
                usuario.Celular = pCelular;

                await DBContext.SaveChangesAsync();

                resultado.Codigo = (int)CodigoDeError.OK;
                resultado.Mensaje = "Datos actualizados";
                resultado.Exitoso = true;
                resultado.Data = true;

            }
            catch (Exception ex)
            {
                resultado.Data = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Actualizar(int32, string, string) => IdUsuario::{usuario}", pIdUsuario);
            }

            return resultado;
        }

        public async Task<Respuesta<UsuarioDTO>> Actualizar(UsuarioRequest pUsuario, int pIdOperador)
        {
            Respuesta<UsuarioDTO> resultado = new();

            try
            {
                //var usuario = await DBContext.Usuarios.Where(us => us.Id == pUsuario).FirstOrDefaultAsync();
                var usuario = await DBContext.Usuarios.FindAsync(pUsuario.Id);


                if (usuario == null)
                {
                    resultado.Data = null;
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                usuario.Id = pUsuario.Id;
                usuario.Nombre = pUsuario.Nombre;
                usuario.ApellidoPaterno = pUsuario.ApellidoPaterno;
                usuario.ApellidoMaterno = pUsuario.ApellidoMaterno;
                usuario.FechaNacimiento = pUsuario.FechaNacimiento;
                usuario.Sexo = pUsuario.Sexo;
                usuario.Celular = pUsuario.Celular;
                usuario.Email = pUsuario.Email;
                usuario.Calle = pUsuario.Calle;
                usuario.NumeroExterior = pUsuario.NumeroExterior;
                usuario.NumeroInterior = pUsuario.NumeroInterior;
                usuario.IdColonia = pUsuario.IdColonia;
                usuario.CalleInicio = pUsuario.CalleInicio;
                usuario.CalleFin = pUsuario.CalleFin;
                usuario.Referencias = pUsuario.Referencias;
                usuario.Telefono = pUsuario.Telefono;
                usuario.IdOperadorMod = pIdOperador;
                usuario.FechaMod = DateTime.Now;


                await DBContext.SaveChangesAsync();

                UsuarioDTO _usuario = mapper.Map<UsuarioDTO>(DBContext.Usuarios.Where(u => u.Id == usuario.Id).FirstOrDefault());

                resultado.Codigo = (int)CodigoDeError.OK;
                resultado.Mensaje = "Datos actualizados";
                resultado.Exitoso = true;
                resultado.Data = _usuario;

            }
            catch (Exception ex)
            {
                resultado.Data = null;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Actualizar(UsuarioRequest, int32) => IdUsuario::{usuario}", pUsuario.Id);
            }

            return resultado;
        }
        #endregion

        public Respuesta<UsuarioDTO> ConsultarUsuario(int idUsuario)
        {
            Respuesta<UsuarioDTO> resultado = new();

            try
            {
                if (!DBContext.Usuarios.Any(u => u.Id == idUsuario))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == idUsuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(u => u.Id == idUsuario && u.Bloqueado == false))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioBloqueado;
                    resultado.Mensaje = CodigoDeError.UsuarioBloqueado.GetDescription();

                    return resultado;
                }

                var usuario = DBContext.Usuarios
                    .Include(x => x.IdProgramaNavigation)
                        .ThenInclude(x => x.IdCanalNavigation)
                    .Include(x => x.IdRutaNavigation)
                    .Include(x => x.IdCediNavigation)
                        .ThenInclude(x => x.IdZonaNavigation)
                            .ThenInclude(x => x.IdEmbotelladoraNavigation)
                    .Include(x => x.IdSupervisorNavigation)
                    .Include(x => x.IdColoniaNavigation)
                    .FirstOrDefault(u => u.Id == idUsuario);

                if (usuario != null)
                {
                    resultado.Data = mapper.Map<UsuarioDTO>(usuario);
                }
                else
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                }
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "ConsultarUsuario(int32) => IdUsuario::{usuario}", idUsuario);
            }

            return resultado;
        }

        #region Login
        public async Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginRequest pCredenciales, int idOrigen)
        {
            Respuesta<UsuarioDTO> resultado = new()
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

                var hash = new Hash(pCredenciales.Password);
                var password = hash.Sha512();

                var validaUsuario = await DBContext.Usuarios.FirstOrDefaultAsync(x => x.Cuc == pCredenciales.Usuario);

                if (validaUsuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }



                if (!(validaUsuario.Password == password))
                {
                    validaUsuario.IntentosSesion++;
                    await DBContext.SaveChangesAsync();

                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (!await DBContext.Usuarios.AnyAsync(u => u.Cuc == pCredenciales.Usuario && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                var usuario = await DBContext.Usuarios
                    .Include(x => x.IdProgramaNavigation)
                        .ThenInclude(x => x.IdCanalNavigation)
                    .Include(x => x.IdRutaNavigation)
                    .Include(x => x.IdCediNavigation)
                        .ThenInclude(x => x.IdZonaNavigation)
                            .ThenInclude(x => x.IdEmbotelladoraNavigation)
                    .Include(x => x.IdSupervisorNavigation)
                    .Include(x => x.IdColoniaNavigation)
                    .FirstAsync(x => x.Id == validaUsuario.Id);

                if (usuario != null)
                {
                    if (usuario.Bloqueado == false)
                    {
                        BitacoraDeUsuario bdu = new()
                        {
                            IdUsuario = usuario.Id,
                            IdTipoDeOperacion = (int)TipoOperacion.InicioSesion,
                            FechaReg = DateTime.Now,
                            Notas = TipoOperacion.InicioSesion.GetDescription(),
                            IdOrigen = idOrigen
                        };

                        usuario.Sesion = Guid.NewGuid().ToString();

                        usuario.BitacoraDeUsuarios.Add(bdu);

                        usuario = Update(usuario);

                        resultado.Data = mapper.Map<UsuarioDTO>(usuario);

                        resultado.Data.FechaVenta = await DBContext.Ventas
                            .Where(x => x.IdUsuario == usuario.Id)
                            .OrderByDescending(x => x.Id)
                            .Select(x => x.FechaVenta).FirstOrDefaultAsync();
                    }
                    else
                    {
                        resultado.Codigo = (int)CodigoDeError.UserNoactiveOrLocked;
                        resultado.Mensaje = CodigoDeError.UserNoactiveOrLocked.GetDescription();
                        resultado.Exitoso = false;
                    }
                }
                else
                {
                    throw new InvalidOperationException(CodigoDeError.ConexionFallida.GetDescription());
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ValidaAcceso(LoginRequest, int32) => Cuc::{usuario}", pCredenciales.Usuario);
            }

            return resultado;
        }

        public async Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginApp pCredenciales, int idOrigen)
        {
            Respuesta<UsuarioDTO> resultado = new()
            {
                IdTransaccion = Guid.NewGuid()
            };

            try
            {
                if (pCredenciales.Sesion != null)
                {
                    if (!await DBContext.Usuarios.AnyAsync(u => u.Sesion == pCredenciales.Sesion.ToString()))
                    {
                        resultado.Exitoso = false;
                        resultado.Codigo = (int)CodigoDeError.SesionCaducada;
                        resultado.Mensaje = CodigoDeError.SesionCaducada.GetDescription();

                        return resultado;
                    }

                    goto iniciarSesion;
                }

                var valida = Extensiones.ValidateRequest(pCredenciales);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

            iniciarSesion:

                var usuario = pCredenciales.Sesion != null
                    ? await DBContext.Usuarios
                        .Include(x => x.IdProgramaNavigation)
                            .ThenInclude(x => x.IdCanalNavigation)
                        .Include(x => x.IdRutaNavigation)
                        .Include(x => x.IdCediNavigation)
                            .ThenInclude(x => x.IdZonaNavigation)
                                .ThenInclude(x => x.IdEmbotelladoraNavigation)
                        .Include(x => x.IdSupervisorNavigation)
                        .Include(x => x.IdColoniaNavigation)
                        .FirstOrDefaultAsync(u => u.Sesion == pCredenciales.Sesion.ToString()) :
                    await DBContext.Usuarios
                        .Include(x => x.IdProgramaNavigation)
                            .ThenInclude(x => x.IdCanalNavigation)
                        .Include(x => x.IdRutaNavigation)
                        .Include(x => x.IdCediNavigation)
                            .ThenInclude(x => x.IdZonaNavigation)
                                .ThenInclude(x => x.IdEmbotelladoraNavigation)
                        .Include(x => x.IdSupervisorNavigation)
                        .Include(x => x.IdColoniaNavigation)
                        .FirstOrDefaultAsync(u => u.Cuc == pCredenciales.Usuario);

                if (usuario == null || usuario.Password == null)
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.Bloqueado)
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioBloqueado;
                    resultado.Mensaje = CodigoDeError.UsuarioBloqueado.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                if (usuario.IntentosSesion >= _config.IntentosSesion)
                {
                    usuario.Bloqueado = true;
                    await DBContext.SaveChangesAsync();

                    resultado.Codigo = (int)CodigoDeError.IntentoDeSesion;
                    resultado.Mensaje = CodigoDeError.IntentoDeSesion.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var hash = new Hash(pCredenciales.Password);
                var password = hash.Sha512();

                if (pCredenciales.Sesion != null)
                {
                    password = usuario.Password;
                }

                var pass1 = Encoding.UTF8.GetString(usuario.Password);
                var pass2 = Encoding.UTF8.GetString(password);

                if (!(Encoding.UTF8.GetString(usuario.Password) == Encoding.UTF8.GetString(password)))
                {
                    usuario.IntentosSesion++;
                    await DBContext.SaveChangesAsync();

                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }
                else
                {
                    if (usuario.IntentosSesion > 0)
                    {
                        usuario.IntentosSesion = 0;
                        await DBContext.SaveChangesAsync();
                    }
                }

                if (!(usuario.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                if (!string.IsNullOrEmpty(pCredenciales.TokenDispositivo))
                {
                    usuario.TokenMovil = pCredenciales.TokenDispositivo;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdUsuario = usuario.Id,
                    IdTipoDeOperacion = (int)TipoOperacion.InicioSesion,
                    Notas = TipoOperacion.InicioSesion.GetDescription(),
                    IdOrigen = idOrigen,
                    FechaReg = DateTime.Now
                };

                usuario.Sesion = Guid.NewGuid().ToString();

                usuario.BitacoraDeUsuarios.Add(bdu);

                usuario = Update(usuario);

                resultado.Data = mapper.Map<UsuarioDTO>(usuario);

                resultado.Data.FechaVenta = await DBContext.Ventas
                            .Where(x => x.IdUsuario == usuario.Id)
                            .OrderByDescending(x => x.Id)
                            .Select(x => x.FechaVenta).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "ValidaAcceso(LoginApp, int32) => Cuc::{usuario}", pCredenciales.Usuario);
            }

            return resultado;
        }

        public async Task<Respuesta<Empty>> BloquearUsuario(LoginRequest credenciales)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var usuario = await DBContext.Usuarios.FirstOrDefaultAsync(u => u.Cuc == credenciales.Usuario);

                if (usuario != null)
                {
                    BitacoraDeUsuario bdu = new BitacoraDeUsuario
                    {
                        IdTipoDeOperacion = (int)TipoOperacion.BloqueoCuenta,
                        FechaReg = DateTime.Now,
                        Notas = TipoOperacion.BloqueoCuenta.GetDescription()
                    };

                    usuario.Bloqueado = true;

                    usuario.BitacoraDeUsuarios.Add(bdu);

                    Update(usuario);
                }
                else
                {
                    throw new InvalidOperationException(CodigoDeError.ConexionFallida.GetDescription());
                }
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;

                _logger.Error(ex, "BloquearUsuario(LoginRequest) => Cuc::{usuario}", credenciales.Usuario);
            }

            return resultado;
        }

        public async Task<Respuesta<Empty>> RecuperarContrasenia(RestablecerPassRequest datos)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var valida = Extensiones.ValidateRequest(datos);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = false;

                    return resultado;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.RecuperarPassword,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.RecuperarPassword.GetDescription()
                };

                if (!DBContext.Usuarios.Any(u => u.Cuc == datos.Cuc && u.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                Usuario usuario = DBContext.Usuarios.First(u => u.Cuc == datos.Cuc);

                usuario.BitacoraDeUsuarios.Add(bdu);

                resultado.Mensaje = datos.TipoMensajeria switch
                {
                    TipoMensajeria.Email => MensajeApp.EnlaceCorreoEnviado.GetDescription(),
                    TipoMensajeria.Sms => MensajeApp.EnlaceSMSenviado.GetDescription(),
                    _ => throw new Exception("Mensajería desconocida"),
                };

                Update(usuario);

                await appEmail.RecuperarPassword(datos.TipoMensajeria, TipoUsuario.Usuario, usuario.Id);
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "RecuperarContrasenia(RestablecerPassRequest) => Cuc::{usuario}", datos.Cuc);
            }

            return resultado;
        }

        public Respuesta<Empty> CambiarContraseniaByToken(CambiarPasswordRequest datos)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                if (datos.Token == null)
                {
                    resultado.Codigo = (int)CodigoDeError.ErrorLigaRecPass;
                    resultado.Mensaje = CodigoDeError.ErrorLigaRecPass.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                var valida = Extensiones.ValidateRequest(datos);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.CambioContrasenia,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.CambioContrasenia.GetDescription()
                };

                BitacoraDeContrasena bdc = new()
                {
                    Origen = "Web",
                    FechaReg = DateTime.Now
                };

                var bec = _bitacoraEnvioCorreo.ConsultaByToken(datos.Token);

                if (!bec.Exitoso || bec.Data == null)
                {
                    resultado.Codigo = bec.Codigo;
                    resultado.Mensaje = bec.Mensaje;
                    resultado.Exitoso = bec.Exitoso;

                    return resultado;
                }

                var hash = new Hash(datos.Password);

                var password = hash.Sha512();

                Usuario usuario = DBContext.Usuarios.Include(u => u.BitacoraDeContrasenas).First(u => u.Id == bec.Data.IdUsuario);

                if (!_bitacoraDeContrasenas.ValidarUltimasContrasenas(usuario.BitacoraDeContrasenas.ToList(), password))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsedPassword;
                    resultado.Mensaje = CodigoDeError.UsedPassword.GetDescription();

                    return resultado;
                }

                bdc.Password = password;

                usuario.BitacoraDeUsuarios.Add(bdu);
                usuario.BitacoraDeContrasenas.Add(bdc);
                usuario.Password = password;
                usuario.CambiarPass = false;

                Update(usuario);

                _bitacoraEnvioCorreo.ActualizaEstatus(bec.Data.Id, TipoDeEstatus.Inactivo);

                resultado.Mensaje = MensajeApp.PassCambiada.GetDisplayName();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "CambiarContraseniaByToken(CambiarPasswordRequest) => Token::{usuario}", datos.Token?.ToString());
            }

            return resultado;
        }
        #endregion

        public async Task<Respuesta<Empty>> CambiarContraseniaApp(CambiarPasswordRequestApp datos)
        {
            Respuesta<Empty> resultado = new();

            try
            {

                var valida = Extensiones.ValidateRequest(datos);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                BitacoraDeUsuario bdu = new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.CambioContrasenia,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.CambioContrasenia.GetDescription()
                };

                BitacoraDeContrasena bdc = new()
                {
                    Origen = "App",
                    FechaReg = DateTime.Now
                };

                // var bec = _bitacoraEnvioCorreo.ConsultaByToken(datos.Token);

                // if (!bec.Exitoso || bec.Data == null)
                // {
                //     resultado.Codigo = bec.Codigo;
                //     resultado.Mensaje = bec.Mensaje;
                //     resultado.Exitoso = bec.Exitoso;

                //     return resultado;
                // }

                var hash = new Hash(datos.Password);

                var password = hash.Sha512();

                Usuario usuario = DBContext.Usuarios.Include(u => u.BitacoraDeContrasenas).First(u => u.Id == datos.IdUsuario);

                if (!_bitacoraDeContrasenas.ValidarUltimasContrasenas(usuario.BitacoraDeContrasenas.ToList(), password))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsedPassword;
                    resultado.Mensaje = CodigoDeError.UsedPassword.GetDescription();

                    return resultado;
                }

                bdc.Password = password;

                usuario.BitacoraDeUsuarios.Add(bdu);
                usuario.BitacoraDeContrasenas.Add(bdc);
                usuario.Password = password;
                usuario.CambiarPass = false;

                Update(usuario);

                //_bitacoraEnvioCorreo.ActualizaEstatus(bec.Data.Id, TipoDeEstatus.Inactivo);

                resultado.Mensaje = MensajeApp.PassCambiada.GetDisplayName();
            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "CambiarContraseniaApp(CambiarPasswordRequestApp) => IdUsuario::{usuario}", datos.IdUsuario);
            }

            return resultado;
        }

        #region MiCuenta
        public Respuesta<MiCuentaDTO> MiCuenta(RequestByIdUsuario data)
        {
            Respuesta<MiCuentaDTO> resultado = new Respuesta<MiCuentaDTO>();

            try
            {
                var valida = Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var usuario = ConsultarUsuario(data.IdUsuario);

                if (!usuario.Exitoso)
                {
                    resultado.Exitoso = usuario.Exitoso;
                    resultado.Codigo = usuario.Codigo;
                    resultado.Mensaje = usuario.Mensaje;

                    return resultado;
                }

                MiCuentaDTO miCuenta = new MiCuentaDTO();

                // var obtenerNegocio = _negocio.ObtenerNegocioPorId(usuario.Data.IdNegocio);

                // if (obtenerNegocio.Data != null)
                // {
                //     miCuenta.Negocio = obtenerNegocio.Data;
                // }

                miCuenta.Propietario = usuario.Data;


                resultado.Data = miCuenta;
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "MiCuenta(RequestByIdUsuario) => IdUsuario::{usuario}", data.IdUsuario);
            }

            return resultado;
        }


        //El método de MiCuenta será sustituido por Cuenta -- Esperar confirmación de Apps
        public Respuesta<CuentaDTO> Cuenta(RequestByIdUsuario data)
        {
            Respuesta<CuentaDTO> resultado = new Respuesta<CuentaDTO>();

            try
            {
                var valida = Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var usuarioResult = ConsultarUsuario(data.IdUsuario);

                if (!usuarioResult.Exitoso)
                {
                    resultado.Exitoso = usuarioResult.Exitoso;
                    resultado.Codigo = usuarioResult.Codigo;
                    resultado.Mensaje = usuarioResult.Mensaje;

                    return resultado;
                }

                resultado.Data = new CuentaDTO();

                var usuario = DBContext.Usuarios.Find(data.IdUsuario);

                // var negocio = DBContext.Negocios
                //                         .Include(n => n.IdEmbotelladoraNavigation)
                //                         .Include(n => n.IdCediNavigation)
                //                         .Include(n => n.IdSupervisorNavigation)
                //                         .FirstOrDefault(n => n.Cuc == usuarioResult.Data.Cuc);

                // if (negocio == null)
                // {
                //     resultado.Exitoso = false;
                //     resultado.Codigo = (int)CodigoDeError.SinDatos;
                //     resultado.Mensaje = CodigoDeError.SinDatos.GetDescription() + ": Negocio no encontrado";

                //     return resultado;
                // }

                var cuentaDTO = new CuentaDTO();

                //mapper.Map(negocio, cuentaDTO);
                mapper.Map(usuario, cuentaDTO);

                resultado.Data = cuentaDTO;
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "Cuenta(RequestByIdUsuario) => IdUsuario::{usuario}", data.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<Empty> ActualizarContactos(ActualizarConctactosDTO data)
        {
            Respuesta<Empty> resultado = new Respuesta<Empty>();

            try
            {
                var valida = Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                var usuario = ConsultarUsuario(data.IdUsuario);

                if (!usuario.Exitoso)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();

                    return resultado;
                }

                if (data.Email == usuario.Data.Email && data.Celular == usuario.Data.Celular)
                {
                    resultado.Exitoso = true;
                    resultado.Codigo = (int)CodigoDeError.SinCambios;
                    resultado.Mensaje = CodigoDeError.SinCambios.GetDescription();

                    return resultado;
                }

                if (DBContext.Usuarios.Any(u => u.Email == data.Email & u.Id != data.IdUsuario))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.CorreoUsado;
                    resultado.Mensaje = CodigoDeError.CorreoUsado.GetDescription();

                    return resultado;
                }

                if (DBContext.Usuarios.Any(u => u.Celular == data.Celular & u.Id != data.IdUsuario))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.CelularUsado;
                    resultado.Mensaje = CodigoDeError.CelularUsado.GetDescription();

                    return resultado;
                }

                BitacoraDeUsuario btu = new()
                {
                    IdTipoDeOperacion = (int)TipoOperacion.ActualizarDatos,
                    FechaReg = DateTime.Now,
                    Notas = TipoOperacion.ActualizarDatos.GetDescription() + ": Usuarios"
                };

                var usuarioModel = DBContext.Usuarios.First(u => u.Id == data.IdUsuario);

                usuarioModel.Email = data.Email;
                usuarioModel.Celular = data.Celular;
                usuarioModel.BitacoraDeUsuarios.Add(btu);

                Update(usuarioModel);

                resultado.Codigo = (int)CodigoDeError.OK;
                resultado.Mensaje = "¡Información actualizada correctamente! Los cambios han sido guardados exitosamente.";

            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "ActualizarContactos(ActualizarConctactosDTO) => IdUsuario::{usuario}", data.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<bool> CambiarContrasenia(CambiarPasswordDTO data)
        {
            Respuesta<bool> resultado = new Respuesta<bool>();
            try
            {
                var valida = Extensiones.ValidateRequest(data);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

                if (!DBContext.Usuarios.Any(x => x.Id == data.IdUsuario && x.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                byte[] passworactual;
                byte[] passwornuevo;

                var hashactual = new Hash(data.PasswordActual);

                passworactual = hashactual.Sha512();

                var usuario = DBContext.Usuarios.Include(u => u.BitacoraDeContrasenas).FirstOrDefault(x => x.Id == data.IdUsuario && x.IdEstatus == (int)TipoDeEstatus.Activo);

                if (usuario != null)
                {
                    if (Encoding.UTF8.GetString(usuario.Password) != Encoding.UTF8.GetString(passworactual))
                    {
                        resultado.Codigo = (int)CodigoDeError.ContraseniaActualNoCoincide;
                        resultado.Mensaje = CodigoDeError.ContraseniaActualNoCoincide.GetDescription();
                        resultado.Exitoso = false;

                        return resultado;
                    }

                    var hash = new Hash(data.Password);
                    passwornuevo = hash.Sha512();

                    if (!_bitacoraDeContrasenas.ValidarUltimasContrasenas(usuario.BitacoraDeContrasenas.ToList(), passwornuevo))
                    {
                        resultado.Exitoso = false;
                        resultado.Codigo = (int)CodigoDeError.UsedPassword;
                        resultado.Mensaje = CodigoDeError.UsedPassword.GetDescription();

                        return resultado;
                    }

                    usuario.Password = passwornuevo;

                    BitacoraDeUsuario bdu = new BitacoraDeUsuario
                    {
                        IdTipoDeOperacion = (int)TipoOperacion.CambioContrasenia,
                        FechaReg = DateTime.Now,
                        Notas = TipoOperacion.CambioContrasenia.GetDescription()
                    };

                    usuario.BitacoraDeUsuarios.Add(bdu);

                    usuario.CambiarPass = false;

                    Update(usuario);
                }
                else
                {
                    new Exception("Conexión fallida");
                }
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;

                _logger.Error(ex, "CambiarContrasenia(CambiarPasswordDTO) => IdUsuario::{usuario}", data.IdUsuario);
            }

            return resultado;
        }

        public Respuesta<Empty> Desbloquear(int idUsuario, int idOperador, string? notas, int idOrigen)
        {
            Respuesta<Empty> resultado = new();

            try
            {
                var data = new
                {
                    IdUsuario = idUsuario,
                    IdOperador = idOperador,
                    IdOrigen = idOrigen,
                    Notas = notas,
                };

                var parametros = Extensiones.CrearSqlParametrosDelModelo(data);

                var exec = DBContext.Database.ExecuteSqlRaw("EXEC sp_Usuarios_DesbloquearCuenta @IdUsuario, @IdOperador, @IdOrigen, @Notas", parametros);

            }
            catch (Exception ex)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;

                _logger.Error(ex, "Desbloquear(int32, int32, string?, int32) => IdUsuario::{usuario}", idUsuario);
            }

            return resultado;
        }
        #endregion

        #region Validaciones
        public Respuesta<Empty> FuerzaDeVentaActivo(FuerzaDeVentaDTO? datos)
        {
            Respuesta<Empty> resultado = new Respuesta<Empty>();

            try
            {
                if (datos == null)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.SinDatos;
                    resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();

                    goto final;
                }

                resultado.Mensaje = "Acción no permitida para fuerza de venta";
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();

                _logger.Error(ex, "FuerzaDeVentaActivo(FuerzaDeVentaDTO?) => IdFDV::{usuario}", datos?.Id);
            }
        final:
            return resultado;
        }
        #endregion
        #region Metodos Privados
        //Actualización de usuario
        private Usuario Update(Usuario usuario)
        {
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var transaction = DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        DBContext.Update(usuario);
                        DBContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return usuario;
        }
        #endregion
    }
}
