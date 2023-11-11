using UserManagementServices.Dtos;
using UserManagementServices.Models.ViewModel;

namespace UserManagementServices.Data.Repository.Interfaces
{
    public interface IUserManagementRepo
    {
        Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user);
        Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token);
        Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginViewModel login);
        Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
