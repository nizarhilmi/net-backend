using visionet_webapi.Models;

namespace visionet_webapi.Common.Command
{
    public class UserCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DayOfBirthday { get; set; }
        public int? RoleId { get; set; }
    }
}
