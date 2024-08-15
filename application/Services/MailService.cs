using System.Net;
using System.Net.Mail;

using application.Abstraction;

using DotNetEnv;

namespace application.Services;

public class MailService : IMailService
{
	public async Task SendMail(string code, string toMail)
	{
		Env.Load("../../../.env");

		string smtpServer = "smtp.mail.ru";
		int smtpPort = 587;
		string smtpUsername = Environment.GetEnvironmentVariable("MAIL");
		string smtpPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

		using (SmtpClient smtpClient = new(smtpServer, smtpPort))
		{
			smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
			smtpClient.EnableSsl = true;

			using (MailMessage mailMessage = new())
			{
				mailMessage.From = new MailAddress(smtpUsername);
				mailMessage.To.Add($"{toMail}"); //адрес получателя
				mailMessage.Subject = "КОД ДЛЯ ПОДТВЕРЖДЕНИЯ";
				mailMessage.Body = $"{code}"; //контент письма // TODO - добавить как то выделение кода более большим шрифтом

				try
				{
					await smtpClient.SendMailAsync(mailMessage);
					// TODO - убрать потом
					Console.WriteLine("Сообщение успешно отправлено.");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
				}
			}
		}
	}
}