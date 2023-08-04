namespace LangM.AspNetCore
{
    public class Called<T> : ICalled<T>
    {
        public Called(T response, Exception exception)
        {
            Response = response;
            Exception = exception;
        }
        public Exception Exception { get; set; }
        public T Response { get; set; }
    }
}
