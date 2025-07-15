using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helpers
{
    public class ECDSAVerifier
    {
        private readonly ECDsa _publicKey;

        public ECDSAVerifier(string publicKeyPath)
        {
            if (!File.Exists(publicKeyPath))
                throw new FileNotFoundException("ECDSA public key file not found.", publicKeyPath);

            var pem = File.ReadAllText(publicKeyPath);
            _publicKey = ECDsa.Create();
            _publicKey.ImportFromPem(pem);
        }

        public bool VerifySignature(UpdateVideoInputDTO payload, string signatureBase64)
        {
            Console.WriteLine("Signature base64 recebida: " + signatureBase64);
            var json = CanonicalJson(payload);
            Console.WriteLine("JSON serializado para assinatura: " + json);
            var data = Encoding.UTF8.GetBytes(json);
            Console.WriteLine(BitConverter.ToString(data));
            var signature = Convert.FromBase64String(signatureBase64);
            return _publicKey.VerifyData(data, signature, HashAlgorithmName.SHA256);
        }

        public static string CanonicalJson(UpdateVideoInputDTO dto)
        {
            var fields = new (string Key, object? Value)[]
            {
                ("video_id", dto.Id),
                ("filename", dto.Filename),
                ("status", dto.Status),
                ("file_url", dto.FileUrl),
                ("error_message", dto.ErrorMessage),
                ("processed_at", dto.ProcessedAt?.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"))
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = null,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never
            };

            // Serializa manualmente, garantindo a ordem
            var sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < fields.Length; i++)
            {
                var (key, value) = fields[i];
                sb.Append($"\"{key}\":");
                sb.Append(value == null ? "null" : JsonSerializer.Serialize(value, options));
                if (i < fields.Length - 1)
                    sb.Append(",");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
