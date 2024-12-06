using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }

        public List<UserDevices> UserDevices { get; } = new List<UserDevices>();
    }
}
