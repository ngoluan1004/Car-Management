using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_Car.Models
{
    [Table ("tblComment")]
    public class Comments
    {
        [Key]
        public int comment_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public int car_id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string content { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"^\d.*")]
        public string VerifyKey {get; set;}
    }
}