// Application/Interfaces/IUserRepository.cs
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<int> CreateUserAsync(User user);
        Task<bool> IsFirstLoginAsync(int userId);
        Task AddLoginLogAsync(LoginLog log);
        Task<decimal> GetBalanceAsync(int userId);
        Task UpdateBalanceAsync(int userId, decimal amount);
    }
}
