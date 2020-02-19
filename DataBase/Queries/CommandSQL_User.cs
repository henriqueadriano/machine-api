namespace machine_api.DataBase.Queries
{
    public interface ICommandSQL_User
    {
        string GetUsers { get; }
        string GetUserById { get; }
        string AddUser { get; }
        string UpdateUser { get; }
        string RemoveUser { get; }
        string GetByEmail { get; }
    }
    public class CommandSQL_User : ICommandSQL_User
    {
        public string GetUsers => "SELECT * FROM User WHERE IsActive = 1";

        public string GetUserById => "SELECT * FROM User WHERE Id = @Id AND IsActive = 1";

        public string AddUser =>  @"INSERT INTO User
                                    (FirstName,LastName,Email,PasswordHash,PasswordSalt,Role) 
                                    VALUES(
	                                    @FirstName,
	                                    @LastName,
	                                    @Email,
                                        @PasswordHash,
                                        @PasswordSalt,
                                        'User'
                                    )";

        public string UpdateUser => @"Update User set 
	                                    FirstName =     @FirstName,
	                                    LastName =      @LastName,
	                                    Email =         @Email
                                    Where Id =          @Id";

        public string RemoveUser => @"  Update User set 
                                        IsActive = 0
                                        Where Id = @Id";

        public string GetByEmail => @"select * from User where Email = @Email AND IsActive = 1";
    }
}
