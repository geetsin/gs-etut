

using Microsoft.AspNetCore.Identity;

namespace Etut.Models.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserCreationDate { get; set; }

        public string UserCreationTime { get; set; }
        public bool IsAdminApproved { get; set; }
    }
}
