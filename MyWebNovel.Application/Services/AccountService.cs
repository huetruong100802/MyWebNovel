using MyWebNovel.Application.Interfaces;

namespace MyWebNovel.Application.Services
{
    public class AccountService(IUnitOfWork unitOfWork) : IAccountService
    {
    }
}
