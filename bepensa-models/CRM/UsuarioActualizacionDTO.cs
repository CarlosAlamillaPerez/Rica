using bepensa_models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class UsuarioActualizacionDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [Display(Name = "Apellido Paterno")]
    public string ApellidoPaterno { get; set; } = null!;

    [Display(Name = "Apellido Materno")]
    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    //public byte[]? Password { get; set; }

    //public string RazonSocial { get; set; } = null!;

    public string? Calle { get; set; }

    public string? NumeroExterior { get; set; }

    public string? NumeroInterior { get; set; }

    public int? IdColonia { get; set; }

    //public string? CodigoPostal { get; set; }

    //public string? Ciudad { get; set; }

    public string? CalleInicio { get; set; }

    public string? CalleFin { get; set; }

    public string? Referencias { get; set; }

    public string? Telefono { get; set; }

    //public string? JefeDeVenta { get; set; }

    //public int? IdScdv { get; set; }

    //public bool CambiarPass { get; set; }

    //public bool Bloqueado { get; set; }

    //public string? Sesion { get; set; }

    //public int IntentosSesion { get; set; }

    //public string? TokenMovil { get; set; }

    //public bool Inscripcion { get; set; }

    //public DateTime? FechaInscripcion { get; set; }

    //public DateTime? FechaAcceso { get; set; }

    //public bool EnvioKit { get; set; }

    //public DateTime? FechaEnvKit { get; set; }

    //public int IdEstatus { get; set; }

    //public string Estatus { get; set; } = null!;

    //public DateTime FechaReg { get; set; }

    //public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public static implicit operator UsuarioRequest(UsuarioActualizacionDTO request)
    {
        return new UsuarioRequest
        {
            Id = request.Id,
            Nombre = request.Nombre,
            ApellidoPaterno = request.ApellidoPaterno,
            ApellidoMaterno = request.ApellidoMaterno,
            FechaNacimiento = request.FechaNacimiento,
            Sexo = request.Sexo,
            Celular = request.Celular,
            Email = request.Email,
            Calle = request.Calle,
            NumeroExterior = request.NumeroExterior,
            NumeroInterior = request.NumeroInterior,
            //CodigoPostal = request.CodigoPostal,
            IdColonia = request.IdColonia,
            //Ciudad = request.Ciudad,
            CalleInicio = request.CalleInicio,
            CalleFin = request.CalleFin,
            Referencias = request.Referencias,
            Telefono = request.Telefono
        };
    }
}
