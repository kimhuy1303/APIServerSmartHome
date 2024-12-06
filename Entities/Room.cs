using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("Room")]
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string? RoomName { get; set; }

        public List<Device> Devices { get; } = new List<Device>();
    }
}
