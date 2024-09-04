// Infrastructure/Repositories/UserRepository.cs
using Application.Interfaces;
using Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateUserAsync(User user)
        {

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"INSERT INTO Users (Username, Password, FirstName, LastName, Device, IPAddress, Balance, CreatedAt)
                            VALUES (@Username, @Password, @FirstName, @LastName, @Device, @IPAddress, @Balance, @CreatedAt);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QuerySingleAsync<int>(sql, user);
                    return id;
                }
            
 
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Users WHERE Username = @Username";
                return await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            }
        }

        public async Task<bool> IsFirstLoginAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM LoginLogs WHERE UserId = @UserId";
                var count = await db.ExecuteScalarAsync<int>(sql, new { UserId = userId });
                return count == 1; 
            }
        }

        public async Task AddLoginLogAsync(LoginLog log)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO LoginLogs (UserId, LoginTime, IPAddress, Device, Browser)
                            VALUES (@UserId, @LoginTime, @IPAddress, @Device, @Browser)";
                await db.ExecuteAsync(sql, log);
            }
        }

        public async Task<decimal> GetBalanceAsync(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Balance FROM Users WHERE Id = @UserId";
                return await db.ExecuteScalarAsync<decimal>(sql, new { UserId = userId });
            }
        }

        public async Task UpdateBalanceAsync(int userId, decimal amount)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Users SET Balance = Balance + @Amount WHERE Id = @UserId";
                await db.ExecuteAsync(sql, new { Amount = amount, UserId = userId });
            }
        }
    }
}
