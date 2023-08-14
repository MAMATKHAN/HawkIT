using System.ComponentModel.DataAnnotations.Schema;

namespace HawkIT.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ArticleImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Tag> Tags { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
