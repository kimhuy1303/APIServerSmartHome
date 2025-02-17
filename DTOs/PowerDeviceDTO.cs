using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class PowerDeviceDTO
    {
        [Required]
        public int PowerValue { get; set; }
        [Required]

        public DateTime TimeUsing { get; set; }
        [Required]

        public int? DeviceId { get; set; }
    }
}
