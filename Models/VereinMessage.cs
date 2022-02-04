using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace VereinAPI2.Models

{


    public class VereinMessage
    {
        [Required]
        public VereinMailAddress? To { get; set; }
        [Required]
        public VereinMailAddress? From { get; set; }
        public List<VereinMailAddress>? BCCs { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }

        public List<AttachmentEntry>? Attachments { get; set; }
 

        public MailMessage CreateMessage()
        {
            var from = From?.ToMailAddress();
            var to = To?.ToMailAddress();
            if (from == null || to == null) throw new ApplicationException();
            var message = new MailMessage(from, to);
            message.Subject = Subject;
            message.Body = Body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(from);
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            if (BCCs != null) foreach (var bcc in BCCs)
                {

                    message.Bcc.Add(bcc.ToMailAddress());
                }
            return message;
        }
    }
}
