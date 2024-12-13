using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Tên người dùng là bắt buộc")]
        [StringLength(20, MinimumLength =3, ErrorMessage ="Tên người dùng phải từ 3 đến 20 ký tự.")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }
        
    }
}
