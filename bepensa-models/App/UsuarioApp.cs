using bepensa_models.DTO;
using bepensa_models.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.App
{
    public class UsuarioApp
    {
        public int Id { get; set; }

    public int IdPrograma { get; set; }

    public string Programa { get; set; } = null!;

    //(public int IdCanal { get; set; }

    //public string Canal { get; set; } = null!;

    public int? IdRuta { get; set; }

    public string? Ruta { get; set; }

    public int IdCedi { get; set; }

    public string Cedi { get; set; } = null!;

    public int IdSupervisor { get; set; }

    public string Supervisor { get; set; } = null!;

    public string Cuc { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    //public byte[]? Password { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? Calle { get; set; }

    public string? NumeroExterior { get; set; }

    public string? NumeroInterior { get; set; }

    public int? IdColonia { get; set; }

    public string? Ciudad { get; set; }

    public string? CalleInicio { get; set; }

    public string? CalleFin { get; set; }

    public string? Referencias { get; set; }

    public string? Telefono { get; set; }

    public string? JefeDeVenta { get; set; }

    public int? IdScdv { get; set; }

    public bool CambiarPass { get; set; }

    public bool Bloqueado { get; set; }

    public string? Sesion { get; set; }

    public int IntentosSesion { get; set; }

    public string? TokenMovil { get; set; }

    public bool Inscripcion { get; set; }

    public DateTime? FechaInscripcion { get; set; }

    public DateTime? FechaAcceso { get; set; }

    public bool EnvioKit { get; set; }

    public DateTime? FechaEnvKit { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public DateTime? FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }
    }
}
