using System.Net;
using System.Net.Mail;
namespace VereinAPI2.Db
{
    public class VereinMessageAccessProvider : MysqlAccessProvider
    {
        public VereinMessageAccessProvider(AppDbAccessProvider db) : base(db) { }
        public async Task SendMailAsync(MailMessage mailMessage, string password)
        {
            if (mailMessage?.From?.Address == null)return;
            using var smtpclient = new SmtpClient("mail.your-server.de", 587);
            smtpclient.Credentials = new NetworkCredential(mailMessage.From.Address, password);
            smtpclient.EnableSsl = true;
            await smtpclient.SendMailAsync(mailMessage);

        }

    }
}
