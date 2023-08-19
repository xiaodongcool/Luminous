namespace Luminous
{
    public static class ResultHttpContextExtensions
    {
        public static void SetMessage(this HttpContext httpContext, string message)
        {
            httpContext.Items["contact-msg"] = message;
        }

        public static void SetCode(this HttpContext httpContext, ResultStatus code)
        {
            httpContext.Items["contact-code"] = code;
        }

        public static void SetContact(this HttpContext httpContext, ResultStatus code, string message)
        {
            httpContext.Items["contact-code"] = code;
            httpContext.Items["contact-msg"] = message;
        }

        public static void GetContact(this HttpContext httpContext, out ResultStatus code, out string message)
        {
            var variables = httpContext.Items;
            message = variables["contact-msg"]?.ToString() ?? "";
            code = (ResultStatus)(variables["contact-code"] ?? ResultStatus.Success);
        }
    }
}
