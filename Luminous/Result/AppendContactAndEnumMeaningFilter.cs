using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Luminous
{
    public class AppendContactAndEnumMeaningFilter : IActionFilter
    {
        private readonly IResultFactory _contactProvider;
        private readonly IConfiguration _configuration;

        public AppendContactAndEnumMeaningFilter(IResultFactory contactProvider, IConfiguration configuration)
        {
            _contactProvider = contactProvider;
            _configuration = configuration;
        }

        public void OnActionExecuted(ActionExecutedContext context)
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

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultResult<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(DefaultResult<int>.Payload));

                        Debug.Assert(property != null);

                        payloadType = property.PropertyType;
                    }
                    else
                    {
                        result = _contactProvider.Create(statusCode, payload, message);
                        payloadType = payload.GetType();
                    }

                    result = AppendEnumMeaningIfEnableConfiguration(result, payloadType);

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
            else if (context.Result is EmptyResult emptyResult)
            {
                var result = _contactProvider.Create<object>(statusCode, null, message);

                context.Result = new JsonResult(result)
                {
                    ContentType = "application/json",
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        private object AppendEnumMeaningIfEnableConfiguration(object result, Type payloadType)
        {
            if (_configuration["Luminous:AppendEnumMeaning"] == "True")
            {
                result = SerializeAndAddEnumDescriptionReturnObj(result, payloadType);
            }

            return result;
        }

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
                            var enumDescription = GetEnumMeaning(enumValue);
                            parentProperty.Parent.Add(new JProperty(parentProperty.Name + "Meaning", enumDescription));
                        }
                    }
                }
            }
        }

        private string GetEnumMeaning(object enumValue)
        {
            var value = enumValue.ToString();

            Debug.Assert(value != null);

            var fieldInfo = enumValue.GetType().GetField(value);

            if (fieldInfo == null)
            {
                return value;
            }

            var meaning = fieldInfo.GetCustomAttribute<MeaningAttribute>()?.Mean
                ?? fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? fieldInfo.GetCustomAttribute<DisplayAttribute>()?.Name;

            return meaning ?? value;
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
}
