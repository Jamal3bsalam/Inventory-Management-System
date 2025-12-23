using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Auth
{
    public class ResetPassword
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
