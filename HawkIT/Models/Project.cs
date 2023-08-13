namespace HawkIT.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Images { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Worker> Workers { get; set; }
    }
}
