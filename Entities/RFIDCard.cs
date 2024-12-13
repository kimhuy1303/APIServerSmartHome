using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("RFIDCard")]
    public class RFIDCard : BasicEntity<RFIDCard>
    {
        public string? CardUID {  get; set; }
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
