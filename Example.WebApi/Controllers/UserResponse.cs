namespace Example.WebApi.Controllers
{
    public class UserResponse
    {
        public UserResponse(string name, Gender gender, Role role)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Gender = gender;
            Role = new RoleResponse(role);
        }

        public UserResponse(string name, Gender gender)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Gender = gender;
        }

        public UserResponse(string name, Role role)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Role = new RoleResponse(role);
        }


        public UserResponse(string name)
        {
            Name = name;
            CreateTime = DateTime.Now;
        }

        public string Name { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public Gender Gender { get; set; }
        public RoleResponse Role { get; set; }
    }

}