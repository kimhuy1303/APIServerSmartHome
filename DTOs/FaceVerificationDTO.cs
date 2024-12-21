using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class FaceVerificationDTO
    {
        [Required]
        public string? FaceImg { get; set; }   
    }
}
