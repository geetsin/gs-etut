using Etut.Models.DataModels;
using Etut.Models.ViewModels;
using Etut.Services;
using Etut.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Etut.Controllers.Api
{
    [Route("api/courses")]
    [ApiController]
    public class CourseApiController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<UserApiController> _logger;
        UserManager<ApplicationUser> _userManager;
        public CourseApiController(ICourseService courseService, ILogger<UserApiController> logger, UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateNewCourse(CourseVM courseModel)
        {
            courseModel.Id = Helper.GetStringGUID();
            courseModel.CreatorId = _userManager.GetUserId(User); // Set the creator ID for the course
            courseModel.IsValid = true;
            courseModel.VideoCount = courseModel.VideoList.Count;

            foreach (Videos courseVideo in courseModel.VideoList)
            {
                courseVideo.Id = Helper.GetStringGUID();
                courseVideo.CreatorId = courseModel.CreatorId;
                courseVideo.CreationDate = DateTime.Now;
                courseVideo.IsValid = true;
            }

            CommonResponse<int> commonResponse = new CommonResponse<int>();
            bool isUpdate = false;
            try
            {
                commonResponse.status = await _courseService.CreateOrUpdateCourse(courseModel, isUpdate);
                if(commonResponse.status == 1)
                    await _courseService.AddUserToCourse(courseModel.Id, courseModel.CreatorId);

                commonResponse.message = commonResponse.status == 1 ? Helper.courseCreateSuccess : Helper.courseCreateSuccess;

            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }

            return Ok(commonResponse);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCourse(CourseVM courseModel)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            bool isUpdate = true;

            try
            {
                commonResponse.status = await _courseService.CreateOrUpdateCourse(courseModel, isUpdate);
                commonResponse.message = commonResponse.status == 1 ? Helper.courseCreateSuccess : Helper.courseCreateSuccess;

            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("student-courses/{courseID}")]
        public async Task<IActionResult> GetStudentsForThisCourse(string courseId)
        {
            
            return Ok();
        }

        [HttpPost]
        [Route("assign-course")]
        public async Task<IActionResult> AddUserToCourse(List<UserCourses> userCourse)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = await _courseService.AddUserToCourse(userCourse);
                commonResponse.message = commonResponse.status == 1 ? Helper.userCourseSuccess : Helper.userCourseFailure;
            } catch(Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }
    }
}
