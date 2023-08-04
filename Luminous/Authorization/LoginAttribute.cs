namespace Luminous
{
    public class LoginAttribute : AuthorizeAttribute
    {
        public LoginAttribute()
        {
            AuthenticationSchemes = TokenAuthenticationSchemes.Token;
        }
    }
}
