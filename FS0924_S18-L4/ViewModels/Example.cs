using SendGrid;
using SendGrid.Helpers.Mail;

namespace FS0924_S18_L4.ViewModels
{
    public class Example
    {
        private static void Main()
        {
            Execute().Wait();
        }

        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGrid_FirstKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply.gididi@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
            );
            var response = await client.SendEmailAsync(msg);
        }
    }
}
