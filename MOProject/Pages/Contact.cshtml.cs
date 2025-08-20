using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MOProject.ViewModels;

namespace MOProject.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;

        public ContactModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [BindProperty]
        public ContactFormModel ContactForm { get; set; }

        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _emailService.SendEmailAsync(
                ContactForm.Name,
                ContactForm.Email,
                ContactForm.Phone,
                ContactForm.Message
            );

            SuccessMessage = "Your message was sent successfully!";
            ModelState.Clear();
            return RedirectToPage("/Index"); // Razor Page in Pages/Index.cshtml

        }
    }
}
