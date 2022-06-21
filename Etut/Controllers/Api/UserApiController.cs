using Etut.Models.DataModels;
using Etut.Models.ViewModels;
using Etut.Services;
using Etut.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Etut.Controllers.Api
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserApiController> _logger;
        public UserApiController(IUserService userService, ILogger<UserApiController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("details/{userId}")]
        public IActionResult GetUserDetailsById(string userId)
        {
            CommonResponse<UserVM> commonResponse = new CommonResponse<UserVM>();
            try
            {
                UserVM? tempUser = _userService.GetUserDetailsById(userId);
                if (tempUser != null)
                {
                    commonResponse.dataenum = tempUser;
                    commonResponse.message = "User details found";
                    commonResponse.status = Helper.successCode;
                }
                else
                {
                    commonResponse.message = "Cannot find user with Id: " + userId;
                    commonResponse.status = Helper.failureCode;
                    return NotFound(commonResponse);
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("approve/{userId}")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try


            {
                commonResponse.status = await _userService.ApproveUser(userId);
                commonResponse.message = commonResponse.status == 1 ? Helper.userApprovalSuccess : Helper.userApprovalFailure;
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("reject/{userId}")]
        public async Task<IActionResult> RejectUser(string userId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = await _userService.RejectUser(userId);
                commonResponse.message = commonResponse.status == 1 ? Helper.userDispprovalSuccess : Helper.userDisapprovalFailure;
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
        public async Task<IActionResult> UpdateUserData(UserVM pData)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = await _userService.UpdateUserData(pData);
                commonResponse.message = commonResponse.status == 1 ? Helper.userUpdateSuccess : Helper.userUpdateFailure;

            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("students/{courseId}")]
        public IActionResult GetStudentListByCourseId(string courseId)
        {
            CommonResponse<List<UserVM>> commonResponse = new CommonResponse<List<UserVM>>();
            try
            {
                commonResponse.dataenum = _userService.GetStudentListByCourseId(courseId);
                commonResponse.status = Helper.successCode;
                commonResponse.message = Helper.genericApiCallSuccessMsg;
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failureCode;
            }
            return Ok(commonResponse);
        }
    }
  
}
