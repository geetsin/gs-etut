namespace Etut.Models.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdminApproved { get; set; }

        public string? UserCreationDate { get; set; }
        public string? UserCreationTime { get; set; }
    }
}

