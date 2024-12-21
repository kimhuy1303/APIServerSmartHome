using APIServerSmartHome.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("Device")]
    public class Device : BasicEntity<Device>
    {
        public string? DeviceName { get; set; }
        public State State { get; set; }
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public List<PowerDevice> PowerDevices { get; } = new List<PowerDevice>();

        public List<UserDevices> UserDevices { get; } = new List<UserDevices>(); 
        public List<OperateTimeWorking> OperateTimeWorkings { get; } = new List<OperateTimeWorking>();
    }
}
