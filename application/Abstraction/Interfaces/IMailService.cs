namespace application.Abstraction.Interfaces;

public interface IMailService
{
	Task SendMail(string code, string toMail);
}