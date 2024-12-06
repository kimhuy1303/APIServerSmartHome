using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("UserDevices")]
    public class UserDevices
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }

        public string? Password { get; set; }
    }
}
