using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Detail : BaseEntity
    {
        public string FileUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ProgressId")]
        public int? ProgressId { get; set; }   
        public Progress Progress { get; set; }   

    }
}
