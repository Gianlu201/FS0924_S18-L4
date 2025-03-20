using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using FS0924_S18_L4.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FS0924_S18_L4.Services
{
    public class EmailService
    {
        private static readonly string ApiKey = Environment.GetEnvironmentVariable(
            "SENDGRID_API_KEY"
        );

        public static async Task<bool> SendEmailAsync(
            string toEmail,
            string subject,
            string message
        )
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                Console.WriteLine(
                    "Errore: La variabile d'ambiente SENDGRID_API_KEY non è impostata."
                );
                return false;
            }

            try
            {
                var client = new SendGridClient(ApiKey);
                var from = new EmailAddress("noreply.gididi@gmail.com", "Paperino");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
                var response = await client.SendEmailAsync(msg);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore invio email: {ex.Message}");
                return false;
            }
        }
    }
}
