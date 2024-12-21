using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("IrrigationSchedule")]
    public class IrrigationSchedule : BasicEntity<IrrigationSchedule>
    {
        public DateTime? TimeWorking { get; set; }
        public bool IsActive { get; set; }
    }
}
