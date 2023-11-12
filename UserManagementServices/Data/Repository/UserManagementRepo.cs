using Microsoft.AspNetCore.Identity;
using UserManagementServices.Data.Repository.Interfaces;
using UserManagementServices.Dtos;
using UserManagementServices.Models.ViewModel;
using UserManagementServices.Services;
using UserManagementServices.Services.Interfaces;

namespace UserManagementServices.Data.Repository
{
    public class UserManagementRepo : IUserManagementRepo
    {
        private readonly IUserService _userService;

        public UserManagementRepo(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token)
        {
            return await _userService.ConfirnmEmailAsync(email, token);
        }

        public async Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user)
        {
            //user is created but not activated
            return await _userService.CreateAsync(user);
        }

        public Task<ApiResponse<string>> ForgotPasswordAsync(string email)
        {
            return _userService.ForgotPasswordAsync(email);
        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        public async Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginViewModel login)
        {
            return await _userService.LoginAsync(login);
        }

        public async Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email)
        {
            return await _userService.LoginWithOTPAsync(code, email);
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            return await _userService.ResetPasswordAsync(model);
        }

        public async Task<ApiResponse<ResetPasswordViewModel>> ResetPasswordAsync(string token, string email)
        {
            if (token == null) { throw new ArgumentNullException("token"); }
            if (email == null) { throw new ArgumentNullException("email"); }
            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return new ApiResponse<ResetPasswordViewModel> { Success = true, Data = model };
        }
    }
}
