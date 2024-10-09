namespace Storage.Domain.Interfaces;

public interface IPasswordHash
{
	public string Generate(string password);
	public bool Verify(string password, string hashedPassword);
}
