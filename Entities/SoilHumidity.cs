using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("SoilHumidity")]
    public class SoilHumidity : BasicEntity<SoilHumidity>
    {
        public int Value { get; set; }
        public DateTime TimeSpan { get; set; }
    }
}
