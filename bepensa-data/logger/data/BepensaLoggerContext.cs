using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using bepensa_data.logger.models;

namespace bepensa_data.logger.data;

public partial class BepensaLoggerContext : DbContext
{
    public BepensaLoggerContext(DbContextOptions<BepensaLoggerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LoggerCanjeDigital> LoggerCanjeDigitals { get; set; }

    public virtual DbSet<LoggerCortesCuentum> LoggerCortesCuenta { get; set; }

    public virtual DbSet<LoggerDetalle> LoggerDetalles { get; set; }

    public virtual DbSet<LoggerExternalApi> LoggerExternalApis { get; set; }

    public virtual DbSet<LoggerJsonElement> LoggerJsonElements { get; set; }

    public virtual DbSet<LoggerPushNotification> LoggerPushNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.Level).HasMaxLength(128);
        });

        modelBuilder.Entity<LoggerCanjeDigital>(entity =>
        {
            entity.ToTable("Logger_CanjeDigital");

            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Folio).HasMaxLength(500);
            entity.Property(e => e.IdTransaccion).HasColumnName("Id_Transaccion");
            entity.Property(e => e.RequestFecha)
                .HasColumnType("datetime")
                .HasColumnName("Request_Fecha");
            entity.Property(e => e.ResponseFecha)
                .HasColumnType("datetime")
                .HasColumnName("Response_Fecha");
        });

        modelBuilder.Entity<LoggerCortesCuentum>(entity =>
        {
            entity.ToTable("Logger_CortesCuenta");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<LoggerDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LOGGER");

            entity.ToTable("Logger_Detalle");

            entity.Property(e => e.Controlador).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Registro");
            entity.Property(e => e.Fuente).HasMaxLength(50);
            entity.Property(e => e.IdTransaccion).HasColumnName("Id_Transaccion");
            entity.Property(e => e.Metodo).HasMaxLength(50);
            entity.Property(e => e.RequestFecha)
                .HasColumnType("datetime")
                .HasColumnName("Request_Fecha");
            entity.Property(e => e.ResponseFecha)
                .HasColumnType("datetime")
                .HasColumnName("Response_Fecha");
            entity.Property(e => e.Usuario)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoggerExternalApi>(entity =>
        {
            entity.ToTable("Logger_ExternalApi");

            entity.Property(e => e.ApiName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RequestTimestamp).HasColumnType("datetime");
            entity.Property(e => e.ResponseTimestamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoggerJsonElement>(entity =>
        {
            entity.ToTable("Logger_JsonElement");

            entity.Property(e => e.FechaReg)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoggerPushNotification>(entity =>
        {
            entity.ToTable("Logger_PushNotifications");

            entity.Property(e => e.Codigo).HasMaxLength(255);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.IdTransaccion).HasColumnName("Id_Transaccion");
            entity.Property(e => e.RequestFecha)
                .HasColumnType("datetime")
                .HasColumnName("Request_Fecha");
            entity.Property(e => e.ResponseFecha)
                .HasColumnType("datetime")
                .HasColumnName("Response_Fecha");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
