using Etut.Data;
using Etut.Models.DataModels;
using Etut.Models.ViewModels;
using Etut.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Etut.Services
{
    public class UserService : IUserService
    {
        public readonly ApplicationDbContext _db;
        private readonly IMemoryCache _memoryCache;

        public UserService(ApplicationDbContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }


        public List<UserVM> GetAdminList()
        {
            var adminList = new List<UserVM>();
            if(_memoryCache.TryGetValue(Helper.allAdminsCacheKey, out List<UserVM> allAdmins))
            {
                adminList = allAdmins;
            }
            else
            {
                adminList = (from users in _db.Users
                             join userRoles in _db.UserRoles on users.Id equals userRoles.UserId
                             join roles in _db.Roles.Where(x => x.Name == Helper.Admin) on userRoles.RoleId equals roles.Id
                             select new UserVM
                             {
                                 Id = users.Id,
                                 FirstName = users.FirstName,
                                 LastName = users.LastName,
                                 Email = users.Email,
                                 IsAdminApproved = users.IsAdminApproved,
                             }
                             ).ToList();
                _memoryCache.Set(Helper.allAdminsCacheKey, adminList, TimeSpan.FromMinutes(1));
            }
            
            return adminList;
        }

        public List<UserVM> GetStudentList()
        {
            var studentList = new List<UserVM>();
            if (_memoryCache.TryGetValue(Helper.allStudentsCacheKey, out List<UserVM> allStudents))
            {
                studentList = allStudents;
            } else
            {
                studentList = (from users in _db.Users
                               join userRoles in _db.UserRoles on users.Id equals userRoles.UserId
                               join roles in _db.Roles.Where(x => x.Name == Helper.Student) on userRoles.RoleId equals roles.Id
                               select new UserVM
                               {
                                   Id = users.Id,
                                   FirstName = users.FirstName,
                                   LastName = users.LastName,
                                   Email = users.Email,
                                   IsAdminApproved = users.IsAdminApproved,
                               }
                               ).ToList();
                _memoryCache.Set(Helper.allStudentsCacheKey, studentList, TimeSpan.FromMinutes(1));

            }
            return studentList;
        }
        public List<UserVM> GetStudentListByCourseId(string courseId)
        {
            var studentList = (from users in _db.Users
                               join userRoles in _db.UserRoles on users.Id equals userRoles.UserId
                               join roles in _db.Roles.Where(x => x.Name == Helper.Student) on userRoles.RoleId equals roles.Id
                               join students in _db.UserCourses.Where(x => x.CourseId == courseId) on users.Id equals students.UserId
                               select new UserVM
                               {
                                   Id = users.Id,
                                   FirstName = users.FirstName,
                                   LastName = users.LastName,
                                   Email = users.Email,
                                   IsAdminApproved = users.IsAdminApproved,
                               }
                               ).ToList();

            Console.WriteLine(studentList);
            return studentList;
        }

        public UserVM? GetUserDetailsById(string userId)
        {
            return _db.Users.Where(x => x.Id == userId).Select(c => new UserVM()
            {
                Id=c.Id,
                FirstName = c.FirstName,
                LastName=c.LastName,
                Email = c.Email,
                IsAdminApproved=c.IsAdminApproved,
                UserCreationDate = c.UserCreationDate,
                UserCreationTime = c.UserCreationTime,

            }).SingleOrDefault();
        }

        public async Task<int> ApproveUser(string userId)
        {
            var tempUser = _db.Users.FirstOrDefault(x => x.Id == userId);
            if(tempUser != null)
            {
                tempUser.IsAdminApproved = true;
                return await _db.SaveChangesAsync();
            }
            else
            {
                return Helper.failureCode;
            }
        }

        public async Task<int> RejectUser(string userId)
        {
            var tempUser = _db.Users.FirstOrDefault(x => x.Id == userId);
            if (tempUser != null)
            {
                tempUser.IsAdminApproved = false;
                return await _db.SaveChangesAsync();
            }
            else
            {
                return Helper.failureCode;
            }
        }

        public async Task<int> UpdateUserData(UserVM model)
        {
            if(model != null)
            {
                var tempUser = _db.Users.FirstOrDefault(x => x.Id == model.Id);
                if(tempUser != null)
                {
                    tempUser.FirstName = model.FirstName;
                    tempUser.LastName = model.LastName;
                    tempUser.Email = model.Email;
                    tempUser.IsAdminApproved = model.IsAdminApproved;

                    return await _db.SaveChangesAsync();
                } else
                {
                    return Helper.failureCode;               
                }

            } else
            {
                return Helper.failureCode;
            }
        }

    }
}
