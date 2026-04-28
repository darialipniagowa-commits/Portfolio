using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; } = string.Empty;

        public string InquiryType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your message.")]
        public string Message { get; set; } = string.Empty;
    }
}