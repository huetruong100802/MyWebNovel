using MyWebNovel.Application.DTOs.Account;
using MyWebNovel.Application.DTOs.Novel;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Enums;

namespace MyWebNovel.Application.Extensions
{
    public static class AccountExtensions
    {
        public static AccountDto ToDto(this Account account)
        {
            return new AccountDto(
                account.Id,
                account.Username,
                account.Email.Value,
                ((RoleEnum)account.RoleId).ToString()
            );
        }

        public static CreateNovelDto WithAuthorId(this CreateNovelDto dto, Guid userId)
        {
            return dto with { AuthorId = userId };
        }
    }
}
