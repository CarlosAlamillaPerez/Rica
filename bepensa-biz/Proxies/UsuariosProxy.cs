using AutoMapper;
using bepensa_biz.Extensions;
using bepensa_biz.Interfaces;
using bepensa_biz.Proxies;
using bepensa_biz.Security;
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
using System.Text;

namespace bepensa_biz.Proxies
{
    public class UsuariosProxy : ProxyBase, IUsuario
    {
        private readonly IMapper mapper;

        private readonly IBitacoraDeContrasenas _bitacoraDeContrasenas;
        private readonly IEnviarCorreo _enviarCorreo;
        private readonly IBitacoraEnvioCorreo _bitacoraEnvioCorreo;
        private readonly IAppEmail appEmail;

        public UsuariosProxy(BepensaContext context, IMapper mapper,
                                IEnviarCorreo enviarCorreo, IBitacoraDeContrasenas bitacoraDeContrasenas,
                                IBitacoraEnvioCorreo bitacoraEnvioCorreo, IAppEmail appEmail)
        {
            DBContext = context;
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
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
        public async Task<Respuesta<UsuarioDTO>> BuscarUsuario(int pUsuario)
        {
            Respuesta<UsuarioDTO> resultado = new();

            try
            {
                var usuario = await DBContext.Usuarios.Where(us => us.Id == pUsuario).FirstOrDefaultAsync();


                if (usuario == null)
                {
                    resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                    resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                    resultado.Exitoso = false;

                    return resultado;
                }

                resultado.Data = mapper.Map<UsuarioDTO>(usuario);

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
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

                var usuario = DBContext.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

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
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
            }

            return resultado;
        }

        #region Login
        public async Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginRequest pCredenciales)
        {
            Respuesta<UsuarioDTO> resultado = new();

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

                if (!await DBContext.Usuarios.AnyAsync(u => u.Cuc == pCredenciales.Usuario && u.Password == password))
                {
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
                                    .Include(x => x.IdSupervisorNavigation)
                                    .Include(x => x.IdColoniaNavigation)
                                    .Where(u => u.Cuc == pCredenciales.Usuario)
                                    .FirstOrDefaultAsync();

                if (usuario != null)
                {
                    if (usuario.Bloqueado == false)
                    {
                        BitacoraDeUsuario bdu = new()
                        {
                            IdUsuario = usuario.Id,
                            IdTipoDeOperacion = (int)TipoDeOperacion.InicioSesion,
                            FechaReg = DateTime.Now,
                            Notas = TipoDeOperacion.InicioSesion.GetDescription()
                        };

                        usuario.Sesion = Guid.NewGuid().ToString();

                        usuario.BitacoraDeUsuarios.Add(bdu);

                        usuario = Update(usuario);

                        resultado.Data = mapper.Map<UsuarioDTO>(usuario);
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
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<UsuarioDTO>> ValidaAcceso(LoginApp credenciales)
        {
            Respuesta<UsuarioDTO> resultado = new Respuesta<UsuarioDTO>();

            BitacoraDeUsuario bdu = new BitacoraDeUsuario { FechaReg = DateTime.Now };

            byte[] password;

            try
            {
                if (credenciales.Sesion != null)
                {
                    goto reconexion;
                }

                var valida = Extensiones.ValidateRequest(credenciales);

                if (!valida.Exitoso)
                {
                    resultado.Codigo = valida.Codigo;
                    resultado.Mensaje = valida.Mensaje;
                    resultado.Exitoso = valida.Exitoso;

                    return resultado;
                }

            reconexion:

                var usuario = credenciales.Sesion != null ?
                    await DBContext.Usuarios.Include(x => x.IdProgramaNavigation).ThenInclude(x => x.IdCanalNavigation).FirstOrDefaultAsync(u => u.Sesion == credenciales.Sesion.ToString()) :
                    await DBContext.Usuarios.Include(x => x.IdProgramaNavigation).ThenInclude(x => x.IdCanalNavigation).FirstOrDefaultAsync(u => u.Cuc == credenciales.Usuario);

                if (usuario == null || usuario.Password == null)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();

                    return resultado;
                }

                if (credenciales.Sesion != null)
                {
                    password = usuario.Password;
                }
                else
                {
                    var hash = new Hash(credenciales.Password);
                    password = hash.Sha512();
                }

                if (!(Encoding.UTF8.GetString(usuario.Password) == Encoding.UTF8.GetString(password)))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
                    resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();

                    return resultado;
                }

                if (usuario.Bloqueado)
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioBloqueado;
                    resultado.Mensaje = CodigoDeError.UsuarioBloqueado.GetDescription();

                    return resultado;
                }

                if (!(usuario.IdEstatus == (int)TipoDeEstatus.Activo))
                {
                    resultado.Exitoso = false;
                    resultado.Codigo = (int)CodigoDeError.UsuarioInactivo;
                    resultado.Mensaje = CodigoDeError.UsuarioInactivo.GetDescription();

                    return resultado;
                }

                if (!string.IsNullOrEmpty(credenciales.TokenDispositivo))
                {
                    usuario.TokenMovil = credenciales.TokenDispositivo;
                }

                bdu.IdUsuario = usuario.Id;
                bdu.IdTipoDeOperacion = (int)TipoDeOperacion.InicioSesion;
                bdu.Notas = TipoDeOperacion.InicioSesion.GetDescription();

                usuario.Sesion = Guid.NewGuid().ToString();

                usuario.BitacoraDeUsuarios.Add(bdu);

                usuario = Update(usuario);

                resultado.Data = mapper.Map<UsuarioDTO>(usuario);

                // resultado.Data.Saldo = await DBContext.Saldos
                //                             .Where(s => s.IdUsuario == usuario.Id)
                //                             .OrderByDescending(s => s.Id)
                //                             .Select(s => s.Saldo1)
                //                             .FirstOrDefaultAsync();

                // resultado.Data.UltimaActualizacion = await DBContext.EstadosDeCuenta
                //                                             .Where(edc =>
                //                                                 edc.IdNegocio == usuario.IdNegocio &&
                //                                                 edc.IdConceptoDeAcumulacion == (int)TipoDeConceptoDeAcumulacion.MecanicaDeAcumulacion
                //                                             )
                //                                             .OrderByDescending(edc => edc.FechaReg)
                //                                             .Select(edc => edc.FechaReg)
                //                                             .FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public async Task<Respuesta<Empty>> BloquearUsuario(LoginRequest credenciales)
        {
            Respuesta<Empty> resultado = new Respuesta<Empty>();

            try
            {
                var usuario = await DBContext.Usuarios.FirstOrDefaultAsync(u => u.Cuc == credenciales.Usuario);

                if (usuario != null)
                {
                    BitacoraDeUsuario bdu = new BitacoraDeUsuario
                    {
                        IdTipoDeOperacion = (int)TipoDeOperacion.BloqueoCuenta,
                        FechaReg = DateTime.Now,
                        Notas = TipoDeOperacion.BloqueoCuenta.GetDescription()
                    };

                    usuario.Bloqueado = true;

                    usuario.IdEstatus = (int)TipoDeEstatus.Bloqueada;

                    usuario.BitacoraDeUsuarios.Add(bdu);

                    Update(usuario);
                }
                else
                {
                    throw new InvalidOperationException(CodigoDeError.ConexionFallida.GetDescription());
                }
            }
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Data = null;
                resultado.Exitoso = false;
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
                    IdTipoDeOperacion = (int)TipoDeOperacion.RecuperarPassword,
                    FechaReg = DateTime.Now,
                    Notas = TipoDeOperacion.RecuperarPassword.GetDescription()
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
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
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
                    IdTipoDeOperacion = (int)TipoDeOperacion.CambioContrasenia,
                    FechaReg = DateTime.Now,
                    Notas = TipoDeOperacion.CambioContrasenia.GetDescription()
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
            catch (Exception)
            {
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
                resultado.Exitoso = false;
            }

            return resultado;
        }
        #endregion

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
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
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
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
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

                BitacoraDeUsuario btu = new BitacoraDeUsuario
                {
                    IdTipoDeOperacion = (int)TipoDeOperacion.UpdateData,
                    FechaReg = DateTime.Now,
                    Notas = TipoDeOperacion.UpdateData.GetDescription() + ": Usuarios"
                };

                var usuarioModel = DBContext.Usuarios.FirstOrDefault(u => u.Id == data.IdUsuario);

                usuarioModel.Email = data.Email;
                usuarioModel.Celular = data.Celular;
                usuarioModel.BitacoraDeUsuarios.Add(btu);

                Update(usuarioModel);

                resultado.Codigo = (int)CodigoDeError.OK;
                resultado.Mensaje = "¡Información actualizada correctamente! Los cambios han sido guardados exitosamente.";

            }
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
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
                        IdTipoDeOperacion = (int)TipoDeOperacion.CambioContrasenia,
                        FechaReg = DateTime.Now,
                        Notas = TipoDeOperacion.CambioContrasenia.GetDescription()
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
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Mensaje = CodigoDeError.UsuarioInvalido.GetDescription();
                resultado.Codigo = (int)CodigoDeError.UsuarioInvalido;
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
            catch (Exception)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.Excepcion;
                resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
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

        private (bool Exitoso, Usuario Model) Create(Usuario usuario)
        {
            bool success = false;
            var strategy = DBContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var transaction = DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        DBContext.Add(usuario);
                        int rowAffected = DBContext.SaveChanges();
                        if (rowAffected > 0) { success = true; }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return (success, usuario);
        }
        #endregion
    }
}
