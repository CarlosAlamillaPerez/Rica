using bepensa_biz.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace bepensa_biz.Security
{
    public class Hash
    {
        public string Content { get; set; }

        public Hash(string _content)
        {
            Content = _content;
        }

        #region Regresa en string
        /// <summary>
        /// Encripta en SHA1
        /// </summary>
        /// <returns> Regresa valor en string</returns>
        public string ToSha1()
        {
            return CreateHash("SHA1");
        }


        /// <summary>
        /// Encripta en SHA256
        /// </summary>
        /// <returns> Regresa valor en string</returns>
        public string ToSha256()
        {
            return CreateHash("SHA256");
        }


        /// <summary>
        /// Encripta en SHA384
        /// </summary>
        /// <returns> Regresa valor en string</returns>
        public string ToSha384()
        {
            return CreateHash("SHA384");
        }

        /// <summary>
        /// Encripta en SHA512
        /// </summary>
        /// <returns> Regresa valor en string</returns>
        public string ToSha512()
        {
            return CreateHash("SHA512");
        }


        /// <summary>
        /// Encripta en MD5
        /// </summary>
        /// <returns> Regresa valor en string</returns>
        public string ToMD5()
        {
            return CreateHash("MD5");
        }

        private string CreateHash(string algorithm)
        {
            var sha = HashAlgorithm.Create(algorithm);
            var buffer = sha.ComputeHash(Encoding.UTF8.GetBytes(Content));
            return buffer.ToHashString();
        }
        #endregion

        #region Regresa en byte[]
        /// <summary>
        /// Encripta en SHA1
        /// </summary>
        /// <returns> Regresa en arreglo de bytes[]</returns>
        public byte[] Sha1()
        {
            return CreateHashB("SHA1");
        }

        /// <summary>
        /// Encripta en SHA256
        /// </summary>
        /// <returns> Regresa en arreglo de bytes[]</returns>
        public byte[] Sha256()
        {
            return CreateHashB("SHA256");
        }

        /// <summary>
        /// Encripta en SHA382
        /// </summary>
        /// <returns> Regresa en arreglo de bytes[]</returns>
        public byte[] Sha384()
        {
            return CreateHashB("SHA384");
        }

        /// <summary>
        /// Encripta en SHA512
        /// </summary>
        /// <returns> Regresa en arreglo de bytes[]</returns>
        public byte[] Sha512()
        {
            return CreateHashB("SHA512");
        }

        /// <summary>
        /// Encripta en MD5
        /// </summary>
        /// <returns> Regresa en arreglo de bytes[]</returns>
        public byte[] MD5()
        {
            return CreateHashB("MD5");
        }



        private byte[] CreateHashB(string algorithm)
        {
            var sha = HashAlgorithm.Create(algorithm);
            var buffer = sha.ComputeHash(Encoding.UTF8.GetBytes(Content));
            return buffer;
        }

        #endregion
    }
}
