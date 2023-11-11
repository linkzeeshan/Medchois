using System.ComponentModel.DataAnnotations;

namespace UserManagementServices.Models.ViewModel
{
    /// <summary>
    /// User Reser Password view Model
    /// </summary>
    public class ResetPasswordViewModel
    {
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password are not match")]
        public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
