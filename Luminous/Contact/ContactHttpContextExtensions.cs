namespace Luminous
{
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
}
