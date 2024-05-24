namespace QL_DoAnThucTap.Models.EF
{
    public class Class : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Student>? Students { get; set; }

    }
}
