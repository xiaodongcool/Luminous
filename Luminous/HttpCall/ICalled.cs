namespace LangM.AspNetCore
{
    public interface ICalled<T>
    {
        public Exception Exception { get; set; }
        public T Response { get; set; }
    }
}
