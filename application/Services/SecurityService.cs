using System;
using System.Security.Cryptography;
using System.Text;
using application.Abstraction;

namespace application.Services;

public class SecurityService : ISecurityService
{
	private string publicKey;
	private string privateKey;

	public void GenerateKeys()
	{
		using (var rsa = new RSACryptoServiceProvider(1024))
		{
			publicKey = rsa.ToXmlString(false);
			privateKey = rsa.ToXmlString(true);
		}
	}

	public string Encrypt(string plainText)
	{
		using (var rsa = new RSACryptoServiceProvider())
		{
			rsa.FromXmlString(publicKey);

			byte[] data = Encoding.UTF8.GetBytes(plainText);
			byte[] encryptedData = rsa.Encrypt(data, false);

			return Convert.ToBase64String(encryptedData);
		}
	}

	public string Decrypt(string encryptedText)
	{
		using (var rsa = new RSACryptoServiceProvider())
		{
			rsa.FromXmlString(privateKey);

			byte[] encryptedData = Convert.FromBase64String(encryptedText);
			byte[] decryptedData = rsa.Decrypt(encryptedData, false);

			return Encoding.UTF8.GetString(decryptedData);
		}
	}
}