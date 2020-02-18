using AutoMapper;
using machine_api.Models.User;
using machine_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace machine_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private IUserService _userService;

        public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            IUserService userService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            try
            {
                // map model to entity
                var user = _mapper.Map<User>(model);
                _userRepository.AddUser(user, model.Password);
                model.Password = null;
                return Ok(model);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var loggedUser = _userService.SetUserToken(user);

            // return basic user info and authentication token
            return Ok(loggedUser);
        }

        /*
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        */
    }
}
