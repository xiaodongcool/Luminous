﻿using Microsoft.AspNetCore.Mvc.Filters;
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
                    Type payloadType;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultContact<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(DefaultContact.Payload));

                        Debug.Assert(property != null);

                        payloadType = property.PropertyType;
                    }
                    else
                    {
                        result = _contactProvider.Create(statusCode, payload, message);
                        payloadType = payload.GetType();
                    }

                    result = SerializeAndAddEnumDescriptionReturnObj(result, payloadType);

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

        public object SerializeAndAddEnumDescriptionReturnObj(object result, Type payloadType)
        {
            var jObject = JToken.FromObject(result);

            RecursivelyAddEnumMeaning(jObject, payloadType);

            return jObject.ToObject<ExpandoObject>() ?? new ExpandoObject();
        }

        private void RecursivelyAddEnumMeaning(JToken token, Type inputType)
        {
            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>().ToList())
                {
                    RecursivelyAddEnumMeaning(property.Value, inputType);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken childToken in token.Children())
                {
                    RecursivelyAddEnumMeaning(childToken, inputType);
                }
            }
            else if (token.Type == JTokenType.Integer)
            {
                if (token.Parent is JProperty parentProperty)
                {
                    if (parentProperty != null && parentProperty.Parent != null)
                    {
                        var path = parentProperty.Path;

                        if (path.StartsWith("Payload."))
                        {
                            path = path.Substring(8);
                        }

                        var propertyInfo = FindPropertyByPath(inputType, path);

                        if (propertyInfo != null && propertyInfo.PropertyType.IsEnum)
                        {
                            var enumValue = Enum.ToObject(propertyInfo.PropertyType, (int)token);
                            var enumDescription = GetEnumMean(enumValue);
                            parentProperty.Parent.Add(new JProperty(parentProperty.Name + "Meaning", enumDescription));
                        }
                    }
                }
            }
        }

        private string GetEnumMean(object enumValue)
        {
            var value = enumValue.ToString();

            Debug.Assert(value != null);

            var fieldInfo = enumValue.GetType().GetField(value);

            if (fieldInfo == null)
            {
                return value;
            }

            var mean = fieldInfo.GetCustomAttribute<MeaningAttribute>()?.Mean
                ?? fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? fieldInfo.GetCustomAttribute<DisplayAttribute>()?.Name;

            return mean ?? value;
        }

        public PropertyInfo? FindPropertyByPath(Type targetType, string path)
        {
            var segments = path.Split('.');

            PropertyInfo? propertyInfo = null;
            Type currentType = targetType;

            foreach (string segment in segments)
            {
                var ienumerableInterface = currentType.GetInterface("IEnumerable`1");

                if (ienumerableInterface != null)
                {
                    currentType = ienumerableInterface.GetGenericArguments()[0];
                }
                else
                {
                    if (Regex.IsMatch(segment, @"\[\d+\]$"))
                    {
                        var temp = Regex.Replace(segment, @"\[\d+\]$", "");

                        propertyInfo = currentType.GetProperty(temp);

                        if (propertyInfo != null)
                        {
                            currentType = propertyInfo.PropertyType.GetGenericArguments()[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        propertyInfo = currentType.GetProperty(segment);

                        if (propertyInfo != null)
                        {
                            currentType = propertyInfo.PropertyType;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return propertyInfo;
        }

    }

    /// <summary>
    ///     重写响应报文的JSON格式
    /// </summary>
    public class AppendEnumMeaningFilter : IActionFilter
    {
        private readonly IContactProvider _contactProvider;

        public AppendEnumMeaningFilter(IContactProvider contactProvider)
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
                    Type payloadType;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultContact<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(DefaultContact.Payload));

                        Debug.Assert(property != null);

                        payloadType = property.PropertyType;
                    }
                    else
                    {
                        result = _contactProvider.Create(statusCode, payload, message);
                        payloadType = payload.GetType();
                    }

                    result = SerializeAndAddEnumDescriptionReturnObj(result, payloadType);

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

        public object SerializeAndAddEnumDescriptionReturnObj(object result, Type payloadType)
        {
            var jObject = JToken.FromObject(result);

            RecursivelyAddEnumMeaning(jObject, payloadType);

            return jObject.ToObject<ExpandoObject>() ?? new ExpandoObject();
        }

        private void RecursivelyAddEnumMeaning(JToken token, Type inputType)
        {
            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>().ToList())
                {
                    RecursivelyAddEnumMeaning(property.Value, inputType);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken childToken in token.Children())
                {
                    RecursivelyAddEnumMeaning(childToken, inputType);
                }
            }
            else if (token.Type == JTokenType.Integer)
            {
                if (token.Parent is JProperty parentProperty)
                {
                    if (parentProperty != null && parentProperty.Parent != null)
                    {
                        var path = parentProperty.Path;

                        if (path.StartsWith("Payload."))
                        {
                            path = path.Substring(8);
                        }

                        var propertyInfo = FindPropertyByPath(inputType, path);

                        if (propertyInfo != null && propertyInfo.PropertyType.IsEnum)
                        {
                            var enumValue = Enum.ToObject(propertyInfo.PropertyType, (int)token);
                            var enumDescription = GetEnumMean(enumValue);
                            parentProperty.Parent.Add(new JProperty(parentProperty.Name + "Meaning", enumDescription));
                        }
                    }
                }
            }
        }

        private string GetEnumMean(object enumValue)
        {
            var value = enumValue.ToString();

            Debug.Assert(value != null);

            var fieldInfo = enumValue.GetType().GetField(value);

            if (fieldInfo == null)
            {
                return value;
            }

            var mean = fieldInfo.GetCustomAttribute<MeaningAttribute>()?.Mean
                ?? fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? fieldInfo.GetCustomAttribute<DisplayAttribute>()?.Name;

            return mean ?? value;
        }

        public PropertyInfo? FindPropertyByPath(Type targetType, string path)
        {
            var segments = path.Split('.');

            PropertyInfo? propertyInfo = null;
            Type currentType = targetType;

            foreach (string segment in segments)
            {
                var ienumerableInterface = currentType.GetInterface("IEnumerable`1");

                if (ienumerableInterface != null)
                {
                    currentType = ienumerableInterface.GetGenericArguments()[0];
                }
                else
                {
                    if (Regex.IsMatch(segment, @"\[\d+\]$"))
                    {
                        var temp = Regex.Replace(segment, @"\[\d+\]$", "");

                        propertyInfo = currentType.GetProperty(temp);

                        if (propertyInfo != null)
                        {
                            currentType = propertyInfo.PropertyType.GetGenericArguments()[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        propertyInfo = currentType.GetProperty(segment);

                        if (propertyInfo != null)
                        {
                            currentType = propertyInfo.PropertyType;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return propertyInfo;
        }

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
