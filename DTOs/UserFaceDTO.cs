using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class UserFaceDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? FaceImage { get; set; } // dạng base64
    }
}
