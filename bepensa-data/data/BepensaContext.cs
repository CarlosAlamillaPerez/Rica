using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using bepensa_data.models;

namespace bepensa_data.data;

public partial class BepensaContext : DbContext
{
    public BepensaContext(DbContextOptions<BepensaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ArchivosDeCarga> ArchivosDeCargas { get; set; }

    public virtual DbSet<BitacoraDeContrasena> BitacoraDeContrasenas { get; set; }

    public virtual DbSet<BitacoraDeFuerzasDeVentum> BitacoraDeFuerzasDeVenta { get; set; }

    public virtual DbSet<BitacoraDeOperadore> BitacoraDeOperadores { get; set; }

    public virtual DbSet<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; }

    public virtual DbSet<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; }

    public virtual DbSet<Canale> Canales { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Carrusel> Carrusels { get; set; }

    public virtual DbSet<CatalogoCorreo> CatalogoCorreos { get; set; }

    public virtual DbSet<CategoriasDeLlamadum> CategoriasDeLlamada { get; set; }

    public virtual DbSet<CategoriasDePremio> CategoriasDePremios { get; set; }

    public virtual DbSet<CategoriasDeProducto> CategoriasDeProductos { get; set; }

    public virtual DbSet<Cedi> Cedis { get; set; }

    public virtual DbSet<CodigosMensaje> CodigosMensajes { get; set; }

    public virtual DbSet<Colonia> Colonias { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<ConceptosDeAcumulacion> ConceptosDeAcumulacions { get; set; }

    public virtual DbSet<CuotasDeCompra> CuotasDeCompras { get; set; }

    public virtual DbSet<DetalleDeMetaDeCompra> DetalleDeMetaDeCompras { get; set; }

    public virtual DbSet<DetallesDeEstadosDeCuentum> DetallesDeEstadosDeCuenta { get; set; }

    public virtual DbSet<DetallesDePortafolio> DetallesDePortafolios { get; set; }

    public virtual DbSet<Embotelladora> Embotelladoras { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadosDeCuentum> EstadosDeCuenta { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<EstatusDeCarrito> EstatusDeCarritos { get; set; }

    public virtual DbSet<EstatusDeLlamadum> EstatusDeLlamada { get; set; }

    public virtual DbSet<EstatusDeRedencione> EstatusDeRedenciones { get; set; }

    public virtual DbSet<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; }

    public virtual DbSet<FuerzasDeVentum> FuerzasDeVenta { get; set; }

    public virtual DbSet<LayaoutEjecucion> LayaoutEjecucions { get; set; }

    public virtual DbSet<LayoutAltaInscripciónDomV1> LayoutAltaInscripciónDomV1s { get; set; }

    public virtual DbSet<LayoutCompra> LayoutCompras { get; set; }

    public virtual DbSet<LayoutDePremio> LayoutDePremios { get; set; }

    public virtual DbSet<Llamada> Llamadas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Mensajeria> Mensajerias { get; set; }

    public virtual DbSet<MetodosDeEntrega> MetodosDeEntregas { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Negocio> Negocios { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Origene> Origenes { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<Periodo> Periodos { get; set; }

    public virtual DbSet<Premio> Premios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductosSelecto> ProductosSelectos { get; set; }

    public virtual DbSet<Programa> Programas { get; set; }

    public virtual DbSet<PuntajesDeSubconceptosDeAcumulacion> PuntajesDeSubconceptosDeAcumulacions { get; set; }

    public virtual DbSet<Redencione> Redenciones { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ruta> Rutas { get; set; }

    public virtual DbSet<Saldo> Saldos { get; set; }

    public virtual DbSet<Seccione> Secciones { get; set; }

    public virtual DbSet<SeccionesPorRol> SeccionesPorRols { get; set; }

    public virtual DbSet<SeguimientoDeRedencione> SeguimientoDeRedenciones { get; set; }

    public virtual DbSet<SubcategoriasDeLlamadum> SubcategoriasDeLlamada { get; set; }

    public virtual DbSet<SubconceptosDeAcumulacion> SubconceptosDeAcumulacions { get; set; }

    public virtual DbSet<Supervisore> Supervisores { get; set; }

    public virtual DbSet<TiposDeAccione> TiposDeAcciones { get; set; }

    public virtual DbSet<TiposDeArchivoDeCarga> TiposDeArchivoDeCargas { get; set; }

    public virtual DbSet<TiposDeEnvio> TiposDeEnvios { get; set; }

    public virtual DbSet<TiposDeLlamadum> TiposDeLlamada { get; set; }

    public virtual DbSet<TiposDeMovimiento> TiposDeMovimientos { get; set; }

    public virtual DbSet<TiposDeOperacion> TiposDeOperacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vista> Vistas { get; set; }

    public virtual DbSet<Zona> Zonas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArchivosDeCarga>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Archivos__3214EC07E4910918");

            entity.ToTable("ArchivosDeCarga");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreDelArchivo)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ArchivosDeCargas)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArchivosD__IdOpe__14B10FFA");

            entity.HasOne(d => d.IdTipoDeArchivoDeCargaNavigation).WithMany(p => p.ArchivosDeCargas)
                .HasForeignKey(d => d.IdTipoDeArchivoDeCarga)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArchivosD__IdTip__12C8C788");
        });

        modelBuilder.Entity<BitacoraDeContrasena>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bitacora__3214EC0769FAC994");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Origen).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(250);

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.BitacoraDeContrasenas)
                .HasForeignKey(d => d.IdOperador)
                .HasConstraintName("FK__BitacoraD__IdOpe__7908F585");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraDeContrasenas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_BitacoraDeContrasenas_Usuarios");
        });

        modelBuilder.Entity<BitacoraDeFuerzasDeVentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bitacora__3214EC07F9102DF2");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notas)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdFuerzaDeVentaNavigation).WithMany(p => p.BitacoraDeFuerzasDeVenta)
                .HasForeignKey(d => d.IdFuerzaDeVenta)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraDeFuerzasDeVenta).HasForeignKey(d => d.IdOperadorReg);

            entity.HasOne(d => d.IdTipoDeOperacionNavigation).WithMany(p => p.BitacoraDeFuerzasDeVenta)
                .HasForeignKey(d => d.IdTipoDeOperacion)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<BitacoraDeOperadore>(entity =>
        {
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Notas)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.BitacoraDeOperadoreIdOperadorNavigations)
                .HasForeignKey(d => d.IdOperador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeOperadores_Operadores");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraDeOperadoreIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK_BitacoraDeOperadores_OperadorRegistro");

            entity.HasOne(d => d.IdTipoDeOperacionNavigation).WithMany(p => p.BitacoraDeOperadores)
                .HasForeignKey(d => d.IdTipoDeOperacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeOperadores_TiposDeOperacion");
        });

        modelBuilder.Entity<BitacoraDeUsuario>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notas).IsUnicode(false);

            entity.HasOne(d => d.IdOperdorRegNavigation).WithMany(p => p.BitacoraDeUsuarios)
                .HasForeignKey(d => d.IdOperdorReg)
                .HasConstraintName("FK_BitacoraDeUsuarios_Operadores");

            entity.HasOne(d => d.IdTipoDeOperacionNavigation).WithMany(p => p.BitacoraDeUsuarios)
                .HasForeignKey(d => d.IdTipoDeOperacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeUsuarios_TiposDeOperacion");
        });

        modelBuilder.Entity<BitacoraEnvioCorreo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bitacora__3214EC077F7C0FE8");

            entity.Property(e => e.Codigo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.FechaLectura).HasColumnType("datetime");
            entity.Property(e => e.MailItemId).HasColumnName("MailItem_Id");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.BitacoraEnvioCorreos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BitacoraE__IdEst__5A1A5A11");

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.BitacoraEnvioCorreos).HasForeignKey(d => d.IdOperador);
        });

        modelBuilder.Entity<Canale>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Canales)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Carrito__3214EC073F6DD4F3");

            entity.ToTable("Carrito");

            entity.Property(e => e.FechaRedencion).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TelefonoRecarga)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusCarritoNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdEstatusCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPremioNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdPremio)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Carrusel>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Carrusel");

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Imagen)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Link).IsUnicode(false);
            entity.Property(e => e.RutaImagen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VigenciaFin).HasColumnType("datetime");
            entity.Property(e => e.VigenciaInicio).HasColumnType("datetime");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany().HasForeignKey(d => d.IdEstatus);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany().HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany()
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOrigenNavigation).WithMany()
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProgramaNavigation).WithMany()
                .HasForeignKey(d => d.IdPrograma)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdTipoDeAccionNavigation).WithMany()
                .HasForeignKey(d => d.IdTipoDeAccion)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdVistaNavigation).WithMany()
                .HasForeignKey(d => d.IdVista)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CatalogoCorreo>(entity =>
        {
            entity.Property(e => e.Cco)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Codigo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.FechaUltimaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Html).IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CatalogoCorreos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatalogoCorreos_Estatus");
        });

        modelBuilder.Entity<CategoriasDeLlamadum>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CategoriasDeLlamada)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriasDeLlamada_Estatus");
        });

        modelBuilder.Entity<CategoriasDePremio>(entity =>
        {
            entity.HasIndex(e => new { e.ClaveCategoria, e.Digital }, "UQ_CategoriasDePremios_ClaveCategoria").IsUnique();

            entity.Property(e => e.ClaveCategoria)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Estilos)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Fechareg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Imgurl)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Ordenar).HasDefaultValue(1);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CategoriasDePremios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriasDePremios_Estatus");
        });

        modelBuilder.Entity<CategoriasDeProducto>(entity =>
        {
            entity.ToTable("CategoriasDeProducto");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CategoriasDeProductos)
                .HasForeignKey(d => d.IdEstatus)
                .HasConstraintName("FK_CategoriasDeProducto_Estatus");

            entity.HasOne(d => d.IdSubconceptoDeAcumulacionNavigation).WithMany(p => p.CategoriasDeProductos)
                .HasForeignKey(d => d.IdSubconceptoDeAcumulacion)
                .HasConstraintName("FK_CategoriasDeProducto_SubconceptosDeAcumulacion");
        });

        modelBuilder.Entity<Cedi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CEDIs__3214EC07C56A9942");

            entity.ToTable("CEDIs");

            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.Cedis)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CodigosMensaje>(entity =>
        {
            entity.Property(e => e.Mensaje).HasMaxLength(300);
        });

        modelBuilder.Entity<Colonia>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Ciudad).HasMaxLength(150);
            entity.Property(e => e.Colonia1)
                .HasMaxLength(150)
                .HasColumnName("Colonia");
            entity.Property(e => e.Cp)
                .HasMaxLength(7)
                .HasColumnName("CP");
            entity.Property(e => e.Fechareg).HasColumnType("datetime");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Colonia)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colonias_Municipios");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compras__3214EC07673789DE");

            entity.Property(e => e.CajasFisicas).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CajasUnitarias).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Importe).HasColumnType("money");

            entity.HasOne(d => d.IdArchivoDeCargaNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdArchivoDeCarga)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdArchi__025D5595");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdNegoc__035179CE");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdOpera__07220AB2");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdPerio__04459E07");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compras__IdProdu__0539C240");
        });

        modelBuilder.Entity<ConceptosDeAcumulacion>(entity =>
        {
            entity.ToTable("ConceptosDeAcumulacion");

            entity.HasIndex(e => e.Nombre, "IX_ConceptosDeAcumulacion").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CuotasDeCompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CuotasDe__3214EC070CB4ED46");

            entity.ToTable("CuotasDeCompra");

            entity.HasIndex(e => new { e.IdNegocio, e.IdPeriodo, e.IdCategoriaDeProducto }, "IX_CuotasDeCompra_IdNegocio_IdPeriodo_IdSubconceptoDeAcumulacion").IsUnique();

            entity.Property(e => e.Cuota).HasColumnType("money");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCategoriaDeProductoNavigation).WithMany(p => p.CuotasDeCompras)
                .HasForeignKey(d => d.IdCategoriaDeProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CuotasDeCompra_CategoriasDeProducto");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.CuotasDeCompras)
                .HasForeignKey(d => d.IdNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuotasDeC__IdNeg__3BCADD1B");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.CuotasDeCompras)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuotasDeC__IdOpe__1352D76D");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.CuotasDeCompras)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuotasDeC__IdPer__3CBF0154");
        });

        modelBuilder.Entity<DetalleDeMetaDeCompra>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DetalleDeMetaDeCompra");

            entity.Property(e => e.Compra).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Cuota).HasColumnType("money");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetallesDeEstadosDeCuentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detalles__3214EC07F0D9FE93");

            entity.HasIndex(e => new { e.IdEstadoDeCuenta, e.IdSubconceptoDeAcumulacion }, "UQ_DetallesDeEstadosDeCuenta_IdEstadoDeCuenta_IdSubconceptoDeAcumulacion");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Comentarios)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEstadoDeCuentaNavigation).WithMany(p => p.DetallesDeEstadosDeCuenta)
                .HasForeignKey(d => d.IdEstadoDeCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesD__IdEst__004002F9");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.DetallesDeEstadosDeCuenta)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesD__IdOpe__031C6FA4");

            entity.HasOne(d => d.IdSubconceptoDeAcumulacionNavigation).WithMany(p => p.DetallesDeEstadosDeCuenta)
                .HasForeignKey(d => d.IdSubconceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesD__IdSub__01342732");
        });

        modelBuilder.Entity<DetallesDePortafolio>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DetallesDePortafolios");

            entity.Property(e => e.Importe).HasColumnType("money");
            entity.Property(e => e.NombrePortafolio)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Sku)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Embotelladora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Embotell__3214EC07E05F0B1E");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .HasColumnName("Estado");
            entity.Property(e => e.Fechareg).HasColumnType("datetime");
        });

        modelBuilder.Entity<EstadosDeCuentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstadoDe__3214EC07BB1561CD");

            entity.HasIndex(e => new { e.IdNegocio, e.IdPeriodo, e.IdConceptoDeAcumulacion }, "UQ_EstadosDeCuenta_IdNegocio_IdPeriodo_IdConceptoDeAcumulacion");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdConceptoDeAcumulacionNavigation).WithMany(p => p.EstadosDeCuenta)
                .HasForeignKey(d => d.IdConceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EstadoDeC__IdCon__21D600EE");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.EstadosDeCuenta)
                .HasForeignKey(d => d.IdNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EstadoDeC__IdNeg__1FEDB87C");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.EstadosDeCuenta)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EstadosDe__IdOpe__7D63964E");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.EstadosDeCuenta)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EstadoDeC__IdPer__20E1DCB5");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.ToTable("Estatus");

            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstatusDeCarrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstatusD__3214EC07D7E148DA");

            entity.ToTable("EstatusDeCarrito");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstatusDeLlamadum>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(35);
        });

        modelBuilder.Entity<EstatusDeRedencione>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.PrefijoRms)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PrefijoRMS");
        });

        modelBuilder.Entity<FuerzasDeVentaNegocio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FuerzasD__3214EC07979FD04E");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdFuerzaDeVentaNavigation).WithMany(p => p.FuerzasDeVentaNegocios)
                .HasForeignKey(d => d.IdFuerzaDeVenta)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.FuerzasDeVentaNegocios)
                .HasForeignKey(d => d.IdNegocio)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.FuerzasDeVentaNegocios)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FuerzasDeVentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FuerzasD__3214EC07D7A25C64");

            entity.HasIndex(e => e.Email, "UQ_FuerzasDeVenta_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Usuario, "UQ_FuerzasDeVenta_Usuario").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(135)
                .IsUnicode(false);
            entity.Property(e => e.Sesion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TokenMovil)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.FuerzasDeVenta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.FuerzasDeVentumIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.FuerzasDeVentumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LayaoutEjecucion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LayaoutEjecucion");

            entity.Property(e => e.Cuc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUC");
            entity.Property(e => e.Indicador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mes)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LayoutAltaInscripciónDomV1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Layout Alta Inscripción DOM v1");

            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Cedi)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CEDI");
            entity.Property(e => e.Cuc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CUC");
            entity.Property(e => e.Embotelladora)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Municipio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NombreDeJefeVenta)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Nombre_de_Jefe_Venta");
            entity.Property(e => e.NombreSupervisor)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoFijo)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LayoutCompra>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LayoutCompra");

            entity.Property(e => e.Cuc).HasColumnName("CUC");
            entity.Property(e => e.Importe).HasColumnType("money");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unidad).HasColumnType("decimal(18, 10)");
        });

        modelBuilder.Entity<LayoutDePremio>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Layout_De_Premios");

            entity.Property(e => e.F10).HasMaxLength(255);
            entity.Property(e => e.F12).HasMaxLength(255);
            entity.Property(e => e.F13).HasMaxLength(255);
            entity.Property(e => e.F14).HasMaxLength(255);
            entity.Property(e => e.F2).HasMaxLength(255);
            entity.Property(e => e.F3).HasMaxLength(255);
            entity.Property(e => e.F4).HasMaxLength(255);
            entity.Property(e => e.F5).HasMaxLength(255);
            entity.Property(e => e.F6).HasMaxLength(255);
            entity.Property(e => e.F8).HasColumnType("money");
        });

        modelBuilder.Entity<Llamada>(entity =>
        {
            entity.Property(e => e.Comentario)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Tema).HasMaxLength(255);

            entity.HasOne(d => d.IdEdlNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdEdl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Llamadas_EstatusDeLlamada");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.LlamadaIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_Llamadas_Operadores1");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.LlamadaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Llamadas_Operadores");

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .HasConstraintName("FK_Llamadas_Llamadas");

            entity.HasOne(d => d.IdSdlNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdSdl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Llamadas_CategoriasDeLlamada");

            entity.HasOne(d => d.IdTdlNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdTdl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Llamadas_TiposDeLlamada");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Llamadas_Usuarios");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mensajeria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mensajer__3214EC0703F6CE31");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MetodosDeEntrega>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MetodosD__3214EC07F893A792");

            entity.ToTable("MetodosDeEntrega");

            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fechareg).HasColumnType("datetime");
            entity.Property(e => e.Municipio1)
                .HasMaxLength(150)
                .HasColumnName("Municipio");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Municipios)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Municipios_Estados");
        });

        modelBuilder.Entity<Negocio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Negocios__3214EC073255DF9A");

            entity.HasIndex(e => e.Cuc, "IX_Negocios_Cuc_Unique").IsUnique();

            entity.Property(e => e.Barrio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdCedi).HasColumnName("IdCEDI");
            entity.Property(e => e.Municipio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Provincia)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Referencias)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoFijo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdCediNavigation).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.IdCedi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Negocios__IdCEDI__4959E263");

            entity.HasOne(d => d.IdEmbotelladoraNavigation).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.IdEmbotelladora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Negocios__IdEmbo__4A4E069C");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Negocios__IdEsta__4B422AD5");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.NegocioIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK__Negocios__IdOper__4D2A7347");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.NegocioIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK__Negocios__IdOper__4C364F0E");

            entity.HasOne(d => d.IdSupervisorNavigation).WithMany(p => p.Negocios)
                .HasForeignKey(d => d.IdSupervisor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Negocios__IdSupe__4E1E9780");
        });

        modelBuilder.Entity<Operadore>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Operadores_Email").IsUnique();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(350)
                .IsUnicode(false);
            entity.Property(e => e.SessionId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Operadores)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operadores_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.InverseIdOperadorModNavigation)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_Operadores_Operadores1");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.InverseIdOperadorRegNavigation)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK_Operadores_Operadores");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Operadores)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operadores_Roles");
        });

        modelBuilder.Entity<Origene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Origenes__3214EC07DC577178");

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.HasIndex(e => e.Tag, "IX_Parametros").IsUnique();

            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Tag)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.ParametroIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parametros_Operadores1");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ParametroIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parametros_Operadores");
        });

        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Periodos__3214EC07CD82BDF3");

            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Premio>(entity =>
        {
            entity.HasIndex(e => e.Sku, "IX_Premios").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(4500)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(50);
            entity.Property(e => e.Marca)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PromoFechaFin).HasColumnType("datetime");
            entity.Property(e => e.PromoFechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Sku)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaDePremioNavigation).WithMany(p => p.Premios)
                .HasForeignKey(d => d.IdCategoriaDePremio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_CategoriasDePremios");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Premios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_Estatus");

            entity.HasOne(d => d.IdMetodoDeEntregaNavigation).WithMany(p => p.Premios).HasForeignKey(d => d.IdMetodoDeEntrega);

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.PremioIdOperadorNavigations)
                .HasForeignKey(d => d.IdOperador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_Operadores");

            entity.HasOne(d => d.IdOperadorMNavigation).WithMany(p => p.PremioIdOperadorMNavigations)
                .HasForeignKey(d => d.IdOperadorM)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_OperadoresM");

            entity.HasOne(d => d.IdTipoDeEnvioNavigation).WithMany(p => p.Premios)
                .HasForeignKey(d => d.IdTipoDeEnvio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_TiposDeEnvio");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasIndex(e => e.Sku, "UQ_Productos_Sku").IsUnique();

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Presentacion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Sabor)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Segmento)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Sku)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Unidades).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaDeProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoriaDeProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_CategoriasDeProducto");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Estatus");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Marcas");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Operadores");
        });

        modelBuilder.Entity<ProductosSelecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07CDCDBB5C");

            entity.HasIndex(e => new { e.IdSubconceptoDeAcumulacion, e.IdProducto }, "IX_ProductosSelectos_IdSubconceptoDeAcumulacion_IdProducto").IsUnique();

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductosSelectos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__IdPro__7B7B4DDC");

            entity.HasOne(d => d.IdSubconceptoDeAcumulacionNavigation).WithMany(p => p.ProductosSelectos)
                .HasForeignKey(d => d.IdSubconceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__IdSub__7A8729A3");
        });

        modelBuilder.Entity<Programa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Programa__3214EC0764CA7ADB");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.PrefijoRms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PrefijoRMS");

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.Programas)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Programas)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Programas__IdEst__5CF6C6BC");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Programas)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Programas__IdOpe__5EDF0F2E");
        });

        modelBuilder.Entity<PuntajesDeSubconceptosDeAcumulacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puntajes__3214EC074BC36C0F");

            entity.ToTable("PuntajesDeSubconceptosDeAcumulacion");

            entity.HasIndex(e => new { e.IdSubconceptoDeAcumulacion, e.IdPrograma }, "UQ_PuntajesDeSubconceptosDeAcumulacion_IdSubconceptoDeAcumulacion_IdPrograma").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.PuntajesDeSubconceptosDeAcumulacionIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK__PuntajesD__IdOpe__75C27486");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.PuntajesDeSubconceptosDeAcumulacionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PuntajesD__IdOpe__74CE504D");

            entity.HasOne(d => d.IdProgramaNavigation).WithMany(p => p.PuntajesDeSubconceptosDeAcumulacions)
                .HasForeignKey(d => d.IdPrograma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PuntajesD__IdPro__72E607DB");

            entity.HasOne(d => d.IdSubconceptoDeAcumulacionNavigation).WithMany(p => p.PuntajesDeSubconceptosDeAcumulacions)
                .HasForeignKey(d => d.IdSubconceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PuntajesD__IdSub__71F1E3A2");
        });

        modelBuilder.Entity<Redencione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Redencio__3214EC076A4CDAD5");

            entity.Property(e => e.Barrio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Comentarios).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FolioRms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FolioRMS");
            entity.Property(e => e.Guia)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.MetodoDeEntrega)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Municipio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NombreDelSolicitante)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PedidoRms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PedidoRMS");
            entity.Property(e => e.Provincia)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Referencias)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoAlterno)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDetalleDeEstadoDeCuentaNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdDetalleDeEstadoDeCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Redenciones_DetallesDeEstadosDecuenta_IdDetalleDeEstadoDeCuenta");

            entity.HasOne(d => d.IdMensajeriaNavigation).WithMany(p => p.Redenciones).HasForeignKey(d => d.IdMensajeria);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Redenciones).HasForeignKey(d => d.IdOperadorReg);

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPremioNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdPremio)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Redenciones_Usuarios");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.Property(e => e.ColorBg)
                .HasMaxLength(50)
                .HasColumnName("Color_Bg");
            entity.Property(e => e.ColorTxt)
                .HasMaxLength(50)
                .HasColumnName("Color_Txt");
            entity.Property(e => e.Icono).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.StoreProcedure).HasMaxLength(150);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Estatus");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Secciones");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_Rutas_Nombre").IsUnique();

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Saldo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Saldos__3214EC07BEEA0ED5");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Saldo1).HasColumnName("Saldo");

            entity.HasOne(d => d.IdDetalleDeEstadoDeCuentaNavigation).WithMany(p => p.Saldos)
                .HasForeignKey(d => d.IdDetalleDeEstadoDeCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saldos__IdDetall__06ED0088");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Saldos)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saldos__IdOperad__09C96D33");

            entity.HasOne(d => d.IdTipoDeMovimientoNavigation).WithMany(p => p.Saldos)
                .HasForeignKey(d => d.IdTipoDeMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saldos__IdTipoDe__07E124C1");
        });

        modelBuilder.Entity<Seccione>(entity =>
        {
            entity.Property(e => e.Area)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Controlador)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Icon)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Vista)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Secciones)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Secciones_Estatus");

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .HasConstraintName("FK_Secciones_Secciones");
        });

        modelBuilder.Entity<SeccionesPorRol>(entity =>
        {
            entity.ToTable("SeccionesPorRol");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.SeccionesPorRols)
                .HasForeignKey(d => d.Idrol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeccionesPorRol_Roles");

            entity.HasOne(d => d.IdseccionNavigation).WithMany(p => p.SeccionesPorRols)
                .HasForeignKey(d => d.Idseccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeccionesPorRol_Secciones");
        });

        modelBuilder.Entity<SeguimientoDeRedencione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seguimie__3214EC07A03DA0F2");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEstatusDeRedencionNavigation).WithMany(p => p.SeguimientoDeRedenciones)
                .HasForeignKey(d => d.IdEstatusDeRedencion)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdRedencionNavigation).WithMany(p => p.SeguimientoDeRedenciones)
                .HasForeignKey(d => d.IdRedencion)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SubcategoriasDeLlamadum>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdCdlNavigation).WithMany(p => p.SubcategoriasDeLlamada)
                .HasForeignKey(d => d.IdCdl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubcategoriasDeLlamada_CategoriasDeLlamada");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.SubcategoriasDeLlamada)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubcategoriasDeLlamada_Estatus");
        });

        modelBuilder.Entity<SubconceptosDeAcumulacion>(entity =>
        {
            entity.ToTable("SubconceptosDeAcumulacion");

            entity.HasIndex(e => e.Nombre, "IX_SubconceptosDeAcumulacion_Nombre").IsUnique();

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConceptoDeAcumulacionNavigation).WithMany(p => p.SubconceptosDeAcumulacions)
                .HasForeignKey(d => d.IdConceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubconceptosDeAcumulacion_ConceptosDeAcumulacion");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.SubconceptosDeAcumulacions)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK_SubconceptosDeAcumulacion_Operadores");
        });

        modelBuilder.Entity<Supervisore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supervis__3214EC076893D342");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposDeAccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposDeA__3214EC0766A9C94B");

            entity.HasIndex(e => e.Accion, "UQ_TiposDeAcciones_Accion").IsUnique();

            entity.Property(e => e.Accion)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposDeArchivoDeCarga>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposDeA__3214EC0745C09995");

            entity.ToTable("TiposDeArchivoDeCarga");

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposDeEnvio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TipoDeEnvio");

            entity.ToTable("TiposDeEnvio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Envio).HasMaxLength(50);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.TiposDeEnvios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TiposDeEnvio_Estatus");
        });

        modelBuilder.Entity<TiposDeLlamadum>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(35);
        });

        modelBuilder.Entity<TiposDeMovimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TiposDeM__3214EC07A6D59129");

            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposDeOperacion>(entity =>
        {
            entity.ToTable("TiposDeOperacion");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC078E1CCBB3");

            entity.HasIndex(e => e.Celular, "IX_Usuarios_Celular")
                .IsUnique()
                .HasFilter("([Celular] IS NOT NULL)");

            entity.HasIndex(e => e.Cuc, "IX_Usuarios_Cuc").IsUnique();

            entity.HasIndex(e => e.Email, "IX_Usuarios_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Telefono, "IX_Usuarios_Telefono")
                .IsUnique()
                .HasFilter("([Telefono] IS NOT NULL)");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleFin)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleInicio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Cuc)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(180)
                .IsUnicode(false);
            entity.Property(e => e.FechaAcceso).HasColumnType("datetime");
            entity.Property(e => e.FechaEnvKit).HasColumnType("datetime");
            entity.Property(e => e.FechaInscripcion).HasColumnType("datetime");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.NumeroExterior)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NumeroInterior)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Referencias)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Sesion)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TokenMovil)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCediNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdCedi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Cedis_IdCedi");

            entity.HasOne(d => d.IdColoniaNavigation).WithMany(p => p.Usuarios).HasForeignKey(d => d.IdColonia);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.UsuarioIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.UsuarioIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProgramaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdPrograma)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdRutaNavigation).WithMany(p => p.Usuarios).HasForeignKey(d => d.IdRuta);

            entity.HasOne(d => d.IdSupervisorNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSupervisor)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Vista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vistas__3214EC075E09FA43");

            entity.HasIndex(e => e.Nombre, "UQ_Vistas_Nombre").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Zonas__3214EC07E62FF3E6");

            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmbotelladoraNavigation).WithMany(p => p.Zonas)
                .HasForeignKey(d => d.IdEmbotelladora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zonas_Embotelladoras");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
