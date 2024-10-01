using System.ComponentModel.DataAnnotations;

namespace PersonalPortfolio.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Jméno je povinné")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail je povinný")]
        [EmailAddress(ErrorMessage = "Neplatná e-mailová adresa")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Zpráva je povinná")]
        public string Message { get; set; }
    }
}
