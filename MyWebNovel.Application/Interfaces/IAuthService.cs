using MyWebNovel.Application.DTOs.Account;
using MyWebNovel.Application.DTOs.Auth;
using MyWebNovel.Domain.Entities.Accounts;

namespace MyWebNovel.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Account?> AuthenticateAsync(LoginDto input);
        Task ChangePasswordAsync(ChangePasswordDto input);
        Task ForgotPasswordAsync(ForgotPasswordDto input);
        Task<AccountDto> RegisterAsync(RegisterDto input);
        Task ResetPasswordAsync(ResetPasswordDto input);
    }
}
