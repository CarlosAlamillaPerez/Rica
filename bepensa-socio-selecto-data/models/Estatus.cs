using System;
using System.Collections.Generic;

namespace bepensa_socio_selecto_data.models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; } = new List<BitacoraEnvioCorreo>();

    public virtual ICollection<CatalogoCorreo> CatalogoCorreos { get; set; } = new List<CatalogoCorreo>();

    public virtual ICollection<CategoriasDeLlamadum> CategoriasDeLlamada { get; set; } = new List<CategoriasDeLlamadum>();

    public virtual ICollection<CategoriasDePremio> CategoriasDePremios { get; set; } = new List<CategoriasDePremio>();

    public virtual ICollection<CategoriasDeProducto> CategoriasDeProductos { get; set; } = new List<CategoriasDeProducto>();

    public virtual ICollection<FuerzasDeVentum> FuerzasDeVenta { get; set; } = new List<FuerzasDeVentum>();

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();

    public virtual ICollection<Operadore> Operadores { get; set; } = new List<Operadore>();

    public virtual ICollection<Premio> Premios { get; set; } = new List<Premio>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<Seccione> Secciones { get; set; } = new List<Seccione>();

    public virtual ICollection<SubcategoriasDeLlamadum> SubcategoriasDeLlamada { get; set; } = new List<SubcategoriasDeLlamadum>();

    public virtual ICollection<TiposDeEnvio> TiposDeEnvios { get; set; } = new List<TiposDeEnvio>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Usuarios2> Usuarios2s { get; set; } = new List<Usuarios2>();
}
