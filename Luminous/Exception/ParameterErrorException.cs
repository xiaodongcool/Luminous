namespace Luminous
{
    public class ParameterErrorException : FailException
    {
        public ParameterErrorException(string resultMessage) : base(ResultStatus.ParameterError, resultMessage, null, false) { }
        public ParameterErrorException(string resultMessage, bool logOnGlobalException) : base(ResultStatus.ParameterError, resultMessage, null, logOnGlobalException) { }
    }
}
