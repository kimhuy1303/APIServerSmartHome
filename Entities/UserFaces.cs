using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("UserFaces")]
    public class UserFaces : BasicEntity<UserFaces>
    {
        public string? Name { get; set; }
        public string? FaceImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
