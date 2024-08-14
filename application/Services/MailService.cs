using System.Net;
using System.Net.Mail;
using System.Text;

using application.Abstraction;

using DotNetEnv;

namespace application.Services;

public class MailService : IMailService
    {
        public void SendMail(string code, string toMail)
        {
            Env.Load("../../../.env");

            string smtpServer = "smtp.mail.ru";
            int smtpPort = 587;
            string smtpUsername = Environment.GetEnvironmentVariable("MAIL");
            string smtpPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add(toMail);
                    mailMessage.Subject = "КОД ДЛЯ ПОДТВЕРЖДЕНИЯ";

                    // Заменяем текстовое письмо на HTML-шаблон
                    mailMessage.IsBodyHtml = true;
                    StringBuilder htmlBody = new StringBuilder();
                    htmlBody.Append("<!DOCTYPE html>");
                    htmlBody.Append("<html lang=\"en\">");
                    htmlBody.Append("<head><meta charset=\"UTF-8\"><title>Email Template</title></head>");
                    htmlBody.Append("<body>");
                    htmlBody.Append("<h1>КОД ДЛЯ ПОДТВЕРЖДЕНИЯ</h1>");
                    htmlBody.Append("<p>Дорогой пользователь,</p>");
                    htmlBody.Append("<p>Ваш код для подтверждения:</p>");
                    htmlBody.Append($"<div style=\"background-color: #007bff; color: #fff; padding: 10px 20px; border-radius: 5px; text-align: center;\">" +
										$"<strong>{code}</strong>" +
                                    $"</div>");
                    htmlBody.Append("</body>");
                    htmlBody.Append("</html>");

                    mailMessage.Body = htmlBody.ToString();

                    try
                    {
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
                    }
                }
            }
        }
    }