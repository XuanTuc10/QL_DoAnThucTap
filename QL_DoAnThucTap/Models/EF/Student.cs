using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Student : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [ForeignKey("AccountId")]
        public int ClassId { get; set; }
        [ForeignKey("AccountId")]
        public int AccountId { get; set; }
        public Class Class { get; set; }
        public Account Account { get; set; }
        public virtual ICollection<Project>? Projects { get; set; }
    }
}
