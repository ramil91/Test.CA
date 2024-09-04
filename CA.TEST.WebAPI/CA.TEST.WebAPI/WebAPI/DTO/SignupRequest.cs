namespace WebAPI.DTO
{
    public class SignupRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Device { get; set; }
        public string IPAddress { get; set; }
    }
}
