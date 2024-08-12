namespace application.Abstraction;

public interface IMailService
{
	void SendMail(string code, string toMail);
}