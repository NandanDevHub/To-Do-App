using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace To_Do_App.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter the Due Date.")]
        public DateTime? DueDate { get; set; }

        [Required]
        public Priority Priority { get; set; } = Priority.Medium;

        [Required(ErrorMessage = "Please Select the Category.")]
        public string CategoryId { get; set; } = string.Empty;

        [ValidateNever] public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "Please select the status.")]
        public string StatusId { get; set; } = string.Empty;

        [ValidateNever] public Status Status { get; set; } = null!;

        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;
    }
    public enum Priority { Low = 1, Medium = 2, High = 3 }
}