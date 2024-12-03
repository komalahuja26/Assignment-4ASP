using System.ComponentModel.DataAnnotations;

namespace assignment2herhealth.Models
{
    public class HealthSuggestion
    {
        
        [Key]
        public int HealthSuggestionId { get; set; } // Primary Key

        [Required]
        [Display(Name = "Period Entry")]
        public int PeriodEntryId { get; set; } // Foreign Key linking to PeriodEntry

        [Required]
        [Display(Name = "Water Intake (Glasses)")]
        public int WaterIntake { get; set; } // Number of glasses of water

        [Display(Name = "Healthy Foods")]
        public string HealthyFoods { get; set; } // List of suggested healthy foods

        public PeriodEntry? PeriodEntry { get; set; } // Navigation property
    }
}

