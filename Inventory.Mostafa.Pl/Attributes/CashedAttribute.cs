using Inventory.Mostafa.Application.Abstraction.Cash;
using Inventory.Mostafa.Infrastructure.Service.Cashe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Inventory.Mostafa.Pl.Attributes
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CashedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetService<ICashService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // 1️⃣ لو فيه داتا في الكاش، رجعها على طول
            var cachedData = await cacheService!.GetAsync<string>(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedData,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            // 2️⃣ لو مفيش، نفذ الـ Action
            var executedContext = await next();

            // 3️⃣ لو فيه نتيجة من الـ Action خزّنها في الكاش
            if (executedContext.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                var jsonData = JsonSerializer.Serialize(objectResult.Value);
                await cacheService.SetAsync(cacheKey, jsonData, TimeSpan.FromSeconds(_expireTime));
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            return $"{request.Path}-{request.QueryString}";
        }
    }
}
