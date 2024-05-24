using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Reminder : BaseEntity
    {
        public string Title { get; set; }   
        public string Content { get; set; } 
        public string ShortContent { get; set; } 
        public DateTime Createtime { get; set; }
        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

    }
}
