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

    public virtual DbSet<CanalesDeVentum> CanalesDeVenta { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Carrusel> Carrusels { get; set; }

    public virtual DbSet<CatalogoCorreo> CatalogoCorreos { get; set; }

    public virtual DbSet<CategoriasDePremio> CategoriasDePremios { get; set; }

    public virtual DbSet<CategoriasDeProducto> CategoriasDeProductos { get; set; }

    public virtual DbSet<CategoriasLlamadum> CategoriasLlamada { get; set; }

    public virtual DbSet<Cedi> Cedis { get; set; }

    public virtual DbSet<CodigosMensaje> CodigosMensajes { get; set; }

    public virtual DbSet<Colonia> Colonias { get; set; }

    public virtual DbSet<ConceptosDeAcumulacion> ConceptosDeAcumulacions { get; set; }

    public virtual DbSet<CumplimientosPortafolio> CumplimientosPortafolios { get; set; }

    public virtual DbSet<CuotasDeCompra> CuotasDeCompras { get; set; }

    public virtual DbSet<DetalleDeMetaDeCompra> DetalleDeMetaDeCompras { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<DetallesDePortafolio> DetallesDePortafolios { get; set; }

    public virtual DbSet<Embotelladora> Embotelladoras { get; set; }

    public virtual DbSet<Empaque> Empaques { get; set; }

    public virtual DbSet<EmpaquesProducto> EmpaquesProductos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<EstatusDeCarrito> EstatusDeCarritos { get; set; }

    public virtual DbSet<EstatusDeLlamadum> EstatusDeLlamada { get; set; }

    public virtual DbSet<EstatusDeRedencione> EstatusDeRedenciones { get; set; }

    public virtual DbSet<EvaluacionesAcumulacion> EvaluacionesAcumulacions { get; set; }

    public virtual DbSet<FuerzasDeVentaNegocio> FuerzasDeVentaNegocios { get; set; }

    public virtual DbSet<FuerzasDeVentum> FuerzasDeVenta { get; set; }

    public virtual DbSet<HistoricoVenta> HistoricoVentas { get; set; }

    public virtual DbSet<ImagenesPromocione> ImagenesPromociones { get; set; }

    public virtual DbSet<LayoutCompra> LayoutCompras { get; set; }

    public virtual DbSet<Llamada> Llamadas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Mensajeria> Mensajerias { get; set; }

    public virtual DbSet<MetasMensuale> MetasMensuales { get; set; }

    public virtual DbSet<MetodosDeEntrega> MetodosDeEntregas { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Negocio> Negocios { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Origene> Origenes { get; set; }

    public virtual DbSet<OrigenesVentum> OrigenesVenta { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<PasswordUsuario> PasswordUsuarios { get; set; }

    public virtual DbSet<Periodo> Periodos { get; set; }

    public virtual DbSet<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; }

    public virtual DbSet<Premio> Premios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductosSelecto> ProductosSelectos { get; set; }

    public virtual DbSet<Programa> Programas { get; set; }

    public virtual DbSet<PuntajesAcumulacion> PuntajesAcumulacions { get; set; }

    public virtual DbSet<Redencione> Redenciones { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ruta> Rutas { get; set; }

    public virtual DbSet<Saldo> Saldos { get; set; }

    public virtual DbSet<Seccione> Secciones { get; set; }

    public virtual DbSet<SeccionesPorRol> SeccionesPorRols { get; set; }

    public virtual DbSet<SegmentosAcumulacion> SegmentosAcumulacions { get; set; }

    public virtual DbSet<SeguimientoDeRedencione> SeguimientoDeRedenciones { get; set; }

    public virtual DbSet<SubcanalesDeVentum> SubcanalesDeVenta { get; set; }

    public virtual DbSet<SubcategoriasLlamadum> SubcategoriasLlamada { get; set; }

    public virtual DbSet<SubconceptosDeAcumulacion> SubconceptosDeAcumulacions { get; set; }

    public virtual DbSet<SuborigenesVentum> SuborigenesVenta { get; set; }

    public virtual DbSet<Supervisore> Supervisores { get; set; }

    public virtual DbSet<TiposDeAccione> TiposDeAcciones { get; set; }

    public virtual DbSet<TiposDeArchivoDeCarga> TiposDeArchivoDeCargas { get; set; }

    public virtual DbSet<TiposDeEnvio> TiposDeEnvios { get; set; }

    public virtual DbSet<TiposDeMovimiento> TiposDeMovimientos { get; set; }

    public virtual DbSet<TiposDeOperacion> TiposDeOperacions { get; set; }

    public virtual DbSet<TiposLlamadum> TiposLlamada { get; set; }

    public virtual DbSet<UrlShortener> UrlShorteners { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

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

            entity.HasOne(d => d.IdOperadorAftdNavigation).WithMany(p => p.BitacoraDeOperadoreIdOperadorAftdNavigations)
                .HasForeignKey(d => d.IdOperadorAftd)
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

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraDeUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeUsuarios_Usuarios");
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

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraEnvioCorreos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_BitacoraEnvioCorreos_Usuarios");
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

        modelBuilder.Entity<CanalesDeVentum>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
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

        modelBuilder.Entity<CategoriasDePremio>(entity =>
        {
            entity.HasIndex(e => new { e.Nombre, e.Digital, e.IdEstatus }, "UQ_CategoriasDePremios_Nomnbre_Digital_IdEstatus").IsUnique();

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
            entity.Property(e => e.FondoColor)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Imgurl)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.LetraColor)
                .HasMaxLength(10)
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

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CategoriasDeProductos)
                .HasForeignKey(d => d.IdEstatus)
                .HasConstraintName("FK_CategoriasDeProducto_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.CategoriasDeProductoIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.CategoriasDeProductoIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdSubconceptoDeAcumulacionNavigation).WithMany(p => p.CategoriasDeProductos)
                .HasForeignKey(d => d.IdSubconceptoDeAcumulacion)
                .HasConstraintName("FK_CategoriasDeProducto_SubconceptosDeAcumulacion");
        });

        modelBuilder.Entity<CategoriasLlamadum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CategoriasDeLlamada");

            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.CategoriasLlamada)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoriasDeLlamada_Estatus");
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

        modelBuilder.Entity<ConceptosDeAcumulacion>(entity =>
        {
            entity.ToTable("ConceptosDeAcumulacion");

            entity.HasIndex(e => new { e.IdCanal, e.Codigo }, "UQ_ConceptosDeAcumulacion_IdCanal_Codigo").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.ConceptosDeAcumulacions)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConceptosDeAcumulacion_Canales");

            entity.HasOne(d => d.IdTipoDeMovimientoNavigation).WithMany(p => p.ConceptosDeAcumulacions)
                .HasForeignKey(d => d.IdTipoDeMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConceptosDeAcumulacion_TiposDeMovimientos");
        });

        modelBuilder.Entity<CumplimientosPortafolio>(entity =>
        {
            entity.ToTable("CumplimientosPortafolio");

            entity.HasIndex(e => new { e.IdUsuario, e.IdEmpaque }, "UQ_CumplimientosPortafolio_IdUsuario_IdEmpaque").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEmpaqueNavigation).WithMany(p => p.CumplimientosPortafolios)
                .HasForeignKey(d => d.IdEmpaque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CumplimientosPortafolio_Empaques");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CumplimientosPortafolios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CumplimientosPortafolio_Usuarios");
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

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.Property(e => e.Cf).HasColumnName("CF");
            entity.Property(e => e.Cu).HasColumnName("CU");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Importe).HasColumnType("decimal(24, 10)");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.DetalleVentaIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.DetalleVentaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Productos");

            entity.HasOne(d => d.IdSuborigenVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdSuborigenVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_SuborigenesVenta");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Ventas");
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

        modelBuilder.Entity<Empaque>(entity =>
        {
            entity.HasIndex(e => new { e.IdPeriodo, e.IdSegAcumulacion, e.Nombre }, "UQ_Empaques_IdPeriodo_IdSegAcumulacion_Nombre").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Empaques)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empaques_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.EmpaqueIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.EmpaqueIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Empaques)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empaques_Periodos");

            entity.HasOne(d => d.IdSegAcumulacionNavigation).WithMany(p => p.Empaques)
                .HasForeignKey(d => d.IdSegAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empaques_SegmentosAcumulacion");
        });

        modelBuilder.Entity<EmpaquesProducto>(entity =>
        {
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEmpaqueNavigation).WithMany(p => p.EmpaquesProductos)
                .HasForeignKey(d => d.IdEmpaque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpaquesProductos_Empaques");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.EmpaquesProductos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpaquesProductos_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.EmpaquesProductoIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.EmpaquesProductoIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.EmpaquesProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpaquesProductos_Productos");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .HasColumnName("Estado");
            entity.Property(e => e.Fechareg).HasColumnType("datetime");
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

        modelBuilder.Entity<EvaluacionesAcumulacion>(entity =>
        {
            entity.ToTable("EvaluacionesAcumulacion");

            entity.HasIndex(e => new { e.IdSda, e.IdPeriodo, e.IdUsuario }, "UQ_EvaluacionesAcumulacion_IdSubcptoAcumulacon_IdPeriodo_IdUsuario").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdSda).HasColumnName("IdSDA");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.EvaluacionesAcumulacionIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.EvaluacionesAcumulacionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.EvaluacionesAcumulacions)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvaluacionesAcumulacion_Periodos");

            entity.HasOne(d => d.IdSdaNavigation).WithMany(p => p.EvaluacionesAcumulacions)
                .HasForeignKey(d => d.IdSda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvaluacionesAcumulacion_SubconceptosDeAcumulacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.EvaluacionesAcumulacions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvaluacionesAcumulacion_Usuarios");
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

        modelBuilder.Entity<HistoricoVenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HistoricoCompras");

            entity.HasIndex(e => new { e.IdPeriodo, e.IdUsuario }, "UQ_HistoricoVentas_IdPeriodo_IdUsuario").IsUnique();

            entity.Property(e => e.Cf).HasColumnName("CF");
            entity.Property(e => e.Cu).HasColumnName("CU");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Importe).HasColumnType("decimal(24, 10)");

            entity.HasOne(d => d.IdArchivoDeCargaNavigation).WithMany(p => p.HistoricoVenta)
                .HasForeignKey(d => d.IdArchivoDeCarga)
                .HasConstraintName("FK_HistoricoCompras_ArchivosDeCarga");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.HistoricoVentaIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_HistoricoCompras_Operadores_IdOperadorMod");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.HistoricoVentaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricoCompras_Operadores_IdOperadorReg");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.HistoricoVenta)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricoCompras_Periodos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistoricoVenta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricoCompras_Usuarios");
        });

        modelBuilder.Entity<ImagenesPromocione>(entity =>
        {
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(160)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.ImagenesPromociones)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Canales");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.ImagenesPromociones)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.ImagenesPromocioneIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Operadores1");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ImagenesPromocioneIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Operadores");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.ImagenesPromociones)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Periodos");
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

        modelBuilder.Entity<Llamada>(entity =>
        {
            entity.Property(e => e.Comentario)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Tema)
                .HasMaxLength(510)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusLlamadaNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdEstatusLlamada)
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

            entity.HasOne(d => d.IdSubcategoriaLlamadaNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdSubcategoriaLlamada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LLamadas_SubcategoriasLlamada");

            entity.HasOne(d => d.IdTipoLlamadaNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdTipoLlamada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Llamadas_TiposDeLlamada");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Llamada)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Llamadas_Usuarios");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.MarcaIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.MarcaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mensajeria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mensajer__3214EC0703F6CE31");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MetasMensuale>(entity =>
        {
            entity.HasIndex(e => new { e.IdUsuario, e.IdPeriodo }, "UQ_MetasMensuales_IdUsuario_IdPeriodo").IsUnique();

            entity.Property(e => e.CompraDigital).HasColumnType("money");
            entity.Property(e => e.CompraPreventa).HasColumnType("money");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.ImporteComprado).HasColumnType("money");
            entity.Property(e => e.Meta).HasColumnType("money");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.MetasMensualeIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.MetasMensualeIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.MetasMensuales)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MetasMensuales_Periodos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MetasMensuales)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MetasMensuales_Usuarios");
        });

        modelBuilder.Entity<MetodosDeEntrega>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MetodosD__3214EC07F893A792");

            entity.ToTable("MetodosDeEntrega");

            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.Property(e => e.Cantidad).HasColumnType("decimal(11, 4)");
            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.IdSda).HasColumnName("IdSDA");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK_Movimientos_Operadores");

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Origenes");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Periodos");

            entity.HasOne(d => d.IdProgramaNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdPrograma)
                .HasConstraintName("FK_Movimientos_Programas");

            entity.HasOne(d => d.IdSdaNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdSda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_SubconceptosDeAcumulacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Usuarios");
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
            entity.Property(e => e.Password).HasMaxLength(250);
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

        modelBuilder.Entity<OrigenesVentum>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_OrigenesVenta_Nombre").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.OrigenesVenta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrigenesVenta_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.OrigenesVentumIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_OrigenesVenta_Operadore_IdOperadorMod");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.OrigenesVentumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrigenesVenta_Operadore_IdOperadorReg");
        });

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.HasIndex(e => e.Tag, "IX_Parametros").IsUnique();

            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Tag)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.ParametroIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_Parametros_Operadores1");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ParametroIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parametros_Operadores");
        });

        modelBuilder.Entity<PasswordUsuario>(entity =>
        {
            entity.HasIndex(e => e.Cuc, "IX_PasswordUsuarios_Cuc").IsUnique();

            entity.Property(e => e.Cuc)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_Peridos_Nombre").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PorcentajesIncrementoVentum>(entity =>
        {
            entity.HasIndex(e => new { e.IdCanal, e.IdZona, e.IdPeriodo }, "UQ_PorcentajesIncrementoVenta_IdCanal_IdZona_IdPeriodo").IsUnique();

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.PorcentajesIncrementoVenta)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PorcentajesIncrementoVenta_Canales");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.PorcentajesIncrementoVenta)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PorcentajesIncrementoVenta_Periodos");

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.PorcentajesIncrementoVenta)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PorcentajesIncrementoVenta_Zonas");
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
            entity.Property(e => e.TyC).IsUnicode(false);

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

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
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
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Unidades).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Estatus");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Marcas");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.ProductoIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ProductoIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ProductosSelecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07CDCDBB5C");

            entity.HasIndex(e => new { e.IdSubconceptoDeAcumulacion, e.IdProducto }, "IX_ProductosSelectos_IdSubconceptoDeAcumulacion_IdProducto").IsUnique();
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

        modelBuilder.Entity<PuntajesAcumulacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puntajes__3214EC074BC36C0F");

            entity.ToTable("PuntajesAcumulacion");

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdSda).HasColumnName("IdSDA");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.PuntajesAcumulacionIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK__PuntajesD__IdOpe__75C27486");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.PuntajesAcumulacionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PuntajesD__IdOpe__74CE504D");

            entity.HasOne(d => d.IdSdaNavigation).WithMany(p => p.PuntajesAcumulacions)
                .HasForeignKey(d => d.IdSda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PuntajesD__IdSub__71F1E3A2");
        });

        modelBuilder.Entity<Redencione>(entity =>
        {
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

            entity.HasOne(d => d.IdMensajeriaNavigation).WithMany(p => p.Redenciones).HasForeignKey(d => d.IdMensajeria);

            entity.HasOne(d => d.IdMovimientoNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Redenciones_Movimientos");

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

        modelBuilder.Entity<SegmentosAcumulacion>(entity =>
        {
            entity.ToTable("SegmentosAcumulacion");

            entity.HasIndex(e => new { e.IdSda, e.Nombre }, "IX_SegmentosAcumulacion_IdSubcptoAcumulacon_Nombre").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdSda).HasColumnName("IdSDA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.SegmentosAcumulacionIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.SegmentosAcumulacionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdSdaNavigation).WithMany(p => p.SegmentosAcumulacions)
                .HasForeignKey(d => d.IdSda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SegmentosAcumulacion_SubconceptosDeAcumulacion");
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

        modelBuilder.Entity<SubcanalesDeVentum>(entity =>
        {
            entity.Property(e => e.IdCdv).HasColumnName("IdCDV");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCdvNavigation).WithMany(p => p.SubcanalesDeVenta)
                .HasForeignKey(d => d.IdCdv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubcanalesDeVenta_SubcanalesDeVenta");
        });

        modelBuilder.Entity<SubcategoriasLlamadum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subcateg__3214EC075F3B5B32");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaLlamadaNavigation).WithMany(p => p.SubcategoriasLlamada)
                .HasForeignKey(d => d.IdCategoriaLlamada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubcategoriasLlamada_CategoriasLlamada");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.SubcategoriasLlamada)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubcategoriasLlamada_Estatus");
        });

        modelBuilder.Entity<SubconceptosDeAcumulacion>(entity =>
        {
            entity.ToTable("SubconceptosDeAcumulacion");

            entity.HasIndex(e => new { e.IdConceptoDeAcumulacion, e.Nombre }, "IX_SubconceptosDeAcumulacion_IdConceptoDeAcumulacion_Nombre").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FondoColor)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LetraColor)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConceptoDeAcumulacionNavigation).WithMany(p => p.SubconceptosDeAcumulacions)
                .HasForeignKey(d => d.IdConceptoDeAcumulacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubconceptosDeAcumulacion_ConceptosDeAcumulacion");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.SubconceptosDeAcumulacionIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.SubconceptosDeAcumulacionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .HasConstraintName("FK_SubconceptosDeAcumulacion_Operadores");
        });

        modelBuilder.Entity<SuborigenesVentum>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_SuborigenesVenta_Nombre").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.SuborigenesVenta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SuborigenesVenta_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.SuborigenesVentumIdOperadorModNavigations)
                .HasForeignKey(d => d.IdOperadorMod)
                .HasConstraintName("FK_SuborigenesVenta_Operadore_IdOperadorMod");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.SuborigenesVentumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SuborigenesVenta_Operadore_IdOperadorReg");

            entity.HasOne(d => d.IdOrigenVentaNavigation).WithMany(p => p.SuborigenesVenta)
                .HasForeignKey(d => d.IdOrigenVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SuborigenesVenta_OrigenesVenta");
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

        modelBuilder.Entity<TiposLlamadum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TiposDeLlamada");

            entity.Property(e => e.Nombre).HasMaxLength(35);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.TiposLlamada)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TiposDeLlamada_Estatus");
        });

        modelBuilder.Entity<UrlShortener>(entity =>
        {
            entity.HasIndex(e => e.Clave, "UrlShorteners_Clave");

            entity.Property(e => e.Clave)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OriginalUrl).IsUnicode(false);
            entity.Property(e => e.ShortUrl)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.UrlShorteners)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UrlShorteners_Estatus");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC078E1CCBB3");

            entity.HasIndex(e => e.Cuc, "IX_Usuarios_Cuc").IsUnique();

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
            entity.Property(e => e.IdScdv).HasColumnName("IdSCDV");
            entity.Property(e => e.JefeDeVenta)
                .HasMaxLength(120)
                .IsUnicode(false);
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
            entity.Property(e => e.Supervisor)
                .HasMaxLength(120)
                .IsUnicode(false);
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

            entity.HasOne(d => d.IdScdvNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdScdv)
                .HasConstraintName("FK_Usuarios_SubcanalesDeVenta");

            entity.HasOne(d => d.IdSupervisorNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSupervisor)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVenta).HasColumnType("datetime");

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Canales");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.VentaIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.VentaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_IdPeriodo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Usuarios");
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
