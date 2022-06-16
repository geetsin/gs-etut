using Etut.Models.ViewModels;

namespace Etut.Services
{
    public interface IUserService
    {
        public List<UserVM> GetStudentList();
        public List<UserVM> GetStudentListByCourseId(string courseId);
        public List<UserVM> GetAdminList();

        public UserVM? GetUserDetailsById(string userId);
        public Task<int> ApproveUser(string userId);
        public Task<int> RejectUser(string userId);
        public Task<int> updateUserData(UserVM model);
    }
}
