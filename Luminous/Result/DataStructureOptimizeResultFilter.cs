using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Luminous
{
    public class DataStructureOptimizeResultFilter : IResultFilter
    {
        private readonly IConfiguration _configuration;

        public DataStructureOptimizeResultFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value != null)
                {
                    Debug.Assert(objectResult.DeclaredType != null);

                    var result = AppendEnumMeaningIfEnableConfiguration(objectResult.Value, objectResult.DeclaredType);

                    objectResult.Value = result;
                }
            }
        }

        private object AppendEnumMeaningIfEnableConfiguration(object result, Type payloadType)
        {
            if (_configuration.GetValue<bool>("Luminous:AspNetCore:AppendEnumMeaning"))
            {
                result = SerializeAndAddEnumDescriptionReturnObj(result, payloadType);
            }

            return result;
        }

        public object SerializeAndAddEnumDescriptionReturnObj(object result, Type payloadType)
        {
            var jObject = JToken.FromObject(result);

            RecursivelyAddEnumMeaning(jObject, payloadType);

            return ConvertJTokenToObject(jObject);
        }

        private object ConvertJTokenToObject(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                //var jObject = (IDictionary<string, object>)token.ToObject(typeof(Dictionary<string, object>));
                //return jObject; 

                return token;
            }
            else if (token.Type == JTokenType.Array)
            {
                var jArray = (IList<object>)token.ToObject(typeof(List<object>));

                Debug.Assert(jArray != null);

                return jArray;
            }
            else
            {
                return ((dynamic)token).Value;
            }
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

                if (currentType.Name == "IEnumerable`1")
                {
                    currentType = currentType.GetGenericArguments()[0];
                }
                else if (ienumerableInterface != null)
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
