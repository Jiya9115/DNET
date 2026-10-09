using System;
using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be 3 to 50 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public string Category { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Message must be 10 to 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Submitted On")]
        public DateTime SubmittedOn { get; set; }
    }
}
