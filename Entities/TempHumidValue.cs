using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("TempHumidValue")]
    public class TempHumidValue : BasicEntity<TempHumidValue>
    {
        public float Temperature {  get; set; } 
        public int Humidity { get; set; }
        public DateTime? TimeSpan { get; set; }
    }
}
