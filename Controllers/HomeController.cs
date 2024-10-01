using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalPortfolio.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace PersonalPortfolio.Controllers
{
    public class HomeController : Controller
    {

        private readonly SmtpSettings _smtpSettings;

        public HomeController(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Contact")]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("Contact")]
        [HttpPost]
        public IActionResult Contact(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_smtpSettings.SenderEmail);
                        message.To.Add(_smtpSettings.RecipientEmail);
                        message.Subject = "Nov� zpr�va z kontaktn�ho formul��e";
                        message.Body = $"Jm�no: {model.Name}\nEmail: {model.Email}\nZpr�va: {model.Message}";

                        using (var client = new SmtpClient(_smtpSettings.SmtpServer))
                        {
                            client.Port = _smtpSettings.SmtpPort;
                            client.Credentials = new NetworkCredential(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword);
                            client.EnableSsl = true;

                            client.Send(message);
                        }
                    }

                    ViewBag.Message = "Zpr�va byla �sp�n� odesl�na!";
                    return View(new ContactForm()); // Vr�t�me pr�zdn� formul��
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"P�i odes�l�n� zpr�vy do�lo k chyb�: {ex.Message}";
                    return View(model);
                }
            }

            return View(model);
        }
    }


}
