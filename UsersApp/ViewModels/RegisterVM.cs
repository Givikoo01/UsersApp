using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class RegisterVM
    {
        [Required]
        
        public string? Name {  get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
