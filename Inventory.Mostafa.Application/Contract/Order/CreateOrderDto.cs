using CommandLine.Text;
using Inventory.Mostafa.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Order number is required.")]
        public string OrderNumber { get; set; } // رقم الأمر
        [Required(ErrorMessage = "Order type is required.")]
        public string OrderType { get; set; }      // النوع (تعميد - أمر شراء - دعم)
        [Required(ErrorMessage = "Supplier name is required.")]
        public string SupplierName { get; set; }      // اسم المورد 
        [Required(ErrorMessage = "Recipint name is required.")]
        public string RecipintName { get; set; }
        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.Date)]
        [SwaggerSchema(Format = "date", Description = "Format: yyyy-MM-dd")]
        public DateOnly OrderDate { get; set; }      // التاريخ
        public string? Attachment { get; set; }
        [Required(ErrorMessage = "At least one item is required.")]
        public List<CreateOrderItem>? Items { get; set; }

    }
}
