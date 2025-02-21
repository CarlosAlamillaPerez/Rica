using bepensa_models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace bepensa_models.DTO;

public class InscripcionDTO : InscripcionRequest
{
    public int Id { get; set; }

    public int IdPrograma { get; set; }

    public int IdRuta { get; set; }

    public int IdCedi { get; set; }

    public int IdSupervisor { get; set; }

    [Display(Name = "Razón social")]
    public string RazonSocial { get; set; } = null!;

    public bool Inscripcion { get; set; } = false;

    public ProgramaDTO Programa { get; set; } = null!;

    public RutaDTO? Ruta { get; set; } = null!;

    public CediDTO Cedi { get; set; } = null!;

    public SupervisorDTO Supervisor { get; set; } = null!;
}
