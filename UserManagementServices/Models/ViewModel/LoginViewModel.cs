using System.ComponentModel.DataAnnotations;

namespace UserManagementServices.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
