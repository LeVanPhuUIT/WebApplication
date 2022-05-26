using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [MaxLength(255)]
        public string FullName { get; set; }
        [MaxLength(15)]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(255)]
        public string Remark { get; set; }
        [DefaultValue(true)]
        public bool RecordActive { get; set; }
    }
}
