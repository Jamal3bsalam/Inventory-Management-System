using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Shared
{
    public class BaseEntity<Tkey>
    {
        public Tkey? Id { get; set; }
    }
}
