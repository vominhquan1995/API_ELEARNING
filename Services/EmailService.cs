using Api_ELearning.Repositories;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Threading.Tasks;

namespace Api_ELearning.Services
{
    public class EmailService : IEmailService
    {
        private readonly IAccountRepository _account;
        private readonly IHostingEnvironment _env;
        private readonly ILogger _log;
        public EmailService(IAccountRepository account, IHostingEnvironment env, ILogger<EmailService> log)
        {
            _account = account;
            _env = env;
            _log = log;
        }
        public async Task<bool> SendMailRecoveryPassword(string email)
        {
            _log.LogInformation("BEGIN => SendMailRecoveryPassword");
            var accountReset = _account.Get(email);
            if (accountReset == null)
            {
                _log.LogWarning("WARNING => SendMailRecoveryPassword : Account => [%s]",accountReset.ToString());
                return false;
            }
            _log.LogInformation("BEGIN => Hash Password");
            var hashPasswordToUrl = Protector.HashPassword(accountReset.Password);
            _log.LogInformation("END <= Hash Password");
            _log.LogInformation("BEGIN => Write Mail");
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("E-Learning", "hotro.elearning@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(accountReset.FirstName, email));
            emailMessage.Subject = "E-Learning - Recovery Password";
            var hiText = "<h1>Chào " + accountReset.FirstName + " " + accountReset.LastName +",</h1>";
            var url = "http://daotaoyduoc.azurewebsites.net/reset-password/" + hashPasswordToUrl;
            string cssAndHeader = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "Template\\MailResetPasswordCss.txt"));
            string beforeUrl = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "Template\\MailResetPasswordBeforeUrl.txt"));
            string afterUrl = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "Template\\MailResetPasswordAfterUrl.txt"));
            var bodyBuilder = new BodyBuilder()
            {
                HtmlBody = cssAndHeader + hiText + beforeUrl + url + afterUrl
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();
            _log.LogInformation("END <= Write Mail");
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
                await client.AuthenticateAsync("hotro.elearning@gmail.com", "Abcd@1234").ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                _log.LogInformation("END <= SendMailRecoveryPassword");
                return true;
            }
        }
    }
}
