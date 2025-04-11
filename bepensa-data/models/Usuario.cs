using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Usuario
{
    public int Id { get; set; }

    public int IdPrograma { get; set; }

    public int? IdRuta { get; set; }

    public int IdCedi { get; set; }

    public int IdSupervisor { get; set; }

    public string Cuc { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    public byte[]? Password { get; set; }

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

    public string? Supervisor { get; set; }

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

    public virtual ICollection<BitacoraDeContrasena> BitacoraDeContrasenas { get; set; } = new List<BitacoraDeContrasena>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<CumplimientosPortafolio> CumplimientosPortafolios { get; set; } = new List<CumplimientosPortafolio>();

    public virtual ICollection<EvaluacionesAcumulacion> EvaluacionesAcumulacions { get; set; } = new List<EvaluacionesAcumulacion>();

    public virtual ICollection<HistoricoVenta> HistoricoVenta { get; set; } = new List<HistoricoVenta>();

    public virtual Cedi IdCediNavigation { get; set; } = null!;

    public virtual Colonia? IdColoniaNavigation { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual Programa IdProgramaNavigation { get; set; } = null!;

    public virtual Ruta? IdRutaNavigation { get; set; }

    public virtual SubcanalesDeVentum? IdScdvNavigation { get; set; }

    public virtual Supervisore IdSupervisorNavigation { get; set; } = null!;

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();

    public virtual ICollection<MetasMensuale> MetasMensuales { get; set; } = new List<MetasMensuale>();

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
