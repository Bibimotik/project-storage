using System.Net;
using System.Net.Mail;
using System.Windows;

using application.MVVM.ViewModel;
using application.Services;

using DotNetEnv;


namespace application.MVVM.View;

public partial class AuthView : Window
{
	public AuthView(AuthViewModel authViewModel)
	{
		DataContext = authViewModel;
		InitializeComponent();
	}

	private void RegistrationUser_OnClick(object sender, RoutedEventArgs e)
	{
		//Env.Load("../../../.env");
		
		//string smtpServer = "smtp.mail.ru"; //smpt сервер
		//int smtpPort = 587; // Обычно используется порт 587
		//string smtpUsername = Environment.GetEnvironmentVariable("MAIL"); //почта с которой отправляется сообщение
		//string smtpPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

		//// Создаем объект клиента SMTP
		//using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
		//{
		//	// Настройки аутентификации
		//	smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
		//	smtpClient.EnableSsl = true;

		//	using (MailMessage mailMessage = new MailMessage())
		//	{
		//		mailMessage.From = new MailAddress(smtpUsername);
		//		mailMessage.To.Add("test@yandex.ru"); //адрес получателя
		//		mailMessage.Subject = "КОД ДЛЯ ПОДТВЕРЖДЕНИЯ";
		//		mailMessage.Body = $"ТЕСТОВОЕ ПИСЬМО"; //контент письма

		//		try
		//		{
		//			smtpClient.Send(mailMessage);
		//			Console.WriteLine("Сообщение успешно отправлено.");
		//		}
		//		catch (Exception ex)
		//		{
		//			Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
		//		}
		//	}
		//}
	}
}
