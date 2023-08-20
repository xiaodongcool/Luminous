namespace Authentication.WebApi.Model
{
    public class LoginRequest
    {
        /// <summary>
        ///     账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }
    }
}
