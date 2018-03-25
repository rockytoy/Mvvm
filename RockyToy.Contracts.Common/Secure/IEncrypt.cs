using System.Security;

namespace RockyToy.Contracts.Common.Secure
{
	public interface IEncrypt
	{
		string Encrypt(string plainText);
		byte[] Encrypt(byte[] plainBytes);
		string Encrypt(string plainText, SecureString passPhrase);
		byte[] Encrypt(byte[] plainBytes, byte[] passPhrase);

		string Decrypt(string cipherText);
		byte[] Decrypt(byte[] cipherText);
		string Decrypt(string cipherText, SecureString passPhrase);
		byte[] Decrypt(byte[] cipherText, byte[] passPhrase);

		string Hash(string content);
		byte[] Hash(byte[] content);
	}
}