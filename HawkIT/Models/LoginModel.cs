using System.ComponentModel.DataAnnotations;

namespace HawkIT.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Это поле обязательно должно быть заполнено")]
        public string Password { get; set; }
    }
}
