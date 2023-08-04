namespace LangM.AspNetCore
{
    public class RefreshAttribute : AuthorizeAttribute
    {
        public RefreshAttribute()
        {
            AuthenticationSchemes = TokenAuthenticationSchemes.RefreshToken;
        }
    }
}
