using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class IrrigationScheduleDTO
    {
        [Required]
        public DateTime? TimeWorking { get; set; }
    }
}
