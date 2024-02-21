using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using DotNetEnv;
using DesktopSecurityApp.UserInterface.Theme;

namespace EmailSenderApp
{
    public class Mailer
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSsl;
        private readonly string _username;
        private readonly string _password;

        public Mailer(string host, int port, bool useSsl, string username)
        {
            _host = host;
            _port = port;
            _useSsl = useSsl;
            _username = username;
        }


        public void SendEmail(string senderEmail, string recipientEmail, string subject)
        {
            DotNetEnv.Env.Load();
            string DSA_username = Environment.GetEnvironmentVariable("DSA_username");   // koristimo u Autenticate !
            string DSA_password = Environment.GetEnvironmentVariable("DSA_password");

            try
            {
                // Create a new email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Desktop Security App", senderEmail));
                message.To.Add(new MailboxAddress(_username, recipientEmail));
                message.Subject = subject;
                var emailTemplate = new EmailTemplate();
                message.Body = new TextPart("html")
                {
                    Text = emailTemplate.GetEmailBody()
                };

                // Connect to the SMTP server and send the email
                using var client = new SmtpClient();
                client.Connect("mail.skim.ba", 465, true);  // "mail.skim.ba umjesto dsa@skim.ba |  port koji je od maila | SecureSocketOptions.SslOnConnect komanda za koristenje sll umjsto TRUE         
                client.Authenticate("dsa@skim.ba", "Stavistahoces123#");                // "dsa@skim.ba umjesto dsa
                client.Send(message);
                client.Disconnect(true);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}