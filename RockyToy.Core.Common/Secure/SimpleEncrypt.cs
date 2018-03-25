using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using RockyToy.Contracts.Common.Extensions;
using RockyToy.Contracts.Common.Secure;

namespace RockyToy.Core.Common.Secure
{
	/// <summary>
	///   ref http://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
	/// </summary>
	public class SimpleEncrypt : IEncrypt
	{
		private const int Iteration = 200;
		private const int KeyBitSize = 256;
		private readonly byte[] _tokenBytes;
		private readonly SecureString _defaultPassString;
		private byte[] DefaultPassBytes => Encoding.UTF8.GetBytes(_defaultPassString.ToUnsecureString());

		public SimpleEncrypt(SecureString pwd, byte[] token)
		{
			_tokenBytes = token;
			_defaultPassString = pwd;
		}

		private static byte[] Generate256BitsOfRandomEntropy()
		{
			var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
			}

			return randomBytes;
		}
		
		public string Encrypt(string plainText)
		{
			return Encrypt(plainText, _defaultPassString);
		}

		public byte[] Encrypt(byte[] plainBytes)
		{
			var plainBytesWithToken = _tokenBytes.Concat(plainBytes).ToArray();
			return Encrypt(plainBytesWithToken, DefaultPassBytes);
		}

		public string Decrypt(string cipherText)
		{
			return Decrypt(cipherText, _defaultPassString);
		}

		public byte[] Decrypt(byte[] cipherText)
		{
			return Decrypt(cipherText, DefaultPassBytes);
		}

		public string Hash(string content)
		{
			return Convert.ToBase64String(Hash(Encoding.UTF8.GetBytes(content)));
		}

		public byte[] Hash(byte[] content)
		{
			using (var hasher = new SHA256Managed())
			{
				return hasher.ComputeHash(content);
			}
		}

		public string Encrypt(string plainText, SecureString passPhrase)
		{
			var encryptedBytes =
				Encrypt(Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(passPhrase.ToUnsecureString()));
			return Convert.ToBase64String(encryptedBytes);
		}

		public byte[] Encrypt(byte[] plainBytes, byte[] passPhrase)
		{
			var plainBytesWithToken = _tokenBytes.Concat(plainBytes).ToArray();
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var saltStringBytes = Generate256BitsOfRandomEntropy();
			var ivStringBytes = Generate256BitsOfRandomEntropy();
			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Iteration))
			{
				var keyBytes = password.GetBytes(KeyBitSize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream())
						{
							using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
							{
								cryptoStream.Write(plainBytesWithToken, 0, plainBytesWithToken.Length);
								cryptoStream.FlushFinalBlock();
								// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
								var cipherTextBytes = saltStringBytes;
								cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
								cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
								return cipherTextBytes;
							}
						}
					}
				}
			}
		}

		public string Decrypt(string cipherText, SecureString passPhrase)
		{
			return Encoding.UTF8.GetString(Decrypt(Encoding.UTF8.GetBytes(cipherText),
				Encoding.UTF8.GetBytes(passPhrase.ToUnsecureString())));
		}
		
		public byte[] Decrypt(byte[] cipherText, byte[] passPhrase)
		{
			// Get the complete stream of bytes that represent:
			// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
			// Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
			var saltStringBytes = cipherText.Take(KeyBitSize / 8).ToArray();
			// Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
			var ivStringBytes = cipherText.Skip(KeyBitSize / 8).Take(KeyBitSize / 8).ToArray();
			// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
			var cipherTextBytes = cipherText.Skip(KeyBitSize / 8 * 2)
				.Take(cipherText.Length - KeyBitSize / 8 * 2).ToArray();

			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Iteration))
			{
				var keyBytes = password.GetBytes(KeyBitSize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream(cipherTextBytes))
						{
							using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
							{
								var plainBytes = new byte[cipherTextBytes.Length];
								var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
								Array.Resize(ref plainBytes, decryptedByteCount);
								if (plainBytes.Length < _tokenBytes.Length)
									throw new FormatException("Invalid encrypted data");
								if (_tokenBytes.Where((t, i) => !plainBytes[i].Equals(t)).Any())
									throw new FormatException("Invalid encrypted token");
								return plainBytes.Skip(_tokenBytes.Length).ToArray();
							}
						}
					}
				}
			}
		}
	}
}