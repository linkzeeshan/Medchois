namespace UserManagementServices.Models.ViewModel
{
    public class LoginOTPResponse
    {
        public string Token { get; set; }
        public DateTime expiration { get; set; }
    }
}
