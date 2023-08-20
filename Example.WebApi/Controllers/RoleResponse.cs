namespace Example.WebApi.Controllers
{
    public class RoleResponse
    {
        public RoleResponse(Role role)
        {
            Role = role;
        }

        public Role Role { get; set; }
    }

}