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

    public virtual DbSet<LoggerJsonElement> LoggerJsonElements { get; set; }

    public virtual DbSet<LoggerPushNotification> LoggerPushNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoggerJsonElement>(entity =>
        {
            entity.ToTable("Logger_JsonElement");

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
