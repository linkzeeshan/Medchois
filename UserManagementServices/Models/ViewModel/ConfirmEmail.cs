namespace UserManagementServices.Models.ViewModel
{
    public class ConfirmEmail
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime expiration { get; set; }
    }
}
