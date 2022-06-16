using Etut.Data;
using Etut.Models.DataModels;
using Etut.Models.ViewModels;
using Etut.Services;
using Etut.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Etut.Controllers
{
    public class CourseController : Controller
    {
        public readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        public CourseController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            ILogger<CourseController> logger, ICourseService courseService, IUserService userService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _courseService = courseService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            List<CourseVM> userCourseList = _courseService.GetCoursesByUserID(_userManager.GetUserId(User));
            // ***TODO*** Check if the user is approved and show view accordingly
            return View(userCourseList);
        }

        [Route("course/create")]
        [HttpGet]
        public IActionResult CreateCourse()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page");
                return RedirectToAction("Signin", "Account");
            }
            if (!User.IsInRole(Helper.Admin))
            {
                _logger.LogInformation("User is not admin. Redirecting to signin page");
                return RedirectToAction("Index", "Course");
            }
            return View();
        }

        [Route("course/details")]
        public IActionResult CourseDetails(string c)
        {
            CourseVM tempCourse = _courseService.GetCourseByCourseName(c);

            return View(tempCourse);
        }

        [Route("course/assign")]
        public IActionResult AssignCourse()
        {
            if (!User.IsInRole(Helper.Admin))
            {
                _logger.LogInformation("User is not Admin. Redirecting to course Index");
                return RedirectToAction("Index", "Course");
            }


            ViewBag.myCoursesList = _courseService.GetCoursesByUserID(_userManager.GetUserId(User));
            ViewBag.allStudentsList = _userService.GetStudentList();

            //_userService.GetStudentListByCourseId(ViewBag.myCoursesList[0].Id);

            return View();
        }
    }
}
