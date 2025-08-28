using System.Text.Json.Serialization;

namespace bepensa_models.General
{
    public class JsonResponseAndroid
    {
        public string[] Relation { get; set; } = null!;
        public Target Target { get; set; } = null!;
    }

    public class Target
    {
        public string Namespace { get; set; } = null!;

        [JsonPropertyName("package_name")]
        public string PackageName { get; set; } = null!;

        [JsonPropertyName("sha256_cert_fingerprints")]
        public string[] Sha256CertFingerprints { get; set; } = null!;
    }

}
