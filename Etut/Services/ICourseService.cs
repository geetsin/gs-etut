using Etut.Models.DataModels;
using Etut.Models.ViewModels;

namespace Etut.Services
{
    public interface ICourseService
    {
        public Task<int> CreateOrUpdateCourse(CourseVM model, bool isUpdate);
        public  Task<int> AddVideosToDatabase(List<Videos> videosList, string courseId);
        public Task AddUserToCourse(string courseId, string userId);

        public List<CourseVM> GetCoursesByUserID(string userId);
        public CourseVM GetCourseByCourseID(string courseId);
        public CourseVM GetCourseByCourseName(string courseName);
        public Task<int> AddUserToCourse(List<UserCourses> userCourses);
    }
}
