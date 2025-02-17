using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;
using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class DeviceDTO
    {
        public string? DeviceName { get; set; }
    }

    public class DeviceRoomDTO
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public int RoomId { get; set; }
    }

    public class DeviceWithRoomDTO
    {
        public Device Device { get; set; }
        public string RoomName { get; set; }
    }
}
