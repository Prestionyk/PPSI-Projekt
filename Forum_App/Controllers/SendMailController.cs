using Forum_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;


namespace Forum_App.Controllers
{
    public class SendMailController : Controller
    {
        private readonly ILogger<SendMailController> _logger;

        public SendMailController(ILogger<SendMailController> logger)
        {
            _logger = logger;
        }

        //  
        // GET: /SendMailer/
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        //  
        // POST:
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new();
                mail.To.Add("smtptestforproject@gmail.com");
                mail.From = new MailAddress(User.Identity.Name);
                mail.Subject = _objModelMail.Subject;
                mail.Body = $"Napisał:{User.Identity.Name}<br>{_objModelMail.Body}";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("smtptestforproject@gmail.com", "dtdxmnpiozwcgndv"); // User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                smtp.Dispose();

                _logger.LogInformation($"Email was send by {User.Identity.Name}");
                return RedirectToAction(nameof(Confirmed));
            }
            else
            {
                _logger.LogCritical("Email delivery failure");
                return View("Index", _objModelMail);
            }
        }

        public IActionResult Confirmed()
            {
                return View();
            }
    }
}
