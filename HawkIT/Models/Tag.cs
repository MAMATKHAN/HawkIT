using System.ComponentModel.DataAnnotations;

namespace HawkIT.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле должно быть обязательно заполнено")]
        public string Name { get; set; }
        public List<Article>? Articles { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
