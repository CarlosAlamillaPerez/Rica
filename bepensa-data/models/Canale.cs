﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bepensa_data.models;

public partial class Canale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstatus { get; set; }

    public DateTime FechaReg { get; set; }

    public int IdOperadorReg { get; set; }

    public int BitValue { get; set; }  

    public string? UriBase { get; set; }

    public virtual ICollection<CatalogoPushNotificacione> CatalogoPushNotificaciones { get; set; } = new List<CatalogoPushNotificacione>();

    public virtual ICollection<ConceptosDeAcumulacion> ConceptosDeAcumulacions { get; set; } = new List<ConceptosDeAcumulacion>();

    public virtual ICollection<Encuesta> Encuesta { get; set; } = new List<Encuesta>();

    public virtual ICollection<FuerzaVentum> FuerzaVenta { get; set; } = new List<FuerzaVentum>();

    public virtual Operadore IdOperadorRegNavigation { get; set; } = null!;

    public virtual ICollection<ImagenesPromocione> ImagenesPromociones { get; set; } = new List<ImagenesPromocione>();

    public virtual ICollection<PorcentajesIncrementoVentum> PorcentajesIncrementoVenta { get; set; } = new List<PorcentajesIncrementoVentum>();

    public virtual ICollection<PrefijosRm> PrefijosRms { get; set; } = new List<PrefijosRm>();

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<TiposWhatsApp> TiposWhatsApps { get; set; } = new List<TiposWhatsApp>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
