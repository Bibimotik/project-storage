using System.Net;
using System.Net.Mail;
using System.Text;

using application.Abstraction.Interfaces;

namespace application.Services;

public class MailService : IMailService
{
	private readonly string _smtpServer;
	private readonly int _smtpPort;
	private readonly string _smtpUsername;
	private readonly string _smtpPassword;

	public MailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
	{
		_smtpServer = smtpServer;
		_smtpPort = smtpPort;
		_smtpUsername = smtpUsername;
		_smtpPassword = smtpPassword;
	}

	public async Task SendMail(string code, string recipient)
	{
		using SmtpClient smtpClient = new(_smtpServer, _smtpPort);
		smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
		smtpClient.EnableSsl = true;

		using MailMessage mailMessage = new();
		mailMessage.From = new MailAddress(_smtpUsername);
		mailMessage.To.Add($"{recipient}");
		mailMessage.Subject = "КОД ДЛЯ ПОДТВЕРЖДЕНИЯ";

		mailMessage.IsBodyHtml = true;
		StringBuilder htmlBody = new();
		htmlBody.Append("<!DOCTYPE html>");
		htmlBody.Append("<html lang=\"en\">");
		htmlBody.Append("<head><meta charset=\"UTF-8\"><title>Email Template</title></head>");
		htmlBody.Append("<body>");
		htmlBody.Append("<h1>КОД ДЛЯ ПОДТВЕРЖДЕНИЯ</h1>");
		htmlBody.Append("<p>Дорогой пользователь,</p>");
		//htmlBody.Append("<p>Ваш код для подтверждения:</p>");
		htmlBody.Append($"<div style=\"background-color: #007bff; color: #fff; padding: 10px 20px; border-radius: 5px; font-size: 25px; text-align: center;\">" +
							$"<strong>{code}</strong>" +
						$"</div>");
		htmlBody.Append("</body>");
		htmlBody.Append("</html>");

		mailMessage.Body = htmlBody.ToString();

		try
		{
			await smtpClient.SendMailAsync(mailMessage);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
		}
	}
}