using Dapper;
using machine_api.DataBase.Queries;
using machine_api.Helpers;
using machine_api.Models.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace machine_api.Services
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);
        Task<int> AddUser(User entity, string password);
        Task UpdateUser(User entity);
        Task RemoveUser(int id);
        Task<List<User>> GetAllUsers();
        Task<User> GetByEmail(string email);
    }
    public class UserRepositorySQlite : IUserRepository
    {
        private DatabaseConn _databaseConn;
        private readonly ICommandSQL_User _commandSQL;
        public UserRepositorySQlite(IOptions<DatabaseConn> databaseConn, 
            ICommandSQL_User commandText)
        {
            _databaseConn = databaseConn.Value;
            _commandSQL = commandText;
        }

        public SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection(_databaseConn.Sqlite_Conn);
        }
        public async Task<int> AddUser(User entity, string password)
        {
            
            User user = HashSaltPassword.CreatePasswordHash(entity,password);
            using (IDbConnection cnn = SimpleDbConnection())
            {
                var result = await cnn.ExecuteAsync(_commandSQL.AddUser, user);
                return result;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            using (IDbConnection cnn = SimpleDbConnection())
            {
                var result = await cnn.QueryAsync<User>(_commandSQL.GetUsers);
                return result.ToList();
            }
        }

        public async Task<User> GetById(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                return await cnn.QueryFirstAsync<User>(_commandSQL.GetUserById, new { @Id = id });
            }
        }

        public async Task RemoveUser(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                await cnn.QueryAsync<User>(_commandSQL.RemoveUser, new { @Id = id });
            }
        }

        public async Task UpdateUser(User user)
        {
            var currentUser = await GetById(user.Id);

            if (currentUser == null)
                throw new Exception("User not found!");

            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                await cnn.QueryAsync<User>(_commandSQL.UpdateUser, 
                    new { 
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                    });
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                return await cnn.QueryFirstAsync<User>(_commandSQL.GetByEmail, new { @Email = email });
            }
        }
    }
}
