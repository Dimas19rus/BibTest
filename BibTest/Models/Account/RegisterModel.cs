using BibTest.DAL.Models;
using System.ComponentModel.DataAnnotations;
namespace BibTest.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Не указан повторно Пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадает")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не указано Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не указана роль")]
        public int RoleId { get; set; }
        
        public IEnumerable<Role>? Roles { get; set; }
    }
}
