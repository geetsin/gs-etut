using Etut.Data;
using Etut.Models.DataModels;
using Etut.Models.ViewModels;
using Etut.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Etut.Services
{
    public class CourseService : ICourseService
    {
        public readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ICourseService> _logger;

        public CourseService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, ILogger<ICourseService> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<int> CreateOrUpdateCourse(CourseVM courseModel, bool isUpdate)
        {
            if (!isUpdate)
            {
                _logger.LogWarning("Creating new course");
                if (courseModel.VideoList != null)
                {
                    int result = await AddVideosToDatabase(courseModel.VideoList, courseModel.Id);
                    if (result == Helper.successCode)
                    {
                        Courses newCourse = new Courses()
                        {
                            Id = courseModel.Id,
                            Name = courseModel.Name,
                            Description = courseModel.Description,
                            CreationDate = DateTime.Now,
                            CreatorId = courseModel.CreatorId,
                            IsValid = courseModel.IsValid,
                            VideoCount = courseModel.VideoCount,
                        };

                        _db.Add(newCourse);
                        await _db.SaveChangesAsync();
                        _logger.LogInformation("Success. New course added! Id: {Id}", courseModel.Id);
                        return Helper.successCode;
                    }
                    else
                    {
                        _logger.LogWarning("New course creation failed. Videos could not be added to database");
                        return Helper.failureCode;
                    }
                }
                _logger.LogWarning("New course creation failed. No videos associated with course");
                return Helper.failureCode;
            }
            else
            {
                // Update course logic
                return Helper.failureCode; // Change this when implemented
            }
        }
        public async Task<int> AddVideosToDatabase(List<Videos> videosList, string courseId)
        {
            for (int i = 0; i < videosList.Count(); i++)
            {
                Videos newVideoObj = new Videos()
                {
                    Id = videosList[i].Id,
                    Name = videosList[i].Name,
                    Description = videosList[i].Description,
                    VideoUrl = videosList[i].VideoUrl,
                    CreatorId = videosList[i].CreatorId,
                    CreationDate = videosList[i].CreationDate,
                    IsValid = videosList[i].IsValid,
                };
                CourseVideos courseVideosMapObj = new CourseVideos()
                {
                    Id = Helper.GetStringGUID(),
                    CourseId = courseId,
                    VideoId = newVideoObj.Id,
                };
                _db.Add(newVideoObj);
                _db.Add(courseVideosMapObj); // Mapping course and associated videos 
                _logger.LogInformation("Video added to database. Id: {Id}", newVideoObj.Id);
                _logger.LogInformation("Video Id: {videoId} added to course Id: {courseId}", courseVideosMapObj.VideoId, courseVideosMapObj.CourseId);
            }
            await _db.SaveChangesAsync();
            _logger.LogInformation("Videos added to database and mapped to a course.");
            return Helper.successCode;
        }

        // Add User and associated course to the mapping DB UserCourses
        public async Task AddUserToCourse(string courseId, string userId)
        {
            UserCourses userCourses = new UserCourses()
            {
                Id = Helper.GetStringGUID(),
                UserId = userId,
                CourseId = courseId,
            };
            _db.Add(userCourses);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Course Id: {courseId} added to User Id: {userId}", userCourses.CourseId, userCourses.UserId);
        }

        public List<CourseVM> GetCoursesByUserID(string userId)
        {
            var userCoursesList = (from course in _db.Courses
                                   join userCourse in _db.UserCourses.Where(x => x.UserId == userId) on course.Id equals userCourse.CourseId
                                   select new CourseVM
                                   {
                                       Id = course.Id,
                                       Name = course.Name,
                                       Description = course.Description,
                                       CreationDate = course.CreationDate,
                                       CreatorId = course.CreatorId,
                                       IsValid = course.IsValid,
                                       VideoCount = course.VideoCount,
                                   }

                                   ).OrderBy(o => o.CreationDate).ToList();

            foreach (var userCourses in userCoursesList)
            {
                userCourses.VideoList = (from video in _db.Videos
                                         join courseVideo in _db.CourseVideos.Where(x => x.CourseId == userCourses.Id) on video.Id equals courseVideo.VideoId
                                         select new Videos
                                         {
                                             Id = video.Id,
                                             Name = video.Name,
                                             Description = video.Description,
                                             VideoUrl = video.VideoUrl,
                                             CreatorId = video.CreatorId,
                                             CreationDate = video.CreationDate,
                                             IsValid = video.IsValid,
                                         }
                                         ).OrderBy(o => o.CreationDate).ToList();
            }

            return userCoursesList;
        }

        public CourseVM GetCourseByCourseID(string courseId)
        {
            var course = _db.Courses.Where(x => x.Id == courseId).Select(course => new CourseVM()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                CreationDate = course.CreationDate,
                CreatorId = course.CreatorId,
                IsValid = course.IsValid,
                VideoCount = course.VideoCount,
            }).SingleOrDefault();

            course.VideoList = (from video in _db.Videos
                                join courseVideo in _db.CourseVideos.Where(x => x.CourseId == course.Id) on video.Id equals courseVideo.VideoId
                                select new Videos
                                {
                                    Id = video.Id,
                                    Name = video.Name,
                                    Description = video.Description,
                                    VideoUrl = video.VideoUrl,
                                    CreatorId = video.CreatorId,
                                    CreationDate = video.CreationDate,
                                    IsValid = video.IsValid,
                                }
                                ).OrderBy(o => o.CreationDate).ToList();
            return course;
        }

        public CourseVM GetCourseByCourseName(string courseName)
        {
            var course = _db.Courses.Where(x => x.Name == courseName).Select(course => new CourseVM()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                CreationDate = course.CreationDate,
                CreatorId = course.CreatorId,
                IsValid = course.IsValid,
                VideoCount = course.VideoCount,
            }).SingleOrDefault();
            if(course !=null)
            {
                course.VideoList = (from video in _db.Videos
                                    join courseVideo in _db.CourseVideos.Where(x => x.CourseId == course.Id) on video.Id equals courseVideo.VideoId
                                    select new Videos
                                    {
                                        Id = video.Id,
                                        Name = video.Name,
                                        Description = video.Description,
                                        VideoUrl = video.VideoUrl,
                                        CreatorId = video.CreatorId,
                                        IsValid = video.IsValid,
                                    }
                                ).ToList();
            }
            
            return course;
        }

        public async Task<int> AddUserToCourse(List<UserCourses> userCourses)
        {
            foreach(UserCourses tempUserCourse in userCourses)
            {
                tempUserCourse.Id = Helper.GetStringGUID();
                _db.UserCourses.Add(tempUserCourse);
            }
            await _db.SaveChangesAsync();
            return Helper.successCode;
        }
    }
}
