namespace Etut.Models.DataModels
{
    public class Videos
    {
        public string? Id { get; set; }
        //public int VideoOrderNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatorId { get; set; }
        public bool IsValid { get; set; }
    }
}
