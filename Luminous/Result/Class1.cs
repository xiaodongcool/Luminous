using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContactFilterServiceCollectionExtensions
    {
        /// <summary>
        ///     格式化接口响应报文内容, 将所有接口请求响应报文内容格式化为 <see cref="IResult"/>
        /// </summary>
        public static void AddContactFilter(this IServiceCollection services)
        {
            services.AddContactProvider();
            services.AddControllers(mvc =>
            {
                mvc.Filters.TryAdd<AppendContactAndEnumMeaningFilter>();
                mvc.Filters.TryAdd<ModelBindFailFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContactProviderServiceCollectionExtensions
    {
        public static void AddContactProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IResultFactory, DefaultResultFactory>();
        }
    }
}


namespace Luminous
{
    public interface IResultFactory
    {
        /// <summary>
        ///     返回一个成功的响应报文
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">提示消息</param>
        IResult<T> Success<T>(T data = default, string message = default);

        /// <summary>
        ///     返回一个失败的响应报文
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="error">错误信息</param>
        /// <param name="exception">异常</param>
        IResult<T> Fail<T>(string message = default, object error = default, Exception exception = default);

        /// <summary>
        ///     返回一个失败的响应报文(入参错误)
        /// </summary>
        /// <param name="message">提示消息</param>
        IResult<T> ParameterError<T>(string message);

        /// <summary>
        ///     创建一个响应报文
        /// </summary>
        /// <param name="webApiStatusCode">响应状态码</param>
        /// <param name="data">响应数据</param>
        /// <param name="message">提示消息</param>
        /// <param name="error">错误信息</param>
        /// <param name="exception">异常</param>
        IResult<T> Create<T>(WebApiStatusCode webApiStatusCode, T data = default, string message = default, object error = default, Exception exception = default);
    }

    public class DefaultResultFactory : IResultFactory
    {
        public IResult<T> Success<T>(T data = default, string message = default)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.Success,
                Payload = data,
                Message = message ?? "请求成功"
            };
        }

        public IResult<T> Fail<T>(string message = default, object error = default, Exception exception = default)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.Fail,
                Message = message ?? "请求失败",
                Error = error,
                Exception = exception
            };
        }

        public IResult<T> ParameterError<T>(string message)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.ParameterError,
                Message = message
            };
        }

        public IResult<T> Create<T>(WebApiStatusCode webApiStatusCode, T data = default, string message = default, object error = default, Exception exception = default)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = webApiStatusCode switch
                {
                    WebApiStatusCode.Success => "请求成功",
                    WebApiStatusCode.Fail => "请求失败",
                    WebApiStatusCode.ParameterError => "请求参数错误",
                    WebApiStatusCode.UnAuthorized => "未通过授权",
                    WebApiStatusCode.Forbidden => "权限不足",
                    WebApiStatusCode.InternalServerError => "抱歉，服务器刚刚开小差了，请稍后再试",
                    _ => "未知错误"
                };
            }

            return new DefaultResult<T>
            {
                Status = webApiStatusCode,
                Message = message,
                Error = error,
                Exception = exception,
                Payload = data
            };
        }
    }
    /// <summary>
    ///     取消 <see cref="ContactFilter"/> 的作用
    /// </summary>
    public class UnContactAttribute : Attribute { }

    /// <summary>
    ///     WebApi 响应报文状态码
    /// </summary>
    public enum WebApiStatusCode
    {
        /// <summary>
        ///     成功
        /// </summary>
        [EnumMember(Value = "success")]
        Success,
        /// <summary>
        ///     失败
        /// </summary>
        [EnumMember(Value = "fail")]
        Fail,
        /// <summary>
        ///     入参错误
        /// </summary>
        [EnumMember(Value = "parameter_error")]
        ParameterError,
        /// <summary>
        ///     服务器错误
        /// </summary>
        [EnumMember(Value = "internal_server_error")]
        InternalServerError,
        /// <summary>
        ///     未通过授权
        /// </summary>
        [EnumMember(Value = "unauthorized")]
        UnAuthorized,
        /// <summary>
        ///     权限不足
        /// </summary>
        [EnumMember(Value = "forbidden")]
        Forbidden,
    }
    public static class ContactHttpContextExtensions
    {
        public static void SetMessage(this HttpContext httpContext, string message)
        {
            httpContext.Items["contact-msg"] = message;
        }

        public static void SetCode(this HttpContext httpContext, WebApiStatusCode code)
        {
            httpContext.Items["contact-code"] = code;
        }

        public static void SetContact(this HttpContext httpContext, WebApiStatusCode code, string message)
        {
            httpContext.Items["contact-code"] = code;
            httpContext.Items["contact-msg"] = message;
        }

        public static void GetContact(this HttpContext httpContext, out WebApiStatusCode code, out string message)
        {
            var variables = httpContext.Items;
            message = variables["contact-msg"]?.ToString() ?? "";
            code = (WebApiStatusCode)(variables["contact-code"] ?? WebApiStatusCode.Success);
        }
    }

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

                    if (type == typeof(DefaultResult))
                    {
                        return;
                    }

                    object result;
                    Type payloadType;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultResult<>).GetGenericTypeDefinition())
                    {
                        result = payload;

                        var property = payload.GetType().GetProperty(nameof(DefaultResult.Payload));

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
    /// <summary>
    ///     WebApi 接口标准约定
    /// </summary>
    public interface IResult<T>
    {
        /// <summary>
        ///     响应状态码
        /// </summary>
        WebApiStatusCode Status { get; set; }

        /// <summary>
        ///     消息提示,通常是一个精简的提示信息,比 status 更具体一点,可以直接显示给用户
        ///     例如：添加成功/失败，手机号码不能为空
        /// </summary>
        string? Message { get; set; }

        /// <summary>
        ///     有效响应数据
        /// </summary>
        T? Payload { get; set; }

        /// <summary>
        ///     错误信息(生产环境关闭)
        /// </summary>
        object? Error { get; set; }

        /// <summary>
        ///     异常信息(生产环境关闭)
        /// </summary>
        Exception? Exception { get; set; }
    }

    public interface IResult : IResult<object> { }

    public class DefaultResult<T> : IResult<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WebApiStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T? Payload { get; set; }
        public object? Error { get; set; }
        public Exception? Exception { get; set; }
    }

    public class DefaultResult : DefaultResult<object>
    {
    }

    public class DeserializeContact<T> : IResult<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WebApiStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T? Payload { get; set; }
        public object? Error { get; set; }
        [JsonIgnore]
        public Exception? Exception { get; set; }
    }
}
