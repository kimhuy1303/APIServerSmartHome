using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class TempHumidDTO
    {
        [Required]
        public float Temperature { get; set; }
        [Required]
        public int Humidity { get; set; }
        [Required]
        public DateTime? TimeSpan { get; set; }
    }
}
