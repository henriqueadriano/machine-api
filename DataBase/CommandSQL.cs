using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace machine_api.DataBase
{
    public interface ICommandText
    {
        string GetUsers { get; }
        string GetUserById { get; }
        string AddUser { get; }
        string UpdateUser { get; }
        string RemoveUser { get; }
    }
    public class CommandSQL : ICommandText
    {
        public string GetUsers => "SELECT * FROM User";

        public string GetUserById => "SELECT * FROM User WHERE Id = @Id";

        public string AddUser =>  @"INSERT INTO User
                                    (FirstName,LastName,Email,PasswordHash,PasswordSalt,Token,Role) 
                                    VALUES(
	                                    @FirstName,
	                                    @LastName,
	                                    @Email,
                                        @PasswordHash,
                                        @PasswordSalt,
                                        @Token,
	                                    @Role
                                    )";

        public string UpdateUser => @"Update User set 
	                                    FirstName =     @FirstName,
	                                    LastName =      @LastName,
	                                    Email =         @Email,
	                                    PasswordHash =  @PasswordHash,
	                                    PasswordSalt =  @PasswordSalt,
	                                    Token =         @Token,
	                                    Role =          @Role 
                                    Where Id =          @Id";

        public string RemoveUser => @"  Update User set 
                                        IsActive = 0
                                        Where Id = @Id";
    }
}
