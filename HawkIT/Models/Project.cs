using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HawkIT.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectImage { get; set; }
        public List<Tag>? Tags { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public List<Worker> Workers { get; set; }
        [NotMapped, Required(ErrorMessage = "Пожалуйста выберите файл")]
        public IFormFile ImageFile { get; set; }
    }
}
