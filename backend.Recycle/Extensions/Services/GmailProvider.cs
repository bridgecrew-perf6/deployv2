using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace backend.Recycle.Services
{
    public class GmailProvider :IEmailProvider
    {
        MimeMessage message;
        SmtpClient smtp;
        private ILogger<GmailProvider> log;
        public GmailProvider(ILogger<GmailProvider> logger)
        {
            message = new MimeMessage();

            log = logger;

        }
        public async Task Send(string to, string body, string from = "mohamedabotyear@gmail.com")
        {
            using (smtp = new SmtpClient())
            {

                try
                {

                    await smtp.ConnectAsync("smtp.gmail.com", 587);//465 
                     }
                catch (SmtpCommandException ex)
                {
                    log.LogInformation("Error trying to connect: {0}", ex.Message);
                    log.LogInformation("\tStatusCode: {0}", ex.StatusCode);
                    return;
                }
                catch (SmtpProtocolException ex)
                {
                    log.LogInformation("Protocol error while trying to connect: {0}", ex.Message);
                    return;
                }

                MailboxAddress sender = new MailboxAddress("admin", from);
                message.From.Add(sender);
                MailboxAddress receiver = new MailboxAddress("user", to);
                message.To.Add(receiver);
                message.Subject = "Recycle Email Confirmation";
                BodyBuilder content = new BodyBuilder
                {
                    HtmlBody = body,
                    TextBody = "Thanks for Confirmation"
                };
                message.Body = content.ToMessageBody();
                if (!smtp.IsConnected)
                {
                    log.LogInformation($"current SmtpServer Can't Connect ,imap.gmail.com ,993");
                    return;
                }

                try
                {
                    smtp.Timeout = 10000;
                    await smtp.AuthenticateAsync("combanyegypt@gmail.com", "01099716684");
                    if (!smtp.IsAuthenticated)
                    {
                        log.LogInformation($"current Admin {sender} not Authenticated");
                        return;
                    }
                }
                catch (SmtpCommandException ex)
                {
                    log.LogInformation(ex.ToString());
                }
                catch (Exception e)
                {
                    log.LogInformation(e.ToString());
                    throw;
                }

                await smtp.SendAsync(message);

               await  smtp.DisconnectAsync(true);
               

            }
        }
    }
}
