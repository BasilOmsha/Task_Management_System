using System.ComponentModel.DataAnnotations;

namespace PMS_Project.Domain.Models
{
    public class Priority : BaseClass
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public ICollection<TaskCard> TaskCards { get; set; } // Navigation property
        
    }
}