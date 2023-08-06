namespace Luminous
{
    /// <summary>
    ///     WebApi 接口标准约定
    /// </summary>
    public interface IResult<T>
    {
        /// <summary>
        ///     响应状态码
        /// </summary>
        ResultStatus Status { get; set; }

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
}
