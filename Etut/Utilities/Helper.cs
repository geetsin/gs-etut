namespace Etut.Utilities
{
    public static class Helper
    {
        // User types
        public static string Admin = "Admin";
        public static string Student = "Student";

        // Status codes
        public static int failureCode = 0;
        public static int successCode = 1;

        // Messages
        public static string genericApiCallSuccessMsg = "API Call Success!";
        public static string genericApiCallFailureMsg = "API Call Failure!";

        public static string userApprovalSuccess = "User has been successfully Approved";
        public static string userApprovalFailure = "User Approval has been failed";
        public static string userDispprovalSuccess = "User has been successfully Dispproved";
        public static string userDisapprovalFailure = "User Disapproval has been failed";

        public static string userUpdateSuccess = "User Data has been updated";
        public static string userUpdateFailure = "User Data could not be updated";


        public static string courseUpdateSuccess = "Course has been updated successfully";
        public static string courseUpdateFailure = "Course could not be updated";
        public static string courseCreateSuccess = "Course has been created successfully";
        public static string courseCreateFailure = "Course could not be created";

        public static string userCourseSuccess = "Users are assigned with the course";
        public static string userCourseFailure = "Could not assign course to users";

        // Caching codes
        public static string allStudentsCacheKey = "allStudents";
        public static string allAdminsCacheKey = "allAdmins";


        public static string GetStringGUID()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
