using Microsoft.AspNetCore.Identity;

namespace UserManagementServices.Models.ViewModel
{
    public class CreateUserResponse
    {
        public string Token { get; set; }
        public IdentityUser User { get; set; }
    }
}
