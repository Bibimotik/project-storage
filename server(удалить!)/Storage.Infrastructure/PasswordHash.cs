using Storage.Domain.Interfaces;

namespace Storage.Infrastructure;

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