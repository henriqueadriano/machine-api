using AutoMapper;
using machine_api.Models;
using machine_api.Models.User;
using machine_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                var result = await _userRepository.AddUser(user, model.Password);
                model.Password = null;
                if (result > 0)
                    return Ok(model);
                else
                    return BadRequest(new { message = "Error on User Registration!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            try
            {
                var user = await _userService.Authenticate(model);
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                var loggedUsers = _mapper.Map<IList<LoggedUser>>(users);
                return Ok(loggedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // only allow admins to access other user records
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var user = await _userRepository.GetById(id);

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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.RemoveUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]UpdateModel model)
        {
            try
            {
                // only allow current user to update his own user
                var currentUserId = int.Parse(User.Identity.Name);
                if (model.Id != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var user = _mapper.Map<User>(model);
                await _userRepository.UpdateUser(user);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = String.Format(CultureInfo.CurrentCulture, ex.Message) });
            }
        }

    }
}
