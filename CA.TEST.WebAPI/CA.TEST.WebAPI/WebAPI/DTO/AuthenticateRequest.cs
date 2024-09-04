namespace WebAPI.DTO
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
    }
}
