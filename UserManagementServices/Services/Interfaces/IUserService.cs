using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using UserManagementServices.Dtos;
using UserManagementServices.Models.ViewModel;

namespace UserManagementServices.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user);
        Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token);
        Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginViewModel login);
        Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<ApiResponse<List<string>>> AssignRoleToUserAsync(IEnumerable<string> roles, IdentityUser user);
        Task<IdentityUser> GetUserByEmailAsync(string email);

    }
}
