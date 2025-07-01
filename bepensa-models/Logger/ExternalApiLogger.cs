using bepensa_data.logger.models;

namespace bepensa_models.Logger
{
    public class ExternalApiLogger
    {
        public long Id { get; set; }

        public string ApiName { get; set; } = null!;

        public string Method { get; set; } = null!;

        public string? RequestBody { get; set; }

        public string? ResponseBody { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public int? StatusCode { get; set; }

        public string? Resultado { get; set; }

        public Guid? IdTransaccionLog { get; set; }

        public static implicit operator LoggerExternalApi(ExternalApiLogger request)
        {
            return new LoggerExternalApi
            {
                Id = request.Id,
                ApiName = request.ApiName,
                Method = request.Method,
                RequestBody = request.RequestBody,
                ResponseBody = request.ResponseBody,
                RequestTimestamp = request.RequestTimestamp,
                ResponseTimestamp = request.ResponseTimestamp,
                StatusCode = request.StatusCode,
                Resultado = request.Resultado,
                IdTransaccionLog = request.IdTransaccionLog,
            };
        }
    }
}
