using Inventory.Mostafa.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Abstraction.Files
{
    public interface IFileServices<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        public string Upload(IFormFile file);
        public void Delete(string fileName);
    }
}
