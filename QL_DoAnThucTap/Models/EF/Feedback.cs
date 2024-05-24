using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Feedback : BaseEntity
    {
        public string FeedBack { get; set; }
        public DateTime Createtime { get; set; }
        [ForeignKey("ProgressId")]
        public int ProgressId { get; set; }
        public Progress Progress { get; set; }
    }
}
