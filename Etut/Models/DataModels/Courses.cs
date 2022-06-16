namespace Etut.Models.DataModels
{
    public class Courses
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatorId { get; set; }
        public bool IsValid { get; set; }
        public int VideoCount { get; set; }


    }
}
