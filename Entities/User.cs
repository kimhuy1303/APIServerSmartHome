using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("User")]
    public class User : BasicEntity<User>
    {

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }  
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }

        public List<UserDevices> UserDevices { get; } = new List<UserDevices>();
        public List<UserFaces> UserFaces { get; } = new List<UserFaces>();
        public List<RFIDCard> RFIDCards { get; } = new List<RFIDCard>();
    }
}
