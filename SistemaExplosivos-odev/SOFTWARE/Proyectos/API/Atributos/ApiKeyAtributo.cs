using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace API.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool existeHeader = context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey);

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (!existeHeader || extractedApiKey != appSettings["ApiKey"])
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Acceso Denegado"
                };

                return;
            }

            await next();
        }
    }
}