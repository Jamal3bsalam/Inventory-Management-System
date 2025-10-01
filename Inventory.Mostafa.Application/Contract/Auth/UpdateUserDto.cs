using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Auth
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public Roles? Roles { get; set; }
    }
}
