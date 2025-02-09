using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PMS_Project.Domain.Models
{
    public class Role : BaseClass
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        // public ICollection<ProjectBoard_User> ProjectBoard_Users { get; set; }
        public ICollection<Workspace_User> Workspace_Users { get; set; }
    }
}