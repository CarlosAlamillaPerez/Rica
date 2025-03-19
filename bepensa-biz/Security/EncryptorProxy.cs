using bepensa_biz.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace bepensa_biz.Security
{
    public class EncryptorProxy : IEncryptor
    {
        private string EncryptionKey { get; set; }

        public EncryptorProxy()
        {
            EncryptionKey = "F203437E-8C98-4948-B48E-DFFD50CD8BCE";
        }

        public string Pack(string data)
        {
            var protectedString = Cipher(data);
            var compressedBytes = Compress(protectedString);
            var base64String = Base64UrlTextEncoder.Encode(compressedBytes.Reverse().ToArray());
            return base64String;
        }

        public string Unpack(string base64String)
        {
            var compressedBytes = Base64UrlTextEncoder.Decode(base64String);
            var protectedString = Decompress(compressedBytes.Reverse().ToArray());
            var unprotectedString = UnCipher(protectedString);
            return unprotectedString;
        }

        public string Pack<TType>(TType data)
            where TType : class
        {
            var serial = JsonConvert.SerializeObject(data);
            return Pack(serial);
        }

        public TType Unpack<TType>(string data)
            where TType : class
        {
            var serial = Unpack(data);
            return JsonConvert.DeserializeObject<TType>(serial);
        }

        private string Cipher(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);

            using Aes encryptor = Aes.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        private string UnCipher(string text)
        {
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }

                text = Encoding.Unicode.GetString(ms.ToArray());
            }

            return text;
        }

        public static byte[] Compress(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var dataStream = new MemoryStream(bytes);
            var memory = new MemoryStream();
            var compressor = new DeflateStream(memory, CompressionMode.Compress);
            dataStream.CopyTo(compressor);
            compressor.Flush();
            bytes = memory.ToArray();
            return bytes;
        }

        public static string Decompress(byte[] data)
        {
            using var dataStream = new MemoryStream(data);
            using var memory = new MemoryStream();
            using var compressor = new DeflateStream(dataStream, CompressionMode.Decompress);
            compressor.CopyTo(memory);
            memory.Flush();
            return Encoding.UTF8.GetString(memory.ToArray());
        }

        public string GeneraPassword(int NoCaracteres)
        {
            Random random = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$*()_+";

            StringBuilder password = new();

            // Asegurarse de que haya al menos un caracter especial
            password.Append(caracteres[random.Next(caracteres.Length - 8, caracteres.Length)]);
            // Asegurarse de que haya al menos un número
            password.Append(caracteres[random.Next(53, caracteres.Length - 10)]);
            // Asegurarse de que haya al menos una mayúscula
            password.Append(caracteres[random.Next(27, caracteres.Length - 19)]);
            // Asegurarse de que haya al menos una minuscula
            password.Append(caracteres[random.Next(1, 26)]);

            // Completar el resto del string con caracteres aleatorios
            for (int i = 0; i < NoCaracteres - 4; i++)
            {
                password.Append(caracteres[random.Next(caracteres.Length)]);
            }

            // Mezclar los caracteres aleatoriamente
            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = password[i];
                password[i] = password[j];
                password[j] = temp;
            }

            return password.ToString();
        }
    }
}
