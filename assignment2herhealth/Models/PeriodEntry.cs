using System.ComponentModel.DataAnnotations;

namespace assignment2herhealth.Models
{
    public class PeriodEntry
    {
        [Key]
        public int PeriodEntryId { get; set; } // Primary Key

        [Required]
        public string Name { get; set; } // User's Name

        [Required]
        public int Age { get; set; } // User's Age

        private DateTime _periodStartDate;

        [Required]
        [Display(Name = "Period Start Date")]
        public DateTime PeriodStartDate
        {
            get { return _periodStartDate; }
            set
            {
                _periodStartDate = value;
                // Automatically calculate the NextPredictedPeriodDate when PeriodStartDate is set
                NextPredictedPeriodDate = _periodStartDate.AddDays(28);
            }
        }

        [Display(Name = "Next Predicted Period Date")]
        public DateTime NextPredictedPeriodDate { get; set; } // Calculated next period date

        public string? Photo { get; set; }

        public List<HealthSuggestion>? HealthSuggestions { get; set; } // Navigation property
    
}
}
