using HawkIT.Models;

namespace HawkIT.ViewModel
{
    public class MainViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Worker> Workers { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Article> Articles { get; set; }
        public int? ActiveTagId { get; set; }
    }
}
