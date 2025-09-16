using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Auth
{
    public class AuthDto
    {
        public string? UserName { get; set; }
        public string? Token { get; set; }
    }
}
