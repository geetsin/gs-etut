using Etut.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Etut.Models.ViewModels
{
    public class CourseVM
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatorId { get; set; }
        public bool IsValid { get; set; }
        public int VideoCount { get; set; }
        public List<Videos>? VideoList { get; set; }

    }
}
