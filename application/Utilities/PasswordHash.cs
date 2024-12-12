using application.Abstraction.Interfaces;

namespace application.Utilities;

public class PasswordHash : IPasswordHash
{
	public string Generate(string password)
	{
		return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
	}

	public bool Verify(string password, string passwordHash)
	{
		try
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException("Неверная версия соли в хэше пароля.", ex);
		}
	}
}