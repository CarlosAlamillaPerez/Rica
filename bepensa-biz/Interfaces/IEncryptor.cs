namespace bepensa_biz.Interfaces
{
    public interface IEncryptor
    {
        string GeneraPassword(int NoCaracteres);
        string Pack(string data);
        string Pack<TType>(TType data) where TType : class;
        string Unpack(string base64String);
        TType Unpack<TType>(string data) where TType : class;
    }
}