using Microsoft.Extensions.Caching.Memory;
using MyWebNovel.Application.DTOs.Account;
using MyWebNovel.Application.DTOs.Auth;
using MyWebNovel.Application.Exceptions;
using MyWebNovel.Application.Extensions;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Shared;
using MyWebNovel.Application.Utils;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Enums;

namespace MyWebNovel.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IEmailService emailService) : IAuthService
    {
        public async Task<Account?> AuthenticateAsync(LoginDto input)
        {
            var account = await unitOfWork.Accounts.GetByUsernameOrEmailAsync(input.Username);
            return account == null || !account.Password.Verify(input.Password) ? null : account;
        }

        public async Task<AccountDto> RegisterAsync(RegisterDto input)
        {
            var account = Account.Create(input.Username, input.Email, input.Password, (int)RoleEnum.Member);
            await unitOfWork.Accounts.AddAsync(account);
            await unitOfWork.SaveChangesAsync();
            return account.ToDto();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto input)
        {
            var account = await unitOfWork.Accounts.GetByUsernameOrEmailAsync(input.Email);
            if (account == null) throw new NotFoundException(nameof(Account), nameof(Account.Email), input.Email);

            var token = StringUtils.GenerateRandomString(6);

            memoryCache.Set(input.Email, token, TimeSpan.FromMinutes(5));

            await emailService.SendEmailAsync(account.Email.Value, "Password Reset Token",
                $"Your password reset token is: {token}\nThis token is valid for 5 minutes.");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            if (!memoryCache.TryGetValue(input.Email, out string? storedToken) || storedToken != input.ResetToken)
                throw new ArgumentException("Invalid or expired reset token.");

            var account = await unitOfWork.Accounts.GetByUsernameOrEmailAsync(input.Email)
                ?? throw new NotFoundException(nameof(Account), nameof(Account.Email), input.Email);

            account.ChangePassword(input.NewPassword);
            await unitOfWork.SaveChangesAsync();

            memoryCache.Remove(input.Email);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto input)
        {
            var account = await unitOfWork.Accounts.GetByIdAsync(input.UserId)
                ?? throw new NotFoundException(nameof(Account), nameof(Account.Id), input.UserId);

            if (!account.Password.Verify(input.OldPassword))
                throw new Exception("Invalid old password.");

            account.ChangePassword(input.NewPassword);

            await unitOfWork.SaveChangesAsync();
        }

    }
}
