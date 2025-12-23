using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Auth
{
    public class AddUserDto
    {
        public string? FullName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter Vaild Email")]
        public string Email { get; set; }
        public Roles Roles { get; set; }
        [Required(ErrorMessage = "Password Is Required !!")]
        [MinLength(6, ErrorMessage = "Password must be at least 4 characters long.")]
        public string? Password { get; set; }
    }
}
