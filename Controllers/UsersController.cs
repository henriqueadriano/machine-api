using AutoMapper;
using machine_api.Models;
using machine_api.Models.User;
using machine_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;

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
                var user = _mapper.Map<User>(model);
                _userRepository.AddUser(user, model.Password);
                model.Password = null;
                return Ok(model);
            }
            catch (Exception ex)
            {
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
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                var loggedUser = _mapper.Map<IList<LoggedUser>>(users);
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
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
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userRepository.RemoveUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody]UpdateModel model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                _userRepository.UpdateUser(user);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = String.Format(CultureInfo.CurrentCulture, ex.Message) });
            }
        }

    }
}
