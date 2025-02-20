using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using bepensa_models.Enums;
using bepensa_models.General;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace bepensa_socio_selecto_biz.Extensions;

public static class Extensiones
{
    /// <summary>
    /// Convierte arreglo de bytes a string
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static string ToHashString(this byte[] buffer)
    {
        var sb = new StringBuilder();
        foreach (var b in buffer)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }


    /// <summary>
    /// Integra Assembly en Startup para inyeccion de dependencias
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterProxies(this IServiceCollection services)
    {
        var refAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(x => x.FullName.Contains("CaminoReal", StringComparison.InvariantCultureIgnoreCase)).ToList();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        for (var i = 0; i < refAssemblies.Count; i++)
        {
            var assemblyName = refAssemblies[i];
            if (!assemblies.Any(x => x.FullName == assemblyName.FullName))
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }
        }

        assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("CaminoReal", StringComparison.InvariantCultureIgnoreCase)).ToList();
        var types = assemblies.SelectMany(o => o.GetTypes()).ToList();
        types.RemoveAll(x => !x.Name.EndsWith("Proxy"));

        for (var i = 0; i < types.Count; i++)
        {
            var type = types[i];
            if (!type.IsInterface && type.IsClass)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    Type interfaz;
                    if (interfaces.Length > 1)
                    {
                        interfaz = interfaces.Where(x => x.Name.EndsWith("Proxy")).FirstOrDefault();
                        if (interfaz == null)
                        {
                            interfaz = interfaces.First();
                        }
                    }
                    else
                    {
                        interfaz = interfaces.First();
                    }
                    services.AddScoped(interfaz, type);
                }
            }
        }

        return services;
    }


    /// <summary>
    /// VAlida modelo segun dataAnnotation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="results"></param>
    /// <returns></returns>
    private static bool ValidateModel<T>(this T source, out List<ValidationResult> results)
    {
        var validator = new ValidationContext(source, null, null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(source, validator, results, true);
    }


    /// <summary>
    /// VAlida el modelo  enviado
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static Respuesta<T> ValidateRequest<T>(this T request)
    {
        Respuesta<T> resultado = new Respuesta<T>();

        if (request == null)
        {
            resultado.Exitoso = false;
            resultado.Codigo = (int)CodigoDeError.ReferenciaNula;
            resultado.Mensaje = CodigoDeError.ReferenciaNula.GetDescription();
            goto fin;
        }

        if (!ValidateModel(request, out List<ValidationResult> result))
        {
            resultado.Exitoso = false;
            resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
            resultado.Mensaje = result[0].ErrorMessage;
        }
        fin:
        return resultado;
    }

    public static Respuesta<List<T>> ValidateRequest<T>(this List<T> requests)
    {
        Respuesta<List<T>> resultado = new Respuesta<List<T>>();

        if (requests == null || !requests.Any())
        {
            resultado.Exitoso = false;
            resultado.Codigo = (int)CodigoDeError.ReferenciaNula;
            resultado.Mensaje = CodigoDeError.ReferenciaNula.GetDescription();

            return resultado;
        }

        foreach (var request in requests)
        {
            if (!ValidateModel(request, out List<ValidationResult> validationResults))
            {
                resultado.Exitoso = false;
                resultado.Codigo = (int)CodigoDeError.PropiedadInvalida;
                resultado.Mensaje = validationResults.First().ErrorMessage; // Retorna el primer error encontrado

                return resultado;
            }
        }

        return resultado;
    }



    /// <summary>
    /// Valida conjunto de checkbox que almenos uno este checado (true)
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    //public static Respuesta<Empty> ValidaCheckbox(List<CheckBoxModel> data)
    //{
    //    Respuesta<Empty> valida = new Respuesta<Empty>();

    //    foreach (var item in data)
    //    {
    //        if (item.Checked == true)
    //        {
    //            valida.Exitoso = true;
    //            valida.Codigo = ErrorCode.Ok;
    //            valida.Mensaje = ErrorCode.Ok.GetDescription();
    //            break;
    //        }
    //        else
    //        {
    //            valida.Exitoso = false;
    //            valida.Codigo = ErrorCode.NoData;
    //            valida.Mensaje = ErrorCode.NoData.GetDescription();
    //        }
    //    }

    //    return valida;
    //}

    /// <summary>
    /// Validacion de la carga de una imagen que sea formato de imagen o pdf
    /// </summary>
    /// <param name="postedFile"></param>
    /// <returns></returns>
    public static bool IsImageOrPdf(this IFormFile postedFile)
    {
        var ImageMinimumBytes = 512;
        //-------------------------------------------
        //  Check the image mime types
        //-------------------------------------------
        if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        //-------------------------------------------
        //  Check the image extension
        //-------------------------------------------
        var postedFileExtension = Path.GetExtension(postedFile.FileName);
        if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        //-------------------------------------------
        //  Attempt to read the file and check the first bytes
        //-------------------------------------------

        using var stream = new MemoryStream();
        postedFile.CopyTo(stream);

        try
        {
            if (!stream.CanRead)
            {
                return false;
            }
            //------------------------------------------
            //   Check whether the image size exceeding the limit or not
            //------------------------------------------ 
            //if (postedFile.ContentLength < ImageMinimumBytes)
            //{
            //    return false;
            //}

            byte[] buffer = new byte[ImageMinimumBytes];
            stream.Read(buffer, 0, ImageMinimumBytes);
            string content = Encoding.UTF8.GetString(buffer);
            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        //-------------------------------------------
        //  Try to instantiate new Bitmap, if .NET will throw exception
        //  we can assume that it's not a valid image
        //-------------------------------------------

        try
        {
            if (postedFile.ContentType != "application/pdf" && !string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                using var bitmap = new Bitmap(stream);
                if (bitmap.Size.IsEmpty)
                {
                    return false;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            stream.Position = 0;
        }

        return true;
    }


    /// <summary>
    /// Quieta acentos y ñ a las palabras
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string NormalizaTexto(string data)
    {
        return Regex.Replace(data.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
    }

    /// <summary>
    /// Crea una instancia SqlParameter con el modelo de datos que se le proporcione
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns>Arreglo de parametros SqlParameter</returns>
    public static SqlParameter[] CrearSqlParametrosDelModelo<T>(T model)
    {
        var parameters = new List<SqlParameter>();

        foreach (var prop in typeof(T).GetProperties())
        {
            var value = prop.GetValue(model);
            var paramName = "@" + prop.Name; // Se asume que el nombre del parámetro es el nombre de la propiedad

            // Se verifica si la propiedad debe ser un parámetro de salida
            bool isOutputParameter = prop.Name.EndsWith("Output", StringComparison.OrdinalIgnoreCase);

            var sqlParam = new SqlParameter(paramName, value is null ? DBNull.Value : value)
            {
                // Si es un parámetro de salida, configurar la dirección
                Direction = isOutputParameter ? ParameterDirection.Output : ParameterDirection.Input,

                SqlDbType = GetSqlDbType(prop.PropertyType),
            };
            // Leer el atributo MaxLength, si está presente
            var maxLengthAttribute = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
            {
                sqlParam.Size = maxLengthAttribute.Length;
            }

            // Agregar el parámetro a la lista
            parameters.Add(sqlParam);
        }

        return parameters.ToArray();
    }

    public static void MapeaOutputParametersToModel<T>(T model, SqlParameter[] parameters)
    {
        var outputParameters = parameters.Where(p => p.Direction == ParameterDirection.Output).ToList();

        foreach (var param in outputParameters)
        {
            // Encuentra la propiedad del modelo que coincide con el nombre del parámetro
            var prop = typeof(T).GetProperty(param.ParameterName.TrimStart('@'), BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                // Asigna el valor del parámetro de salida a la propiedad del modelo
                var value = param.Value == DBNull.Value ? null : param.Value;
                prop.SetValue(model, Convert.ChangeType(value, prop.PropertyType));
            }
        }
    }


    private static SqlDbType GetSqlDbType(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type underlyingType)
        {
            type = underlyingType;
        }

        if (type == typeof(int))
            return SqlDbType.Int;
        if (type == typeof(long))
            return SqlDbType.BigInt;
        if (type == typeof(short))
            return SqlDbType.SmallInt;
        if (type == typeof(byte))
            return SqlDbType.TinyInt;
        if (type == typeof(decimal))
            return SqlDbType.Decimal;
        if (type == typeof(float))
            return SqlDbType.Float;
        if (type == typeof(double))
            return SqlDbType.Float;
        if (type == typeof(bool))
            return SqlDbType.Bit;
        if (type == typeof(string))
            return SqlDbType.VarChar;
        if (type == typeof(DateTime))
            return SqlDbType.DateTime;
        if (type == typeof(DateTimeOffset))
            return SqlDbType.DateTimeOffset;
        if (type == typeof(TimeSpan))
            return SqlDbType.Time;
        if (type == typeof(Guid))
            return SqlDbType.UniqueIdentifier;
        if (type == typeof(DateOnly))
            return SqlDbType.Date;
        if (type == typeof(byte[]))
            return SqlDbType.VarBinary; // Asume longitud variable
        // Agrega más tipos según sea necesario

        throw new NotSupportedException($"Tipo {type} no soportado.");
    }
}
