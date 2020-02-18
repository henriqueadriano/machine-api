using AutoMapper;
using machine_api.Models;
using machine_api.Models.User;
using machine_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            try
            {
                var user = _userService.Authenticate(model);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                LoggedUser loggedUser = _userService.SetUserToken(user);
                // return basic user info and authentication token
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            try
            {
                // map model to entity
                var users = _userRepository.GetAllUsers();
                var loggedUser = _mapper.Map<IList<LoggedUser>>(users);
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                // only allow admins to access other user records
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var user = _userRepository.GetById(id);

                var loggedUser = _mapper.Map<LoggedUser>(user);

                if (user == null)
                    return NotFound();

                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
