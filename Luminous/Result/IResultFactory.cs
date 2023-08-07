namespace Luminous
{
    public interface IResultFactory
    {
        IResult<T> Create<T>(ResultStatus status, T? data = default, string? message = null);
        IResult<T> Fail<T>(string? message = null);
        IResult<T> ParameterError<T>(string? message = null);
        IResult<T> Success<T>(T? data = default, string? message = null);
    }
    public interface IDebugResultFactory
    {
        IResult<T> Create<T>(ResultStatus status, T? data = default, string? message = null);
        IResult<T> Fail<T>(string? message = null);
        IResult<T> ParameterError<T>(string? message = null);
        IResult<T> Success<T>(T? data = default, string? message = null);
    }
}
