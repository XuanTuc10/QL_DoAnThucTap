namespace QL_DoAnThucTap.Models.EF
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Project>? Projects { get; set; }
    }
}
