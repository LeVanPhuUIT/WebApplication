using System;
using System.Collections.Generic;

namespace WebApplication.Services.User.Dto
{
    public class SearchUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Remark { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
    }
}
