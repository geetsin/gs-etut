using Etut.Data;
using Etut.Models.DataModels;
using Etut.Utilities;
using Etut.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Etut.Services;

namespace Etut.Controllers
{
    public class AccountController : Controller
    {
        public readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger, IUserService userService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _userService = userService;
        }
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page from account/index");
                return RedirectToAction("Signin", "Account");
            }
            return View();
        }
        public IActionResult Signin()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User signed in. Redirecting to course/index");
                return RedirectToAction("Index", "Course");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(SigninVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User signed in Successfully");
                    return RedirectToAction("Index", "Course");
                }
                _logger.LogWarning("Invalid Signin attempt");
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(model);
        }
        public IActionResult Signup()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                _logger.LogInformation("New Roles created for the first time");
                _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                _roleManager.CreateAsync(new IdentityRole(Helper.Student));
            }
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User signed in. Redirecting to course/index");
                return RedirectToAction("Index", "Course");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupVM model)
        {
            string currentTime = DateTime.Now.ToString("hh:mm:ss");
            string currentDate = DateTime.Now.ToString("dd/MM/yy");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    UserCreationDate = currentDate,
                    UserCreationTime = currentTime,
                    IsAdminApproved = false,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Helper.Student);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("New User signed up Successfully");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Signup ERROR: ", error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User signed out Successfully");
            return RedirectToAction("Signin", "Account");
        }

        [Route("account/create-admin")]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page");
                return RedirectToAction("Signin", "Account");
            }
            if (!User.IsInRole(Helper.Admin)) {
                _logger.LogInformation("User is not admin. Redirecting to signin page");
                return RedirectToAction("Index", "Course");
            }
            return View();
        }

        [Route("account/create-admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(SignupVM model)
        {
            string currentTime = DateTime.Now.ToString("hh:mm:ss");
            string currentDate = DateTime.Now.ToString("dd/MM/yy");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    UserCreationDate = currentDate,
                    UserCreationTime = currentTime,
                    IsAdminApproved = true,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Helper.Admin);
                    _logger.LogInformation("New Admin account created Successfully");
                    return RedirectToAction("Users", "Account");
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Admin Creation ERROR: ", error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [Route("account/users")]
        public IActionResult Users()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page from account/users");
                return RedirectToAction("Signin", "Account");
            }
            ViewBag.AdminList = _userService.GetAdminList();
            ViewBag.StudentList = _userService.GetStudentList();
            return View();
        }

        [Route("account/user-profile")]
        public IActionResult UserProfile()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page from account/user-profile");
                return RedirectToAction("Signin", "Account");
            }
            ViewBag.CurrenstUserDetails = _userService.GetUserDetailsById(_userManager.GetUserId(User)); // Get currently signed in user's details
            return View();
        }

        [Route("account/change-password")]  
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!_signInManager.IsSignedIn(HttpContext.User))
            {
                _logger.LogInformation("User not signed in. Redirecting to signin page from account/change-password");
                return RedirectToAction("Signin", "Account");
            }
            return View();
        }

        [Route("account/change-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        { 
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                // Restricting password change for demo accounts
                if(user.Email == Helper.demoAdminEmail || user.Email == Helper.demoStudentEmail)
                {
                    return RedirectToAction("UserProfile", "Account");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if(result.Succeeded)
                {
                    ModelState.Clear();
                    return RedirectToAction("UserProfile", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }




        }
}
