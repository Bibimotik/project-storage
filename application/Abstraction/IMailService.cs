namespace application.Abstraction;

public interface IMailService
{
	Task SendMail(string code, string toMail);
}