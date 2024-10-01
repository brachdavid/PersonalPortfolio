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
                        message.Subject = "Nová zpráva z kontaktního formuláøe";
                        message.Body = $"Jméno: {model.Name}\nEmail: {model.Email}\nZpráva: {model.Message}";

                        using (var client = new SmtpClient(_smtpSettings.SmtpServer))
                        {
                            client.Port = _smtpSettings.SmtpPort;
                            client.Credentials = new NetworkCredential(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword);
                            client.EnableSsl = true;

                            client.Send(message);
                        }
                    }

                    ViewBag.Message = "Zpráva byla úspìšnì odeslána!";
                    return View(new ContactForm()); // Vrátíme prázdný formuláø
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Pøi odesílání zprávy došlo k chybì: {ex.Message}";
                    return View(model);
                }
            }

            return View(model);
        }
    }


}
