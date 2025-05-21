﻿using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<BitacoraDeWhatsApp> BitacoraDeWhatsApps { get; set; } = new List<BitacoraDeWhatsApp>();

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<CatalogoCorreo> CatalogoCorreos { get; set; } = new List<CatalogoCorreo>();

    public virtual ICollection<CategoriasDePremio> CategoriasDePremios { get; set; } = new List<CategoriasDePremio>();

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductos { get; set; } = new List<CategoriasDeProducto>();

    public virtual ICollection<CategoriasLlamadum> CategoriasLlamada { get; set; } = new List<CategoriasLlamadum>();

    public virtual ICollection<Contactano> Contactanos { get; set; } = new List<Contactano>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Empaque> Empaques { get; set; } = new List<Empaque>();

    public virtual ICollection<EmpaquesProducto> EmpaquesProductos { get; set; } = new List<EmpaquesProducto>();

    public virtual ICollection<Encuesta> Encuesta { get; set; } = new List<Encuesta>();

    public virtual ICollection<FuerzaVentum> FuerzaVenta { get; set; } = new List<FuerzaVentum>();

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<MotivosContactano> MotivosContactanos { get; set; } = new List<MotivosContactano>();

    public virtual ICollection<Operadore> Operadores { get; set; } = new List<Operadore>();

    public virtual ICollection<OrigenesVentum> OrigenesVenta { get; set; } = new List<OrigenesVentum>();

    public virtual ICollection<Premio> Premios { get; set; } = new List<Premio>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<Seccione> Secciones { get; set; } = new List<Seccione>();

    public virtual ICollection<SubcategoriasLlamadum> SubcategoriasLlamada { get; set; } = new List<SubcategoriasLlamadum>();

    public virtual ICollection<SuborigenesVentum> SuborigenesVenta { get; set; } = new List<SuborigenesVentum>();

    public virtual ICollection<Tarjeta> Tarjeta { get; set; } = new List<Tarjeta>();

    public virtual ICollection<TiposDeEnvio> TiposDeEnvios { get; set; } = new List<TiposDeEnvio>();

    public virtual ICollection<TiposDePremio> TiposDePremios { get; set; } = new List<TiposDePremio>();

    public virtual ICollection<TiposDeTransaccion> TiposDeTransaccions { get; set; } = new List<TiposDeTransaccion>();

    public virtual ICollection<TiposLlamadum> TiposLlamada { get; set; } = new List<TiposLlamadum>();

    public virtual ICollection<UrlShortener> UrlShorteners { get; set; } = new List<UrlShortener>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
