using System;
using System.Collections.Generic;

namespace bepensa_data.models;

public partial class EstatusDePushNotificacione
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<BitacoraPushNotificacione> BitacoraPushNotificaciones { get; set; } = new List<BitacoraPushNotificacione>();
}
