using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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
            Old(context);
        }

        private void Old(ActionExecutedContext context)
        {
            if (context.Exception != null || !context.ModelState.IsValid)
            {
                return;
            }

            var result = context.Result;

            if (result is ViewResult || result is RedirectResult || result is RedirectToActionResult)
            {
                return;
            }

            var action = context.ActionDescriptor as ControllerActionDescriptor;
            if (action.MethodInfo.GetCustomAttribute<UnContactAttribute>() != null)
            {
                return;
            }

            context.HttpContext.GetContact(out var statusCode, out var message);

            if (context.Result is ObjectResult objectResult)
            {
                var model = objectResult.Value;

                if (model != null)
                {
                    var type = model.GetType();

                    if (type == typeof(DefaultContact))
                    {
                        return;
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultContact<>).GetGenericTypeDefinition())
                    {
                        return;
                    }
                }

                if (objectResult.Value == null && statusCode == WebApiStatusCode.Success)
                {
                    statusCode = WebApiStatusCode.Fail;
                }
            }

            var convention = _contactProvider.Create(statusCode, (context.Result as ObjectResult)?.Value, message);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            context.Result = new JsonResult(SerializeAndAddEnumDescriptionReturnObj(convention))
            {
                ContentType = "application/json",
            };
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public object? SerializeAndAddEnumDescriptionReturnObj(IContact<object?>? input)
        {
            if (input == null)
            {
                return null;
            }

            JToken jsonObject = JToken.FromObject(input);
            RecursivelyAddEnumDescriptions(jsonObject, input.Data?.GetType());

            return jsonObject.ToObject<ExpandoObject>();
        }

        public string SerializeAndAddEnumDescription(IContact<object?>? input)
        {
            if (input == null)
            {
                return null;
            }

            JToken jsonObject = JToken.FromObject(input);
            RecursivelyAddEnumDescriptions(jsonObject, input.Data?.GetType());

            return jsonObject.ToString();
        }

        private void RecursivelyAddEnumDescriptions(JToken token, Type? inputType)
        {
            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>().ToList())
                {
                    RecursivelyAddEnumDescriptions(property.Value, inputType);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken childToken in token.Children())
                {
                    RecursivelyAddEnumDescriptions(childToken, inputType);
                }
            }
            else if (token.Type == JTokenType.Integer)
            {
                JProperty parentProperty = token.Parent as JProperty;
                if (parentProperty != null)
                {
                    PropertyInfo propertyInfo = GetPropertyInfoFromPath(inputType, parentProperty.Path);
                    if (propertyInfo != null && propertyInfo.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.ToObject(propertyInfo.PropertyType, (int)token);
                        var enumDescription = GetEnumDescription(enumValue);
                        //parentProperty.Parent.AddAfterSelf(d);
                        parentProperty.Parent.Add(new JProperty(parentProperty.Name + "Description", enumDescription));
                    }
                }
            }
        }

        private string GetEnumDescription(object enumValue)
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return enumValue.ToString(); // Fallback to enum value if no description found
        }
        PropertyInfo GetPropertyInfoFromPath(Type objectType, string propertyPath)
        {
            PropertyInfo propertyInfo = null;
            string[] pathParts = propertyPath.Split('.').Skip(1).ToArray();

            foreach (string part in pathParts)
            {
                propertyInfo = objectType.GetProperty(part);
                if (propertyInfo == null)
                {
                    return null;
                }
                objectType = propertyInfo.PropertyType;
            }

            return propertyInfo;
        }
    }
}
