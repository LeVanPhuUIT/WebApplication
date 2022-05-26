using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Services.User.Dto;

namespace WebApplication.Services.User
{
    public interface IUserService
    {
        Task<IdentityResult> AddUser(InsertUserInput userDto);
        Task<IdentityResult> UpdateUser(UpdateUserInput userDto);
        Task<List<SearchUserDto>> SearchUser(string keyword);
        Task<bool> DeleteUser(string userName);
    }
    public class UserService : IUserService
    {
        UserManager<ApplicationUser> userManager;
        ApplicationDbContext applicationDbContext;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IdentityResult> AddUser(InsertUserInput userDto)
        {
            if (userDto.Roles.Any(x => x != "Admin" && x != "NormalUser"))
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Code = "Roles", Description = "Role not found!" } });
            }
            var user = new ApplicationUser()
            {
                UserName = userDto.UserName,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                DateOfBirth = userDto.DateOfBirth,
                Email = userDto.Email,
                Remark = userDto.Remark,
                RecordActive = true
            };
            var result = await userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            foreach (var role in userDto.Roles)
            {
                result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateUser(UpdateUserInput userDto)
        {
            if (userDto.Roles.Any(x => x != "Admin" && x != "NormalUser"))
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Code = "Roles", Description = "Role not found!" } });
            }

            var user = await userManager.FindByNameAsync(userDto.UserName);

            if(user == null)
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Code = "UserName", Description = "User not found!" } });
            }

            var oldRoles = await userManager.GetRolesAsync(user);
            foreach (var role in oldRoles)
            {
                await userManager.RemoveFromRoleAsync(user, role);
            }

            user.NormalizedUserName = userDto.UserName.ToUpper();
            user.FullName = userDto.FullName;
            user.Gender = userDto.Gender;
            user.DateOfBirth = userDto.DateOfBirth;
            user.Email = userDto.Email;
            user.Remark = userDto.Remark;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, code, userDto.Password);
            }

            foreach (var role in userDto.Roles)
            {
                result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

        public async Task<List<SearchUserDto>> SearchUser(string keyword)
        {
            var users = await applicationDbContext.Users
                .Where(x => (keyword == null || x.UserName.Contains(keyword) || x.FullName.Contains(keyword) || x.Email.Contains(keyword)))
                .Select(x => new SearchUserDto()
                {
                    Id = x.Id,
                    Email = x.Email,
                    UserName = x.UserName,
                    Fullname = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    Remark = x.Remark,
                    Roles = applicationDbContext.Roles
                                .Where(y => applicationDbContext.UserRoles.Any(ur => ur.RoleId == y.Id && ur.UserId == x.Id))
                                .Select(y => y.Name).ToList()
                })
                .ToListAsync();

            return users;
        }
        public async Task<bool> DeleteUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            user.RecordActive = false;
            await userManager.UpdateAsync(user);
            return true;
        }
    }
}
