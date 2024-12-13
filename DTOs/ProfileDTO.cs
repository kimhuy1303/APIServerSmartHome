using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.DTOs
{
    public class ProfileDTO
    {
        //public int Id { get; set; }
        public string? FullName { get; set; }

        [RegularExpression(@"^(?:\+84|0)([35789][0-9]{8})$",
        ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phonenumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
