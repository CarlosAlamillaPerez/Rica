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

    public virtual DbSet<BitacoraDeEncuestum> BitacoraDeEncuesta { get; set; }

    public virtual DbSet<BitacoraDeOperadore> BitacoraDeOperadores { get; set; }

    public virtual DbSet<BitacoraDeUsuario> BitacoraDeUsuarios { get; set; }

    public virtual DbSet<BitacoraDeWhatsApp> BitacoraDeWhatsApps { get; set; }

    public virtual DbSet<BitacoraEnvioCorreo> BitacoraEnvioCorreos { get; set; }

    public virtual DbSet<BitacoraFuerzaVentum> BitacoraFuerzaVenta { get; set; }

    public virtual DbSet<BitacoraPushNotificacione> BitacoraPushNotificaciones { get; set; }

    public virtual DbSet<Canale> Canales { get; set; }

    public virtual DbSet<CanalesDeVentum> CanalesDeVenta { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Carrusel> Carrusels { get; set; }

    public virtual DbSet<CatalogoCorreo> CatalogoCorreos { get; set; }

    public virtual DbSet<CatalogoPushNotificacione> CatalogoPushNotificaciones { get; set; }

    public virtual DbSet<CategoriasDePremio> CategoriasDePremios { get; set; }

    public virtual DbSet<CategoriasDeProducto> CategoriasDeProductos { get; set; }

    public virtual DbSet<CategoriasLlamadum> CategoriasLlamada { get; set; }

    public virtual DbSet<Cedi> Cedis { get; set; }

    public virtual DbSet<CodigosMensaje> CodigosMensajes { get; set; }

    public virtual DbSet<CodigosRedimido> CodigosRedimidos { get; set; }

    public virtual DbSet<Colonia> Colonias { get; set; }

    public virtual DbSet<ConceptosDeAcumulacion> ConceptosDeAcumulacions { get; set; }

    public virtual DbSet<Contactano> Contactanos { get; set; }

    public virtual DbSet<CumplimientosPortafolio> CumplimientosPortafolios { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Embotelladora> Embotelladoras { get; set; }

    public virtual DbSet<Empaque> Empaques { get; set; }

    public virtual DbSet<EmpaquesProducto> EmpaquesProductos { get; set; }

    public virtual DbSet<Encuesta> Encuestas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<EstatusDeCarrito> EstatusDeCarritos { get; set; }

    public virtual DbSet<EstatusDeLlamadum> EstatusDeLlamada { get; set; }

    public virtual DbSet<EstatusDePushNotificacione> EstatusDePushNotificaciones { get; set; }

    public virtual DbSet<EstatusDeRedencione> EstatusDeRedenciones { get; set; }

    public virtual DbSet<EstatusPago> EstatusPagos { get; set; }

    public virtual DbSet<EvaluacionesAcumulacion> EvaluacionesAcumulacions { get; set; }

    public virtual DbSet<FuerzaVentum> FuerzaVenta { get; set; }

    public virtual DbSet<HistorialCompraPunto> HistorialCompraPuntos { get; set; }

    public virtual DbSet<HistoricoDeCortesCuentum> HistoricoDeCortesCuenta { get; set; }

    public virtual DbSet<HistoricoVenta> HistoricoVentas { get; set; }

    public virtual DbSet<ImagenesPromocione> ImagenesPromociones { get; set; }

    public virtual DbSet<JefesVentum> JefesVenta { get; set; }

    public virtual DbSet<Llamada> Llamadas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Mensajeria> Mensajerias { get; set; }

    public virtual DbSet<MetasMensuale> MetasMensuales { get; set; }

    public virtual DbSet<MetodosDeEntrega> MetodosDeEntregas { get; set; }

    public virtual DbSet<MotivosContactano> MotivosContactanos { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<OpcionesPreguntum> OpcionesPregunta { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Origene> Origenes { get; set; }

    public virtual DbSet<OrigenesVentum> OrigenesVenta { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<PasswordUsuario> PasswordUsuarios { get; set; }

    public virtual DbSet<Periodo> Periodos { get; set; }

    public virtual DbSet<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; }

    public virtual DbSet<PortafolioAvance> PortafolioAvances { get; set; }

    public virtual DbSet<PrefijosRm> PrefijosRms { get; set; }

    public virtual DbSet<PreguntasEncuestum> PreguntasEncuesta { get; set; }

    public virtual DbSet<Premio> Premios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductosSelecto> ProductosSelectos { get; set; }

    public virtual DbSet<Programa> Programas { get; set; }

    public virtual DbSet<PuntajesAcumulacion> PuntajesAcumulacions { get; set; }

    public virtual DbSet<Redencione> Redenciones { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<RespuestaEsperadum> RespuestaEsperada { get; set; }

    public virtual DbSet<RespuestasEncuestum> RespuestasEncuesta { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesFdv> RolesFdvs { get; set; }

    public virtual DbSet<Ruta> Rutas { get; set; }

    public virtual DbSet<Seccione> Secciones { get; set; }

    public virtual DbSet<SeccionesPorRol> SeccionesPorRols { get; set; }

    public virtual DbSet<SegmentosAcumulacion> SegmentosAcumulacions { get; set; }

    public virtual DbSet<SeguimientoDeRedencione> SeguimientoDeRedenciones { get; set; }

    public virtual DbSet<SeguimientoVista> SeguimientoVistas { get; set; }

    public virtual DbSet<SubcanalesDeVentum> SubcanalesDeVenta { get; set; }

    public virtual DbSet<SubcategoriasLlamadum> SubcategoriasLlamada { get; set; }

    public virtual DbSet<SubconceptosDeAcumulacion> SubconceptosDeAcumulacions { get; set; }

    public virtual DbSet<SuborigenesVentum> SuborigenesVenta { get; set; }

    public virtual DbSet<Supervisore> Supervisores { get; set; }

    public virtual DbSet<Tamanio> Tamanios { get; set; }

    public virtual DbSet<Tarjeta> Tarjetas { get; set; }

    public virtual DbSet<TiposControlPreguntum> TiposControlPregunta { get; set; }

    public virtual DbSet<TiposDeAccione> TiposDeAcciones { get; set; }

    public virtual DbSet<TiposDeArchivoDeCarga> TiposDeArchivoDeCargas { get; set; }

    public virtual DbSet<TiposDeEnvio> TiposDeEnvios { get; set; }

    public virtual DbSet<TiposDeMovimiento> TiposDeMovimientos { get; set; }

    public virtual DbSet<TiposDeOperacion> TiposDeOperacions { get; set; }

    public virtual DbSet<TiposDePremio> TiposDePremios { get; set; }

    public virtual DbSet<TiposDeTransaccion> TiposDeTransaccions { get; set; }

    public virtual DbSet<TiposLlamadum> TiposLlamada { get; set; }

    public virtual DbSet<TiposPago> TiposPagos { get; set; }

    public virtual DbSet<TiposPreguntum> TiposPregunta { get; set; }

    public virtual DbSet<TiposWhatsApp> TiposWhatsApps { get; set; }

    public virtual DbSet<UrlShortener> UrlShorteners { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosPrueba> UsuariosPruebas { get; set; }

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

        modelBuilder.Entity<BitacoraDeEncuestum>(entity =>
        {
            entity.HasIndex(e => new { e.IdEncuesta, e.IdUsuario }, "IX_BitacoraDeEncuesta_IdEncuesta_IdUsuario");

            entity.HasIndex(e => e.Token, "IX_BitacoraDeEncuesta_Token").IsUnique();

            entity.Property(e => e.FechaFinRespuesta).HasColumnType("datetime");
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.FechaInicioRespuesta).HasColumnType("datetime");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaRespuestaEsperada).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.BitacoraDeEncuesta)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeEncuesta_Encuestas");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.BitacoraDeEncuesta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeEncuesta_Estatus");

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.BitacoraDeEncuestumIdOperadorNavigations)
                .HasForeignKey(d => d.IdOperador)
                .HasConstraintName("FK_BitacoraDeEncuesta_Operadores");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.BitacoraDeEncuestumIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraDeEncuestumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraDeEncuesta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_BitacoraDeEncuesta_Usuarios");
        });

        modelBuilder.Entity<BitacoraDeOperadore>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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

        modelBuilder.Entity<BitacoraDeWhatsApp>(entity =>
        {
            entity.ToTable("BitacoraDeWhatsApp");

            entity.Property(e => e.Celular)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EstatusApi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.JsonRequest).IsUnicode(false);
            entity.Property(e => e.JsonResponse).IsUnicode(false);
            entity.Property(e => e.Mensaje).IsUnicode(false);
            entity.Property(e => e.Parametros)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.BitacoraDeWhatsApps)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeWhatsApp_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.BitacoraDeWhatsAppIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraDeWhatsAppIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.BitacoraDeWhatsApps)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeWhatsApp_Periodos");

            entity.HasOne(d => d.IdTipoWhatsAppNavigation).WithMany(p => p.BitacoraDeWhatsApps)
                .HasForeignKey(d => d.IdTipoWhatsApp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeWhatsApp_TiposWhatsApp");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraDeWhatsApps)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraDeWhatsApp_Usuarios");
        });

        modelBuilder.Entity<BitacoraEnvioCorreo>(entity =>
        {
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
                .HasConstraintName("FK_BitacoraEnvioCorreos_Estatus");

            entity.HasOne(d => d.IdOperadorNavigation).WithMany(p => p.BitacoraEnvioCorreoIdOperadorNavigations)
                .HasForeignKey(d => d.IdOperador)
                .HasConstraintName("FK_BitacoraEnvioCorreos_Operadores");

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraEnvioCorreoIdOperadorRegNavigations).HasForeignKey(d => d.IdOperadorReg);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraEnvioCorreos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_BitacoraEnvioCorreos_Usuarios");
        });

        modelBuilder.Entity<BitacoraFuerzaVentum>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdFdv).HasColumnName("IdFDV");
            entity.Property(e => e.Notas)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdFdvNavigation).WithMany(p => p.BitacoraFuerzaVenta)
                .HasForeignKey(d => d.IdFdv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraFuerzaVenta_FuerzaVenta");

            entity.HasOne(d => d.IdTipoDeOperacionNavigation).WithMany(p => p.BitacoraFuerzaVenta)
                .HasForeignKey(d => d.IdTipoDeOperacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraFuerzaVenta_TiposDeOperacion");

            entity.HasOne(d => d.IdUsuarioAftdNavigation).WithMany(p => p.BitacoraFuerzaVenta)
                .HasForeignKey(d => d.IdUsuarioAftd)
                .HasConstraintName("FK_BitacoraFuerzaVenta_Usuarios");
        });

        modelBuilder.Entity<BitacoraPushNotificacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bitacora_PushNotificaciones");

            entity.Property(e => e.Codigo).HasMaxLength(255);
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.FechaLectura).HasColumnType("datetime");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Icon).HasMaxLength(255);
            entity.Property(e => e.ImgAndroid).HasMaxLength(255);
            entity.Property(e => e.ImgIos)
                .HasMaxLength(255)
                .HasColumnName("ImgIOS");
            entity.Property(e => e.Seccion).HasMaxLength(255);
            entity.Property(e => e.TextoEn)
                .HasMaxLength(1000)
                .HasColumnName("TextoEN");
            entity.Property(e => e.TextoEs)
                .HasMaxLength(1000)
                .HasColumnName("TextoES");
            entity.Property(e => e.TituloEn)
                .HasMaxLength(255)
                .HasColumnName("TituloEN");
            entity.Property(e => e.TituloEs)
                .HasMaxLength(255)
                .HasColumnName("TituloES");
            entity.Property(e => e.UrlLink).HasMaxLength(255);

            entity.HasOne(d => d.IdCatPushNotificacionNavigation).WithMany(p => p.BitacoraPushNotificaciones)
                .HasForeignKey(d => d.IdCatPushNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bitacora_PushNotificaciones_Catalogo_PushNotificaciones");

            entity.HasOne(d => d.IdEstatusPushNotificacionNavigation).WithMany(p => p.BitacoraPushNotificaciones)
                .HasForeignKey(d => d.IdEstatusPushNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bitacora_PushNotificaciones_EstatusDePushNotificaciones");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.BitacoraPushNotificacioneIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.BitacoraPushNotificacioneIdOperadorRegNavigations).HasForeignKey(d => d.IdOperadorReg);

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.BitacoraPushNotificaciones)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraPushNotificaciones_Origenes");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.BitacoraPushNotificaciones)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BitacoraPushNotificaciones_Periodos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.BitacoraPushNotificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bitacora_PushNotificaciones_Usuarios");
        });

        modelBuilder.Entity<Canale>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            // Agregar configuración para UriBase
            entity.Property(e => e.UriBase)
                .HasMaxLength(120)
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

            entity.HasOne(d => d.IdTarjetaNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdTarjeta)
                .HasConstraintName("FK_Carrito_Tarjetas");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrito_Usuarios");
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
        });

        modelBuilder.Entity<CatalogoCorreo>(entity =>
        {
            entity.Property(e => e.Cco)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Codigo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Comentarios)
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

        modelBuilder.Entity<CatalogoPushNotificacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Catalogo_PushNotificaciones");

            entity.HasIndex(e => new { e.IdCanal, e.Codigo }, "UQ_CatalogoPushNotificaciones_IdCanal_Codigo").IsUnique();

            entity.Property(e => e.Codigo).HasMaxLength(255);
            entity.Property(e => e.Icon).HasMaxLength(255);
            entity.Property(e => e.ImgAndroid).HasMaxLength(255);
            entity.Property(e => e.ImgIos)
                .HasMaxLength(255)
                .HasColumnName("ImgIOS");
            entity.Property(e => e.Seccion).HasMaxLength(255);
            entity.Property(e => e.TextoEn)
                .HasMaxLength(1000)
                .HasColumnName("TextoEN");
            entity.Property(e => e.TextoEs)
                .HasMaxLength(1000)
                .HasColumnName("TextoES");
            entity.Property(e => e.TituloEn)
                .HasMaxLength(255)
                .HasColumnName("TituloEN");
            entity.Property(e => e.TituloEs)
                .HasMaxLength(255)
                .HasColumnName("TituloES");
            entity.Property(e => e.UrlLink).HasMaxLength(255);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.CatalogoPushNotificaciones)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Catalogo_PushNotificaciones_Canales");
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

        modelBuilder.Entity<CodigosRedimido>(entity =>
        {
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Folio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FolioRms)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FolioRMS");
            entity.Property(e => e.Motivo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Pin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoRecarga)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.CodigosRedimidos)
                .HasForeignKey(d => d.IdCarrito)
                .HasConstraintName("FK_CodigosRedimidos_Carrito");

            entity.HasOne(d => d.IdRedencionNavigation).WithMany(p => p.CodigosRedimidos)
                .HasForeignKey(d => d.IdRedencion)
                .HasConstraintName("FK_CodigosRedimidos_Redenciones");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CodigosRedimidos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodigosRedimidos_Usuarios");
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

        modelBuilder.Entity<Contactano>(entity =>
        {
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasMaxLength(1000);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Contactanos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contactanos_Estatus");

            entity.HasOne(d => d.IdMotivoContactanosNavigation).WithMany(p => p.Contactanos)
                .HasForeignKey(d => d.IdMotivoContactanos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contactanos_MotivosContactanos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Contactanos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contactanos_Usuarios");
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

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trg_Act_Empaques"));

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
            entity.Property(e => e.Imagen)
                .HasMaxLength(250)
                .IsUnicode(false);
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

        modelBuilder.Entity<Encuesta>(entity =>
        {
            entity.HasIndex(e => new { e.IdCanal, e.Codigo }, "UQ_Encuestas_IdCanal_Codigo").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MensajeEnvio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Tema)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
            entity.Property(e => e.Vista)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.Encuesta)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encuestas_Canales");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Encuesta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encuestas_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.EncuestaIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.EncuestaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

        modelBuilder.Entity<EstatusDePushNotificacione>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(80);
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

        modelBuilder.Entity<EstatusPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EstatusCompraPuntos");

            entity.ToTable("EstatusPago");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EvaluacionesAcumulacion>(entity =>
        {
            entity.ToTable("EvaluacionesAcumulacion");

            entity.HasIndex(e => new { e.IdSda, e.IdPeriodo, e.IdUsuario }, "UQ_EvaluacionesAcumulacion_IdSubcptoAcumulacion_IdPeriodo_IdUsuario").IsUnique();

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

        modelBuilder.Entity<FuerzaVentum>(entity =>
        {
            entity.HasIndex(e => new { e.IdCanal, e.Usuario }, "IX_FuerzaVenta_IdCanal_Usuario").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdRolFdv).HasColumnName("IdRolFDV");
            entity.Property(e => e.SesionId)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.FuerzaVenta)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FuerzaVenta_Canales");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.FuerzaVenta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FuerzaVenta_IdEstatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.FuerzaVentumIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.FuerzaVentumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdRolFdvNavigation).WithMany(p => p.FuerzaVenta)
                .HasForeignKey(d => d.IdRolFdv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FuerzaVenta_RolesFDV");
        });

        modelBuilder.Entity<HistorialCompraPunto>(entity =>
        {
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleFin)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleInicio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdOpenPay).HasMaxLength(200);
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NumeroExterior)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NumeroInterior)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Referencia).HasMaxLength(200);
            entity.Property(e => e.Referencias)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoAlterno)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdColoniaNavigation).WithMany(p => p.HistorialCompraPuntos)
                .HasForeignKey(d => d.IdColonia)
                .HasConstraintName("FK_HistorialCompraPuntos_Colonias");

            entity.HasOne(d => d.IdEstatusPagoNavigation).WithMany(p => p.HistorialCompraPuntos)
                .HasForeignKey(d => d.IdEstatusPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialCompraPuntos_EstatusPago");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.HistorialCompraPuntoIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.HistorialCompraPuntoIdOperadorRegNavigations).HasForeignKey(d => d.IdOperadorReg);

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.HistorialCompraPuntos)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialCompraPuntos_Origenes");

            entity.HasOne(d => d.IdTipoPagoNavigation).WithMany(p => p.HistorialCompraPuntos)
                .HasForeignKey(d => d.IdTipoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialCompraPuntos_TiposPago");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialCompraPuntos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialCompraPuntos_Usuarios");
        });

        modelBuilder.Entity<HistoricoDeCortesCuentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HistoricoCortesCuenta");

            entity.HasIndex(e => new { e.IdPeriodo, e.IdUsuario }, "UQ_HistoricoCortesCuenta_IdPeriodo_IdUsuario").IsUnique();

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.HistoricoDeCortesCuenta)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricoCortesCuenta_Periodos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistoricoDeCortesCuenta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricoCortesCuenta_Usuarios");
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
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(160)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(10)
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

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.ImagenesPromocioneIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.ImagenesPromocioneIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.ImagenesPromociones)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenesPromociones_Periodos");
        });

        modelBuilder.Entity<JefesVentum>(entity =>
        {
            entity.Property(e => e.IdFdv).HasColumnName("IdFDV");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdFdvNavigation).WithMany(p => p.JefesVenta)
                .HasForeignKey(d => d.IdFdv)
                .HasConstraintName("FK_JefesVenta_FuerzaVenta");
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

            entity.Property(e => e.Clave)
                .HasMaxLength(5)
                .IsUnicode(false);
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

        modelBuilder.Entity<MotivosContactano>(entity =>
        {
            entity.Property(e => e.Motivo).HasMaxLength(255);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.MotivosContactanos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MotivosContactanos_Estatus");
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

        modelBuilder.Entity<OpcionesPreguntum>(entity =>
        {
            entity.Property(e => e.Texto)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasDefaultValue(1);

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.OpcionesPreguntumIdPreguntaNavigations)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpcionesPregunta_PreguntasEncuesta_IdPreguntaEncuesta");

            entity.HasOne(d => d.IdSkipPreguntaEncuestaNavigation).WithMany(p => p.OpcionesPreguntumIdSkipPreguntaEncuestaNavigations).HasForeignKey(d => d.IdSkipPreguntaEncuesta);

            entity.HasOne(d => d.IdTipoControlNavigation).WithMany(p => p.OpcionesPregunta)
                .HasForeignKey(d => d.IdTipoControl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OpcionesPregunta_TiposControlPregunta");
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

        modelBuilder.Entity<PortafolioAvance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PortafolioAvance");

            entity.Property(e => e.Porcentaje).HasColumnName("porcentaje");
            entity.Property(e => e.Subconceptodeacumulacion)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Totalcumple).HasColumnName("totalcumple");
        });

        modelBuilder.Entity<PrefijosRm>(entity =>
        {
            entity.ToTable("PrefijosRMS");

            entity.HasIndex(e => new { e.IdCanal, e.IdZona }, "UQ_PrefijosRMS_IdCanal_IdZona").IsUnique();

            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Prefijo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.PrefijosRms)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrefijosRMS_Canales");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.PrefijosRmIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.PrefijosRmIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.PrefijosRms)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrefijosRMS_Zonas");
        });

        modelBuilder.Entity<PreguntasEncuestum>(entity =>
        {
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MensajeLimite).IsUnicode(false);
            entity.Property(e => e.MensajeObligatoria).IsUnicode(false);
            entity.Property(e => e.MsjRspRequeridas).IsUnicode(false);
            entity.Property(e => e.RespuestasRequeridas).HasDefaultValue(1);
            entity.Property(e => e.Texto).IsUnicode(false);
            entity.Property(e => e.TextoAlternativo).IsUnicode(false);
            entity.Property(e => e.Valor).HasDefaultValue(1);

            entity.HasOne(d => d.IdEncuestaNavigation).WithMany(p => p.PreguntasEncuesta)
                .HasForeignKey(d => d.IdEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreguntasEncuesta_Encuestas");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.PreguntasEncuesta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreguntasEncuesta_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.PreguntasEncuestumIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.PreguntasEncuestumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .HasConstraintName("FK_PreguntasEncuesta_PreguntasEncuesta");

            entity.HasOne(d => d.IdTipoPreguntaNavigation).WithMany(p => p.PreguntasEncuesta)
                .HasForeignKey(d => d.IdTipoPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreguntasEncuesta_TiposPregunta");
        });

        modelBuilder.Entity<Premio>(entity =>
        {
            entity.HasIndex(e => e.Sku, "IX_Premios").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(4500)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg).HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(50);
            entity.Property(e => e.ImagenAlterna).HasMaxLength(200);
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

            entity.HasOne(d => d.IdTipoDePremioNavigation).WithMany(p => p.Premios)
                .HasForeignKey(d => d.IdTipoDePremio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premios_TiposDePremio");

            entity.HasOne(d => d.IdTipoTransaccionNavigation).WithMany(p => p.Premios)
                .HasForeignKey(d => d.IdTipoTransaccion)
                .HasConstraintName("FK_Premios_TiposDeTransaccion");
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
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleFin)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CalleInicio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EstatusRms)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("EstatusRMS");
            entity.Property(e => e.Factura)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FolioRms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FolioRMS");
            entity.Property(e => e.FolioTarjeta)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Guia)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Mensajeria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MetodoDeEntrega)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Municipio)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NoTarjeta)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroExterior)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NumeroInterior)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.PedidoRms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PedidoRMS");
            entity.Property(e => e.Referencias)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Solicitante)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoAlterno)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusRedencionNavigation).WithMany(p => p.Redenciones)
                .HasForeignKey(d => d.IdEstatusRedencion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Redenciones_EstatusDeRedenciones");

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
                .HasMaxLength(100)
                .HasColumnName("Color_Bg");
            entity.Property(e => e.ColorTxt)
                .HasMaxLength(100)
                .HasColumnName("Color_Txt");
            entity.Property(e => e.Icono).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(300);
            entity.Property(e => e.StoreProcedure).HasMaxLength(300);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Canales");

            entity.HasOne(d => d.IdDestinoNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Origenes");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Estatus");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reportes_Secciones");
        });

        modelBuilder.Entity<RespuestaEsperadum>(entity =>
        {
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Texto)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOpcionPreguntaNavigation).WithMany(p => p.RespuestaEsperada)
                .HasForeignKey(d => d.IdOpcionPregunta)
                .HasConstraintName("FK_RespuestaEsperada_OpcionesPregunta");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.RespuestaEsperadumIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.RespuestaEsperadumIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.RespuestaEsperada)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RespuestaEsperada_PreguntasEncuesta");
        });

        modelBuilder.Entity<RespuestasEncuestum>(entity =>
        {
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Texto).IsUnicode(false);

            entity.HasOne(d => d.IdBitacoraEncuestaNavigation).WithMany(p => p.RespuestasEncuesta)
                .HasForeignKey(d => d.IdBitacoraEncuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RespuestasEncuesta_BitacoraDeEncuesta");

            entity.HasOne(d => d.IdOpcionPreguntaNavigation).WithMany(p => p.RespuestasEncuesta)
                .HasForeignKey(d => d.IdOpcionPregunta)
                .HasConstraintName("FK_RespuestasEncuesta_OpcionesPregunta");

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.RespuestasEncuesta)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RespuestasEncuesta_Origenes");

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.RespuestasEncuesta)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RespuestasEncuesta_PreguntasEncuesta");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolesFdv>(entity =>
        {
            entity.ToTable("RolesFDV");

            entity.HasIndex(e => e.Nombre, "UQ_RolesFDV_Nombre").IsUnique();

            entity.Property(e => e.Busqueda)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
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

        modelBuilder.Entity<SeguimientoVista>(entity =>
        {
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdFdvaftd).HasColumnName("IdFDVAftd");

            entity.HasOne(d => d.IdFdvaftdNavigation).WithMany(p => p.SeguimientoVista)
                .HasForeignKey(d => d.IdFdvaftd)
                .HasConstraintName("FK_SeguimientoVistas_FuerzaVenta");

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.SeguimientoVista)
                .HasForeignKey(d => d.IdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoVistas_Origenes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SeguimientoVista)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoVistas_Usuarios");

            entity.HasOne(d => d.IdVistaNavigation).WithMany(p => p.SeguimientoVista)
                .HasForeignKey(d => d.IdVista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoVistas_Vistas");
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

            entity.Property(e => e.ClasesEdoCta)
                .HasMaxLength(60)
                .IsUnicode(false);
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

        modelBuilder.Entity<Tamanio>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_Tamanios_ValidaPadre"));

            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .HasConstraintName("FK_Tamanios_Tamanios");
        });

        modelBuilder.Entity<Tarjeta>(entity =>
        {
            entity.Property(e => e.FechaBaja).HasColumnType("datetime");
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Folio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.NoTarjeta)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarjetas_Estatus");

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.TarjetaIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.TarjetaIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPremioNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdPremio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarjetas_Premios");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarjetas_Usuarios");
        });

        modelBuilder.Entity<TiposControlPreguntum>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
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

        modelBuilder.Entity<TiposDePremio>(entity =>
        {
            entity.ToTable("TiposDePremio");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.TiposDePremios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.TiposDePremioIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.TiposDePremioIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TiposDeTransaccion>(entity =>
        {
            entity.ToTable("TiposDeTransaccion");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaMod).HasColumnType("datetime");
            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.TiposDeTransaccions)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdOperadorModNavigation).WithMany(p => p.TiposDeTransaccionIdOperadorModNavigations).HasForeignKey(d => d.IdOperadorMod);

            entity.HasOne(d => d.IdOperadorRegNavigation).WithMany(p => p.TiposDeTransaccionIdOperadorRegNavigations)
                .HasForeignKey(d => d.IdOperadorReg)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

        modelBuilder.Entity<TiposPago>(entity =>
        {
            entity.ToTable("TiposPago");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposPreguntum>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposWhatsApp>(entity =>
        {
            entity.ToTable("TiposWhatsApp");

            entity.HasIndex(e => new { e.IdCanal, e.Nombre }, "UQ_TiposWhatsApp_IdCanal_Nombre").IsUnique();

            entity.Property(e => e.Botones)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Mensaje).IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Parametros)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Plantilla)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UrlImagen)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.TiposWhatsApps)
                .HasForeignKey(d => d.IdCanal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TiposWhatsApp_Canales");
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
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
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

            entity.HasOne(d => d.IdTamanioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTamanio)
                .HasConstraintName("FK_Usuarios_Tamanios");
        });

        modelBuilder.Entity<UsuariosPrueba>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Operacion_UsuariosPrueba");

            entity.ToTable("UsuariosPrueba", tb => tb.HasTrigger("trg_UsuariosPrueba_ActualizarPassword"));

            entity.HasIndex(e => e.IdUsuario, "UQ_UsuariosPrueba_IdUsuario").IsUnique();

            entity.Property(e => e.Comentarios)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Cuc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.UsuariosPrueba)
                .HasForeignKey<UsuariosPrueba>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operacion_UsuariosPrueba_Usuarios");
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
            entity.HasIndex(e => e.Codigo, "UQ_Vistas_Codigo").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.NombreAlternativo)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.RequiereFechaFin).HasDefaultValue(true);
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
