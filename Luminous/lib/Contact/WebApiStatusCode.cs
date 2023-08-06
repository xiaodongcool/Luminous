using System.Runtime.Serialization;

namespace Luminous
{
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
}
