using APIServerSmartHome.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("OperateTimeWorking")]
    public class OperateTimeWorking : BasicEntity<OperateTimeWorking>
    {
        public State State { get; set; }
        public DateTime? OperatingTime { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
