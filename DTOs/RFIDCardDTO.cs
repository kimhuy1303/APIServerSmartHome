using APIServerSmartHome.Enum;
using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class RFIDCardDTO
    {
        [Required]
        public string? CardUID { get; set; }
        
    }
}
