using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace bepensa_models.ApiResponse;

public class RastreoItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(object));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);

        //DHL
        if (jo["typeCode"] != null)
        {
            return jo.ToObject<RastreoItemType3>();
        }
        //FEDEX
        else if (jo["eventType"] != null)
        {
            return jo.ToObject<RastreoItemType1>();
        }
        //ESTAFETA
        else if (jo["code"] != null)
        {
            return jo.ToObject<RastreoItemType2>();
        }
        //Shipclick
        else if (jo["id"] != null)
        {
            return jo.ToObject<RastreoItemType4>();
        }
        else
        {
            throw new JsonSerializationException("Tipo de objeto desconocido en 'rastreo''");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class RastreoItemType3
{
    public string? date { get; set; }
    public string? time { get; set; }
    public string? typeCode { get; set; }
    public string? description { get; set; }
    public List<ServicioArea>? serviceArea { get; set; }
    public string? signedBy { get; set; }
}

public class ServicioArea
{
    public string? code { get; set; }
    public string? description { get; set; }
}

public class RastreoItemType1
{
    public DateTime Date { get; set; }
    public string EventType { get; set; }
    public string EventDescription { get; set; }
    public string ExceptionCode { get; set; }
    public string ExceptionDescription { get; set; }
    public ScanLocation ScanLocation { get; set; }
    public string LocationId { get; set; }
    public string LocationType { get; set; }
    public string DerivedStatusCode { get; set; }
    public string DerivedStatus { get; set; }
}

public class ScanLocation
{
    public List<string> StreetLines { get; set; }
    public string City { get; set; }
    public string StateOrProvinceCode { get; set; }
    public string PostalCode { get; set; }
    public string CountryCode { get; set; }
    public bool Residential { get; set; }
    public string CountryName { get; set; }
}

public class RastreoItemType2
{
    public string Code { get; set; }
    public string SpanishDescription { get; set; }
    public string EnglishDescription { get; set; }
    public string ReasonCode { get; set; }
    public DateTime EventDateTime { get; set; }
    public string WarehouseCode { get; set; }
    public string WarehouseName { get; set; }
}

public class RastreoItemType4
{
    public string? dateTime { get; set; }
    public string? create_date { get; set; }
    public statusObjeto? statusObj { get; set; }
    public string? country { get; set; }
    public string? city { get; set; }
    public List<auxiliar>? aux { get; set; }
    public int? id { get; set; }
    public string? code { get; set; }
    public string? value { get; set; }
    public int? status { get; set; }
    public string? description { get; set; }
}

public class statusObjeto
{
    public int? id { get; set; }
    public string? code { get; set; }
    public string? description { get; set; }
    public string? description_en { get; set; }
    public string? long_description { get; set; }
    public string? long_description_en { get; set; }
    public int? parent_id { get; set; }
    public int? is_alternative { get; set; }
    public int? is_requiere_persona_recibe { get; set; }
    public int? is_requiere_observaciones { get; set; }
    public int? is_outstanding { get; set; }
    public int? is_visible_on_tracking_ { get; set; }
    public int? is_visible_on_web { get; set; }
    public int? is_problem { get; set; }
    public int? is_final { get; set; }
    public int? ponderacion { get; set; }
    public int? count_as_try { get; set; }
    public int? active { get; set; }
    public int? is_hora_automatica { get; set; }
    public int? is_suceptible_cobro { get; set; }
    public int? is_email { get; set; }

}

public class auxiliar
{
    public string? code { get; set; }
    public string description { get; set; }
}