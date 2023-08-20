using Luminous;

namespace Example.WebApi.Controllers
{
    public enum Role
    {
        [Meaning("用户")]
        User = 1,
        [Meaning("管理员")]
        Admin = 2
    }

}