using System.ComponentModel.DataAnnotations.Schema;

namespace HawkIT.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string SpecializationIcon { get; set; }
        public string WorkerImage { get; set; }
        public List<Project>? Projects { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile IconFile { get; set; }
    }
}
