using machine_api.Helpers;
using machine_api.Models.User;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace machine_api.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(AuthenticateModel model);
        LoggedUser SetUserToken(User user);
    }
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private TokenConfig _tokenConfig;

        public UserService(
            IUserRepository userRepository,
            IOptions<TokenConfig> tokenConfig)
        {
            _userRepository = userRepository;
            _tokenConfig = tokenConfig.Value;
        }

        public async Task<User> Authenticate(AuthenticateModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return null;

            var user = await _userRepository.GetByEmail(model.Email);

            if (user == null)
                return null;

            if (!HashSaltPassword.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public LoggedUser SetUserToken(User user)
        {
            return GenerateToken.AddTokenToUser(user, _tokenConfig.secret);
        }

    }
}
