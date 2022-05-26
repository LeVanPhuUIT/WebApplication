using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApplication.Services.User;
using WebApplication.Services.User.Dto;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class UserController : Controller
    {
        IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IdentityResult> AddUser([FromBody] InsertUserInput user)
        {
            return await userService.AddUser(user);
        }

        [HttpPut]
        public async Task<IdentityResult> UpdateUser([FromBody] UpdateUserInput user)
        {
            return await userService.UpdateUser(user);
        }


        [HttpGet]
        public async Task<List<SearchUserDto>> SearchUser([Required] string keyword)
        {
            return await userService.SearchUser(keyword);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteUser(string userName)
        {
            return await userService.DeleteUser(userName);
        }
    }
}
