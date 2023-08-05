using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Luminous
{
    /// <summary>
    ///     重写响应报文的JSON格式
    /// </summary>
    public class ContactFilter : IActionFilter
    {
        private readonly IContactProvider _contactProvider;

        public ContactFilter(IContactProvider contactProvider)
        {
            _contactProvider = contactProvider;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                context.HttpContext.GetContact(out var statusCode, out var message);

                var payload = objectResult.Value;

                if (payload != null)
                {
                    var type = payload.GetType();

                    if (type == typeof(DefaultContact))
                    {
                        return;
                    }

                    object result;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultContact<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(DefaultContact.Payload));

                        Debug.Assert(property != null);

                    }
                    else
                    {
                        result = _contactProvider.Create(statusCode, payload, message);
                    }

                    context.Result = new JsonResult(result)
                    {
                        ContentType = "application/json",
                    };

                    return;
                }
                else
                {
                    var result = _contactProvider.Create(statusCode, payload, message);

                    context.Result = new JsonResult(result)
                    {
                        ContentType = "application/json",
                    };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }

    public class MeaningAttribute : Attribute
    {
        public MeaningAttribute(string mean)
        {
            Mean = mean;
        }

        public string Mean { get; }
    }
    public class PageInfo<T>
    {
        public PageInfo(int total, List<T> data)
        {
            Total = total;
            Data = data;
        }

        public PageInfo(int total, T[] data)
        {
            Total = total;
            Data = data;
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
