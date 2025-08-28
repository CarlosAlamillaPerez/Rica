using System;
using System.Collections.Generic;

namespace bepensa_data.modelsRD;

public partial class Usuario
{
    public long Id { get; set; }

    public string Cuc { get; set; } = null!;

    public int? IdPrograma { get; set; }

    public long IdNegocio { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    public byte[]? Password { get; set; }

    public bool CambiarPass { get; set; }

    public bool Bloqueado { get; set; }

    public string? Sesion { get; set; }

    public bool Registro { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public DateTime FechaMod { get; set; }

    public int? IdOperadorMod { get; set; }

    public string? TokenMovil { get; set; }

    public virtual ICollection<BitacoraDeContrasena> BitacoraDeContrasenas { get; set; } = new List<BitacoraDeContrasena>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Contactano> Contactanos { get; set; } = new List<Contactano>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Programa? IdProgramaNavigation { get; set; }

    public virtual ICollection<Llamada> Llamada { get; set; } = new List<Llamada>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();
}
