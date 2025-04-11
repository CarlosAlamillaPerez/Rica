﻿using System;
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

    public byte[] Password { get; set; } = null!;

    public string? SessionId { get; set; }

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int? IdOperadorReg { get; set; }

    public int? IdOperadorMod { get; set; }

    public virtual ICollection<ArchivosDeCarga> ArchivosDeCargas { get; set; } = new List<ArchivosDeCarga>();

    public virtual ICollection<BitacoraDeContrasena> BitacoraDeContrasenas { get; set; } = new List<BitacoraDeContrasena>();

    public virtual ICollection<BitacoraDeFuerzasDeVentum> BitacoraDeFuerzasDeVenta { get; set; } = new List<BitacoraDeFuerzasDeVentum>();

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadoreIdOperadorAftdNavigations { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeOperadore> BitacoraDeOperadoreIdOperadorNavigations { get; set; } = new List<BitacoraDeOperadore>();

    public virtual ICollection<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; } = new List<BitacoraDeUsuario>();

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<Canale> Canales { get; set; } = new List<Canale>();

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductoIdOperadorModNavigations { get; set; } = new List<CategoriasDeProducto>();

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductoIdOperadorRegNavigations { get; set; } = new List<CategoriasDeProducto>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<CuotasDeCompra> CuotasDeCompras { get; set; } = new List<CuotasDeCompra>();

    public virtual ICollection<DetalleVenta> DetalleVentaIdOperadorModNavigations { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DetalleVenta> DetalleVentaIdOperadorRegNavigations { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DetallesDeEstadosDeCuentum> DetallesDeEstadosDeCuenta { get; set; } = new List<DetallesDeEstadosDeCuentum>();

    public virtual ICollection<Empaque> EmpaqueIdOperadorModNavigations { get; set; } = new List<Empaque>();

    public virtual ICollection<Empaque> EmpaqueIdOperadorRegNavigations { get; set; } = new List<Empaque>();

    public virtual ICollection<EmpaquesProducto> EmpaquesProductoIdOperadorModNavigations { get; set; } = new List<EmpaquesProducto>();

    public virtual ICollection<EmpaquesProducto> EmpaquesProductoIdOperadorRegNavigations { get; set; } = new List<EmpaquesProducto>();

    public virtual ICollection<EstadosDeCuentum> EstadosDeCuenta { get; set; } = new List<EstadosDeCuentum>();

    public virtual ICollection<EvaluacionesAcumulacion> EvaluacionesAcumulacionIdOperadorModNavigations { get; set; } = new List<EvaluacionesAcumulacion>();

    public virtual ICollection<EvaluacionesAcumulacion> EvaluacionesAcumulacionIdOperadorRegNavigations { get; set; } = new List<EvaluacionesAcumulacion>();

    public virtual ICollection<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; } = new List<FuerzasDeVentaNegocio>();

    public virtual ICollection<FuerzasDeVentum> FuerzasDeVentumIdOperadorModNavigations { get; set; } = new List<FuerzasDeVentum>();

    public virtual ICollection<FuerzasDeVentum> FuerzasDeVentumIdOperadorRegNavigations { get; set; } = new List<FuerzasDeVentum>();

    public virtual ICollection<HistoricoVenta> HistoricoVentaIdOperadorModNavigations { get; set; } = new List<HistoricoVenta>();

    public virtual ICollection<HistoricoVenta> HistoricoVentaIdOperadorRegNavigations { get; set; } = new List<HistoricoVenta>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Operadore? IdOperadorModNavigation { get; set; }

    public virtual Operadore? IdOperadorRegNavigation { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<ImagenesPromocione> ImagenesPromocioneIdOperadorModNavigations { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<ImagenesPromocione> ImagenesPromocioneIdOperadorRegNavigations { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<Operadore> InverseIdOperadorModNavigation { get; set; } = new List<Operadore>();

    public virtual ICollection<Operadore> InverseIdOperadorRegNavigation { get; set; } = new List<Operadore>();

    public virtual ICollection<Llamada> LlamadaIdOperadorModNavigations { get; set; } = new List<Llamada>();

    public virtual ICollection<Llamada> LlamadaIdOperadorRegNavigations { get; set; } = new List<Llamada>();

    public virtual ICollection<Marca> MarcaIdOperadorModNavigations { get; set; } = new List<Marca>();

    public virtual ICollection<Marca> MarcaIdOperadorRegNavigations { get; set; } = new List<Marca>();

    public virtual ICollection<MetasMensuale> MetasMensualeIdOperadorModNavigations { get; set; } = new List<MetasMensuale>();

    public virtual ICollection<MetasMensuale> MetasMensualeIdOperadorRegNavigations { get; set; } = new List<MetasMensuale>();

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Negocio> NegocioIdOperadorModNavigations { get; set; } = new List<Negocio>();

    public virtual ICollection<Negocio> NegocioIdOperadorRegNavigations { get; set; } = new List<Negocio>();

    public virtual ICollection<OrigenesVentum> OrigenesVentumIdOperadorModNavigations { get; set; } = new List<OrigenesVentum>();

    public virtual ICollection<OrigenesVentum> OrigenesVentumIdOperadorRegNavigations { get; set; } = new List<OrigenesVentum>();

    public virtual ICollection<Parametro> ParametroIdOperadorModNavigations { get; set; } = new List<Parametro>();

    public virtual ICollection<Parametro> ParametroIdOperadorRegNavigations { get; set; } = new List<Parametro>();

    public virtual ICollection<Premio> PremioIdOperadorMNavigations { get; set; } = new List<Premio>();

    public virtual ICollection<Premio> PremioIdOperadorNavigations { get; set; } = new List<Premio>();

    public virtual ICollection<Producto> ProductoIdOperadorModNavigations { get; set; } = new List<Producto>();

    public virtual ICollection<Producto> ProductoIdOperadorRegNavigations { get; set; } = new List<Producto>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<PuntajesAcumulacion> PuntajesAcumulacionIdOperadorModNavigations { get; set; } = new List<PuntajesAcumulacion>();

    public virtual ICollection<PuntajesAcumulacion> PuntajesAcumulacionIdOperadorRegNavigations { get; set; } = new List<PuntajesAcumulacion>();

    public virtual ICollection<Redencione> Redenciones { get; set; } = new List<Redencione>();

    public virtual ICollection<Ruta> Ruta { get; set; } = new List<Ruta>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();

    public virtual ICollection<SegmentosAcumulacion> SegmentosAcumulacionIdOperadorModNavigations { get; set; } = new List<SegmentosAcumulacion>();

    public virtual ICollection<SegmentosAcumulacion> SegmentosAcumulacionIdOperadorRegNavigations { get; set; } = new List<SegmentosAcumulacion>();

    public virtual ICollection<SubconceptosDeAcumulacion> SubconceptosDeAcumulacionIdOperadorModNavigations { get; set; } = new List<SubconceptosDeAcumulacion>();

    public virtual ICollection<SubconceptosDeAcumulacion> SubconceptosDeAcumulacionIdOperadorRegNavigations { get; set; } = new List<SubconceptosDeAcumulacion>();

    public virtual ICollection<SuborigenesVentum> SuborigenesVentumIdOperadorModNavigations { get; set; } = new List<SuborigenesVentum>();

    public virtual ICollection<SuborigenesVentum> SuborigenesVentumIdOperadorRegNavigations { get; set; } = new List<SuborigenesVentum>();

    public virtual ICollection<Usuario> UsuarioIdOperadorModNavigations { get; set; } = new List<Usuario>();

    public virtual ICollection<Usuario> UsuarioIdOperadorRegNavigations { get; set; } = new List<Usuario>();

    public virtual ICollection<Venta> VentaIdOperadorModNavigations { get; set; } = new List<Venta>();

    public virtual ICollection<Venta> VentaIdOperadorRegNavigations { get; set; } = new List<Venta>();
}
