using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace EmailSenderApp
{
    public class Mailer
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSsl;
        private readonly string _username;
        private readonly string _password;

        public Mailer(string host, int port, bool useSsl, string username, string password)
        {
            _host = host;
            _port = port;
            _useSsl = useSsl;
            _username = username;
            _password = password;
        }

        public void SendEmail(string senderEmail, string recipientEmail, string subject, string body)
        {
            try
            {
                // Create a new email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sender Name", senderEmail));
                message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                // Connect to the SMTP server and send the email
                using (var client = new SmtpClient())
                {
                    client.Connect(_host, _port, _useSsl);
                    client.Authenticate(_username, _password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}