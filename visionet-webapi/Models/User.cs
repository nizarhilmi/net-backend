
namespace visionet_webapi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DayOfBirthday { get; set; }
        public Role Role { get; set; }
        public int? RoleId { get; set; }
    }
}
