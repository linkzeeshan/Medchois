using UserManagementServices.Cores;

namespace UserManagementServices.Services.Interfaces
{
    public interface IUserEmailService
    {
        Task SendEmailAsyc(Message message);
    }
}
