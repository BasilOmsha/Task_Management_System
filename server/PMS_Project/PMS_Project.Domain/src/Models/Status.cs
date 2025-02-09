using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class Status : BaseClass
    {
        public string Name { get; set; }
        public ICollection<TaskCard> TaskCards { get; set; } // Navigation property for one-to-many relationship
    }
}