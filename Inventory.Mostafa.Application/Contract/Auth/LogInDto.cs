using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Auth
{
    public class LogInDto
    {
        [Required(ErrorMessage = "UserName Is Required !!")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password Is Required !!")]
        public string? Password { get; set; }
    }
}
