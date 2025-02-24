using AutoMapper;
using bepensa_data.data;
using bepensa_models.DTO;
using bepensa_models.Enums;
using bepensa_models.General;
using bepensa_biz.Extensions;
using Microsoft.EntityFrameworkCore;
using bepensa_models.DataModels;
using bepensa_biz.Interfaces;
using bepensa_biz.Security;
using bepensa_data.models;

namespace bepensa_biz.Proxies;

public class InscripcionesProxy : ProxyBase, IInscripcion
{
    private readonly IMapper mapper;

    public InscripcionesProxy(BepensaContext context, IMapper mapper)
    {
        DBContext = context;
        this.mapper = mapper;
    }

    public async Task<Respuesta<InscripcionDTO>> ConsultarUsuario(LoginInscripcionRequest pInscripcion)
    {
        Respuesta<InscripcionDTO> resultado = new();

        try
        {
            var valida = Extensiones.ValidateRequest(pInscripcion);

            if (!valida.Exitoso)
            {
                resultado.Codigo = valida.Codigo;
                resultado.Mensaje = valida.Mensaje;
                resultado.Exitoso = false;

                return resultado;
            }

            bool cucValido = long.TryParse(pInscripcion.Cuc, out long cucVerificado);

            if (!cucValido)
            {
                resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            var fechaActual = DateOnly.FromDateTime(DateTime.Now);

            var inscripcionActiva = await DBContext.Parametros
                                            .AnyAsync(p => 
                                                p.Tag.Equals(TipoParametro.FechaInscripcion.GetDisplayName())
                                                && (p.FechaInicio != null && p.FechaInicio <= fechaActual)
                                                && (p.FechaFin == null || p.FechaFin >= fechaActual));

            if (!inscripcionActiva)
            {
                resultado.Codigo = (int)CodigoDeError.InscripcionFinalizada;
                resultado.Mensaje = CodigoDeError.InscripcionFinalizada.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            var usuario = await DBContext.Usuarios.Where(n => n.Cuc == cucVerificado.ToString()).FirstOrDefaultAsync();

            if (usuario == null)
            {
                resultado.Codigo = (int)CodigoDeError.NoExisteUsuario;
                resultado.Mensaje = CodigoDeError.NoExisteUsuario.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            resultado.Data = mapper.Map<InscripcionDTO>(usuario);

        }
        catch (Exception)
        {
            resultado.Exitoso = false;
            resultado.Codigo = (int)CodigoDeError.Excepcion;
            resultado.Mensaje = CodigoDeError.Excepcion.GetDescription();
        }

        return resultado;
    }

    #region Validaciones específicas
    public Respuesta<Empty> ValidarEmail(string email)
    {
        Respuesta<Empty> resultado = new();
        try
        {
            var verificarEmail = DBContext.Usuarios.Any(u => u.Email != null && u.Email == email);

            if (verificarEmail)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.EmailUsado;
                resultado.Mensaje = CodigoDeError.EmailUsado.GetDescription();
            }
            else
            {
                resultado.Mensaje = "El correo electrónico no está registrado";
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

    public Respuesta<Empty> ValidarCelular(string pCelular)
    {
        Respuesta<Empty> resultado = new();
        try
        {
            var existe = DBContext.Usuarios.Any(u => u.Celular != null && u.Celular == pCelular);

            if (existe)
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.CelularUsado;
                resultado.Mensaje = CodigoDeError.CelularUsado.GetDescription();
            }
            else
            {
                resultado.Mensaje = "El celular no está registrado";
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

    public Respuesta<Empty> ValidarTelefono(string pTelefono)
    {
        Respuesta<Empty> resultado = new();
        try
        {
            var existe = DBContext.Usuarios.Any(u => u.Telefono != null && u.Telefono == pTelefono);

            if (existe)
            {
                resultado.Codigo = (int)CodigoDeError.TelefonoUsado;
                resultado.Mensaje = CodigoDeError.TelefonoUsado.GetDescription();
                resultado.Exitoso = false;
            }
            else
            {
                resultado.Mensaje = "El teléfono no está registrado";
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
    #endregion
    public async Task<Respuesta<Empty>> Registrar(InscripcionRequest pInscripcion)
    {
        Respuesta<Empty> resultado = new();

        try
        {
            var valida = Extensiones.ValidateRequest(pInscripcion);

            if (!valida.Exitoso)
            {
                resultado.Codigo = valida.Codigo;
                resultado.Exitoso = false;
                resultado.Mensaje = valida.Mensaje;

                return resultado;
            }

            if (pInscripcion.Celular == null && pInscripcion.Email == null)
            {
                resultado.Codigo = (int)CodigoDeError.EmailInvalido;
                resultado.Mensaje = "Ingresa un celular o correo electrónico válido";
                resultado.Exitoso = false;

                return resultado;
            }

            if (await DBContext.Usuarios.AnyAsync(n => n.Cuc == pInscripcion.Cuc && n.Inscripcion == true))
            {
                resultado.Codigo = (int)CodigoDeError.UsuarioRegistrado;
                resultado.Mensaje = CodigoDeError.UsuarioRegistrado.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            //if(pInscripcion.Email != null)
            //{
            //    var validaEmail = ValidarEmail(pInscripcion.Email);

            //    if (!validaEmail.Exitoso)
            //    {
            //        resultado.Codigo = validaEmail.Codigo;
            //        resultado.Mensaje = validaEmail.Mensaje;
            //        resultado.Exitoso = false;

            //        return resultado;
            //    }
            //}

            //if (pInscripcion.Celular != null)
            //{
            //    var validaCelular = ValidarCelular(pInscripcion.Celular);

            //    if (!validaCelular.Exitoso)
            //    {
            //        resultado.Codigo = validaCelular.Codigo;
            //        resultado.Mensaje = validaCelular.Mensaje;
            //        resultado.Exitoso = false;

            //        return resultado;
            //    }
            //}

            //if (pInscripcion.Telefono != null)
            //{
            //    var validaTel = ValidarTelefono(pInscripcion.Telefono);

            //    if (!validaTel.Exitoso)
            //    {
            //        resultado.Codigo = validaTel.Codigo;
            //        resultado.Mensaje = validaTel.Mensaje;
            //        resultado.Exitoso = false;

            //        return resultado;
            //    }
            //}

            if (!await DBContext.Usuarios.AnyAsync(n => n.Cuc == pInscripcion.Cuc))
            {
                resultado.Codigo = (int)CodigoDeError.SinDatos;
                resultado.Mensaje = CodigoDeError.SinDatos.GetDescription();
                resultado.Exitoso = false;

                return resultado;
            }

            var parametros = Extensiones.CrearSqlParametrosDelModelo(pInscripcion);

            var registro = await DBContext.Database.ExecuteSqlRawAsync(
                                "EXEC Usuarios_spInscripcion " +
                                "@Cuc, @TipoRuta, " +
                                "@Nombre, @ApellidoPaterno, @ApellidoMaterno, " +
                                "@Celular, @Email, @Sexo, @FechaNacimiento, @IdColonia, @Ciudad, " +
                                "@Calle, @NumeroExterior, @NumeroInterior, @CalleInicio, @CalleFin, @Telefono, @Referencias", parametros);

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
