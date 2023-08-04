using Newtonsoft.Json.Converters;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     WebApi 接口标准约定
    /// </summary>
    public interface IContact<T>
    {
        /// <summary>
        ///     响应状态码
        /// </summary>
        WebApiStatusCode Status { get; set; }

        /// <summary>
        ///     消息提示,通常是一个精简的提示信息,比 status 更具体一点,可以直接显示给用户
        ///     例如：添加成功/失败，手机号码不能为空
        /// </summary>
        string Message { get; set; }

        /// <summary>
        ///     有效响应数据
        /// </summary>
        T Data { get; set; }

        /// <summary>
        ///     错误信息(生产环境关闭)
        /// </summary>
        object Error { get; set; }

        /// <summary>
        ///     异常信息(生产环境关闭)
        /// </summary>
        Exception Exception { get; set; }
    }

    public interface IContact : IContact<object> { }
   
    public class DefaultContact : IContact
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WebApiStatusCode Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Error { get; set; }
        public Exception Exception { get; set; }
    }

    public class DefaultContact<T> : IContact<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WebApiStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Error { get; set; }
        public Exception Exception { get; set; }
    }

    public class DeserializeContact<T> : IContact<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WebApiStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Error { get; set; }
        [JsonIgnore]
        public Exception Exception { get; set; }
    }
}
