using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayRec
{
    public class RecordDto
    {
        public int? Id { get; set; }
        public string? Notic { get; set; }
        public DateOnly? Date { get; set; }
        public string? FileUrl { get; set; }
    }
}
