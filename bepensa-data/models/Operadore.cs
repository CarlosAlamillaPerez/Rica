using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Operadore
{
    public int Id { get; set; }

    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Celular { get; set; }

    public string Password { get; set; } = null!;

    public string? SessionId { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<ArchivosDeCarga> ArchivosDeCargas { get; set; } = new List<ArchivosDeCarga>();

    public virtual ICollection<BitacoraDeContrasena> BitacoraDeContrasenas { get; set; } = new List<BitacoraDeContrasena>();

    public virtual ICollection<BitacoraDeFuerzasDeVentum> BitacoraDeFuerzasDeVenta { get; set; } = new List<BitacoraDeFuerzasDeVentum>();

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadoreIdOperadorNavigations { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadoreIdOperadorRegNavigations { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<Canale> Canales { get; set; } = new List<Canale>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<CuotasDeCompra> CuotasDeCompras { get; set; } = new List<CuotasDeCompra>();

    public virtual ICollection<DetallesDeEstadosDeCuentum> DetallesDeEstadosDeCuenta { get; set; } = new List<DetallesDeEstadosDeCuentum>();

    public virtual ICollection<EstadosDeCuentum> EstadosDeCuenta { get; set; } = new List<EstadosDeCuentum>();

    public virtual ICollection<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; } = new List<FuerzasDeVentaNegocio>();

    public virtual ICollection<FuerzasDeVentum> FuerzasDeVentumIdOperadorModNavigations { get; set; } = new List<FuerzasDeVentum>();

    public virtual ICollection<FuerzasDeVentum> FuerzasDeVentumIdOperadorRegNavigations { get; set; } = new List<FuerzasDeVentum>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Operadore> InverseIdOperadorModNavigation { get; set; } = new List<Operadore>();

    public virtual ICollection<Operadore> InverseIdOperadorRegNavigation { get; set; } = new List<Operadore>();

    public virtual ICollection<Llamada> LlamadaIdOperadorModNavigations { get; set; } = new List<Llamada>();

    public virtual ICollection<Llamada> LlamadaIdOperadorRegNavigations { get; set; } = new List<Llamada>();

    public virtual ICollection<Negocio> NegocioIdOperadorModNavigations { get; set; } = new List<Negocio>();

    public virtual ICollection<Negocio> NegocioIdOperadorRegNavigations { get; set; } = new List<Negocio>();

    public virtual ICollection<Parametro> ParametroIdOperadorModNavigations { get; set; } = new List<Parametro>();

    public virtual ICollection<Parametro> ParametroIdOperadorRegNavigations { get; set; } = new List<Parametro>();

    public virtual ICollection<Premio> PremioIdOperadorMNavigations { get; set; } = new List<Premio>();

    public virtual ICollection<Premio> PremioIdOperadorNavigations { get; set; } = new List<Premio>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacionIdOperadorModNavigations { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();

    public virtual ICollection<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacionIdOperadorRegNavigations { get; set; } = new List<PuntajesDeSubconceptosDeAcumulacion>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Ruta> Ruta { get; set; } = new List<Ruta>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();

    public virtual ICollection<SubconceptosDeAcumulacion> SubconceptosDeAcumulacions { get; set; } = new List<SubconceptosDeAcumulacion>();

    public virtual ICollection<Usuario> UsuarioIdOperadorModNavigations { get; set; } = new List<Usuario>();

    public virtual ICollection<Usuario> UsuarioIdOperadorRegNavigations { get; set; } = new List<Usuario>();
}
