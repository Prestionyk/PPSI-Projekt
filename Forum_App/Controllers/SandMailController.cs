using Forum_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;


namespace Forum_App.Controllers
{
    public class SandMailController : Controller
    {
        //  
        // GET: /SendMailer/   
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
                mail.Body = _objModelMail.Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("smtptestforproject@gmail.com", "dtdxmnpiozwcgndv"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                smtp.Dispose();

                return View("Index", _objModelMail);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
