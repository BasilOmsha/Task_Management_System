using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_Project.Domain.Models
{
    public class AppList : BaseClass
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        public Guid ProjectBoardId { get; set; }

        public int Position { get; set; }


        
        [ForeignKey(nameof(ProjectBoardId))]
        public ProjectBoard ProjectBoard { get; set; } //many-to-one with ProjectBoard - navigation property
        public ICollection<TaskCard> TaskCard { get; set; } //one-to-many with TaskCard - navigation property

        //possible additional properties?
        /*public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }*/
    }
}