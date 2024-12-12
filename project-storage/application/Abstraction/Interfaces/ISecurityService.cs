namespace application.Abstraction;

public interface ISecurityService
{
	void GenerateKeys();
	string Encrypt(string plainText);
	string Decrypt(string encryptedText);
}