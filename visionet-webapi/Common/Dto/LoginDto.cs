namespace visionet_webapi.Common.Dto
{
    public class LoginDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
    }
}
