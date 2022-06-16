using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Etut.Models.DataModels
{
    public class CourseVideos
    {
        [Key]
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string VideoId { get; set; }
    }
}
