using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string? Password { get; set; }
        
    }
}
