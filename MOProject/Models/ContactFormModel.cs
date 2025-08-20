// ViewModels/ContactFormModel.cs
using System.ComponentModel.DataAnnotations;

namespace MOProject.ViewModels
{
    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
