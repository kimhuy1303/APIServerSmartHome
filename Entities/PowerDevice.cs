using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("PowerDevice")]
    public class PowerDevice : BasicEntity<PowerDevice>
    {
        public int PowerValue { get; set; }
        public float TimeUsing { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
