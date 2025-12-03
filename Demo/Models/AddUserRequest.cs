namespace Demo.Models
{
    public class AddUserRequest
    {
        public string email { get; set; } = null!;
        public string senha { get; set; } = null!;
        public string rg { get; set; } = null!;
    }
}
