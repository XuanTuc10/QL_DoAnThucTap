using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Progress : BaseEntity
    {
        public string Title { get; set; }   
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ProjectId")]
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        public virtual ICollection<Detail>? Details { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}
