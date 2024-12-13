using System.ComponentModel.DataAnnotations;

namespace APIServerSmartHome.Entities
{
    public class BasicEntity<T> 
    {
        [Key]
        public int Id { get; set; }
    }

}
