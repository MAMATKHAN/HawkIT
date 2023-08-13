namespace HawkIT.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Article>? Articles { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
