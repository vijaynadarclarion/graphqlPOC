namespace Adf.Core.Cryptography
{
    public interface IDataProtectionCipherService
    {
        string Decrypt(string cipherText);
        string Encrypt(string input);
    }
}