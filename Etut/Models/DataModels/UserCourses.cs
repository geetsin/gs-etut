using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Etut.Models.DataModels
{

    public class UserCourses
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CourseId { get; set; }
        
    }
}
