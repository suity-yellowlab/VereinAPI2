using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;
using VereinAPI2.Services;
namespace VereinAPI2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VereinMessageController : ControllerBase
    {
       
        private readonly IEmailService emailService;
        protected VereinMessageAccessProvider VMAP { get; }
        public AppDbAccessProvider Db { get; }
        public VereinMessageController(AppDbAccessProvider db, IEmailService emailService)
        {  
            Db = db;
            VMAP = new VereinMessageAccessProvider(Db);
            this.emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadMessages(VereinMessageRequest messageRequest)
        {
            if (messageRequest == null || messageRequest.Messages.Count == 0 || messageRequest.Name == null || messageRequest.Password == null) {
                return BadRequest();
            }       
            await emailService.SendMailAsync(messageRequest.Messages, messageRequest.Name, messageRequest.Password);
           


            
            return Ok();

        }

        [HttpPost("attachment")]
        public async Task<IActionResult> UploadFile([FromForm] IList<IFormFile> files)
        {
           
            if (files.Count == 0) { return BadRequest(); }
            var attachments = new List<AttachmentEntry>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                   var attachment = await emailService.SaveAttachment(file);
                    attachments.Add(attachment);

                }
                else { return BadRequest(); }
            }
            return Ok(attachments);


        }
    }
}
