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

namespace machine_api.Services
{
    public interface IUserRepository
    {
        User GetById(int id);
        void AddUser(User entity, string password);
        void UpdateUser(User entity);
        void RemoveUser(int id);
        List<User> GetAllUsers();
        User GetByEmail(string email);
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
        public void AddUser(User entity, string password)
        {
            
            User user = HashSaltPassword.CreatePasswordHash(entity,password);
            using (IDbConnection cnn = SimpleDbConnection())
            {
                cnn.Execute(_commandSQL.AddUser, user);
            }
        }

        public List<User> GetAllUsers()
        {
            using (IDbConnection cnn = SimpleDbConnection())
            {
                return cnn.Query<User>(_commandSQL.GetUsers).ToList();
            }
        }

        public User GetById(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                return cnn.Query<User>(_commandSQL.GetUserById, new { @Id = id }).FirstOrDefault();
            }
        }

        public void RemoveUser(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                cnn.Query<User>(_commandSQL.RemoveUser, new { @Id = id });
            }
        }

        public void UpdateUser(User user)
        {
            var currentUser = GetById(user.Id);

            if (currentUser == null)
                throw new Exception("User not found!");

            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                cnn.Query<User>(_commandSQL.UpdateUser, 
                    new { 
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                    });
            }
        }

        public User GetByEmail(string email)
        {
            using (IDbConnection cnn = new SQLiteConnection(_databaseConn.Sqlite_Conn))
            {
                return cnn.Query<User>(_commandSQL.GetByEmail, new { @Email = email }).FirstOrDefault();
            }
        }
    }
}
