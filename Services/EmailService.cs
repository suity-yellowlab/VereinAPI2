using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VereinAPI2.Helpers;
using VereinAPI2.Models;

namespace VereinAPI2.Services
{
    public interface IEmailService {

        Task SendMailAsync(VereinMessage mailMessage,string username, string password);
        Task SendMailAsync(IEnumerable<VereinMessage> messages,string username, string password);
        Task<AttachmentEntry> SaveAttachment(IFormFile formFile);
    }
    public class EmailService : IEmailService
    {
        public const string uploadDirectory = "uploads/";
        private readonly string uploadPath;
        private readonly IWebHostEnvironment env;
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> mailsettings, IWebHostEnvironment env) { 
            emailSettings = mailsettings.Value;
            this.env = env;
            uploadPath = Path.Combine(this.env.ContentRootPath, uploadDirectory);
            System.IO.Directory.CreateDirectory(uploadPath);
        }

        public async Task<AttachmentEntry> SaveAttachment(IFormFile formFile)
        {
            var guid = Guid.NewGuid();
            var fileName = $"{guid}";
            var filePath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return new AttachmentEntry()
            {
                UUID = guid.ToString(),
                Filename = formFile.FileName,
            };
        }

        public async Task SendMailAsync(VereinMessage message, string username, string password) { 
            await SendMailAsync(new VereinMessage[] {message}, username,password);
        }

        public async Task SendMailAsync(IEnumerable<VereinMessage> messages,string username, string password)
        {
            using var client = new SmtpClient(emailSettings.Host,emailSettings.Port);
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;
            foreach (var vereinmessage in messages) {
               var mailMessage = vereinmessage.CreateMessage();
                if (mailMessage == null) continue;
                if (vereinmessage.Attachments != null) foreach (var attachment in vereinmessage.Attachments)
                    {
                        if (attachment.UUID == null) continue;
                        var stream = new FileStream(Path.Combine(uploadPath, attachment.UUID), FileMode.Open);
                        var mailAttachment = new Attachment(stream, attachment.Filename);
                            mailMessage.Attachments.Add(mailAttachment);
                    }
                await client.SendMailAsync(mailMessage);
            }


        }
    }
}
