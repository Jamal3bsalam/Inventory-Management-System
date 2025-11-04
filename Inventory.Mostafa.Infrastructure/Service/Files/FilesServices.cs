using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Service.Files
{
    public class FilesServices<TEntity,Tkey> : IFileServices<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilesServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string Upload(IFormFile file)
        {
            //D:\Inventory Management System\Inventory.Mostafa.Pl\wwwroot\Files\OrdersAttachment\
            string CurrentDirectory = Directory.GetCurrentDirectory();
            var FolderName = GetFolderName(typeof(TEntity).Name);
            string folderPath = (CurrentDirectory + $"\\wwwroot\\Files\\{FolderName}");

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            string filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }
        public void Delete(string fileName)
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string filePath = (CurrentDirectory + $"\\wwwroot\\{fileName}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetFolderName(string entityName)
        {
            return entityName switch
            {
                "Orders" => "OrdersAttachment",
                "StoreRelease" => "StoreRelease",
                "UnitExpense" => "UnitExpense",
                "CustodayRecord" => "CustodayRecord",
                "Returns" => "Returns",
                "WriteOff" => "WriteOff",
                "CustodayTransfers" => "CustodayTransfers",
                _ => "General"
            };
        }
    }
}
