using visionet_webapi.Models;

namespace visionet_webapi.Common.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DayOfBirthday { get; set; }
    }
}
