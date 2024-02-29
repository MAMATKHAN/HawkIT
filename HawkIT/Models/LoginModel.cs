using System.ComponentModel.DataAnnotations;

namespace HawkIT.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан Пароль")]
        public string Password { get; set; }
    }
}
