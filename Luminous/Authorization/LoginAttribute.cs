namespace LangM.AspNetCore
{
    public class LoginAttribute : AuthorizeAttribute
    {
        public LoginAttribute()
        {
            AuthenticationSchemes = TokenAuthenticationSchemes.Token;
        }
    }
}
