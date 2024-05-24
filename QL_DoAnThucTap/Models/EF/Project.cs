using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }    
        public DateTime CreateDate { get; set; }    
        public DateTime UpdateDate { get; set; }
        [ForeignKey("TopicId")]
        public int? TopicId { get; set; }
        [ForeignKey("StudentId")]
        public int? StudentId { get; set; }
        [ForeignKey("TeacherId")]
        public int? TeacherId { get; set; }
        public Topic? Topic { get; set; }
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
        public virtual ICollection<Progress>? Progresses { get; set; }
    }
}
