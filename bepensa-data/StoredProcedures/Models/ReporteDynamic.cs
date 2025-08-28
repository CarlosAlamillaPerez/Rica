using Microsoft.EntityFrameworkCore;

namespace bepensa_data.StoredProcedures.Models;

[Keyless]
public class ReporteDynamic
{
    public string JsonString { get; set; } = null!;
}
