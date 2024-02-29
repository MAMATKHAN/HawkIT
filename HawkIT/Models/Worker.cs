using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HawkIT.Models
{
    public class Worker
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Specialization { get; set; }
        public string? SpecializationIcon { get; set; }
        public string? WorkerImage { get; set; }
        public List<Project>? Projects { get; set; }
        [NotMapped, Required(ErrorMessage = "Пожалуйста выберите файл")]
        public IFormFile ImageFile { get; set; }
        [NotMapped, Required(ErrorMessage = "Пожалуйста выберите файл")]
        public IFormFile IconFile { get; set; }
    }
}
