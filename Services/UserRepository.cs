using Dapper;
using machine_api.DataBase.Queries;
using machine_api.Helpers;
using machine_api.Models.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace machine_api.Services
{
    public interface IUserRepository
    {
        User GetById(int id);
        void AddUser(User entity, string password);
        void UpdateUser(User entity, int id);
        void RemoveUser(int id);
        List<User> GetAllUsers();
    }
    public class UserRepository : IUserRepository
    {
        private DatabaseConn _databaseConn;
        private readonly ICommandSQL _commandSQL;
        public UserRepository(IOptions<DatabaseConn> databaseConn, ICommandSQL commandText)
        {
            _databaseConn = databaseConn.Value;
            _commandSQL = commandText;
        }

        public void AddUser(User entity, string password)
        {
            
            User user = HashSaltPassword.CreatePasswordHash(entity,password);
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                cnn.Execute(_commandSQL.AddUser, user);
            }
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
