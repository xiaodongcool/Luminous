﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using MySqlX.XDevAPI.Common;

namespace Luminous
{
    public class UnifyResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.GetContact(out var statusCode, out var message);

            if (context.Result is ObjectResult objectResult)
            {
                var payload = objectResult.Value;

                if (payload != null)
                {
                    var type = payload.GetType();

                    object result;
                    Type payloadType;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(Result<int>.Payload));

                        Debug.Assert(property != null);

                        payloadType = property.PropertyType;
                    }
                    else
                    {
                        Debug.Assert(objectResult.DeclaredType != null);

                        result = DynamicResultActivator.Create(statusCode, payload, objectResult.DeclaredType, message);
                    }

                    context.Result = new ObjectResult(result);

                    return;
                }
                else
                {
                    var result = new Result<object>(statusCode, payload, message);

                    context.Result = new JsonResult(result);
                }
            }
            else if (context.Result is EmptyResult)
            {
                var result = new Result<object>(statusCode, null, message);

                context.Result = new JsonResult(result);
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
