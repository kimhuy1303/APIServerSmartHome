using APIServerSmartHome.Enum;
using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class RFIDCardDTO
    {
        [Required]
        public string? CardUID { get; set; }
        
    }
    public class RFIDCardUpdateDTO
    {
        public string? CardUID { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsActive { get; set; }
    }
}
