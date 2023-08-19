namespace Luminous
{
    public class RefreshAttribute : AuthorizeAttribute
    {
        public RefreshAttribute()
        {
            AuthenticationSchemes = TokenAuthenticationSchemes.RefreshToken;
        }
    }
}
