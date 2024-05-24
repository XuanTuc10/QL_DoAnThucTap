namespace QL_DoAnThucTap.Models.EF
{
    public class Role : BaseEntity
    {
        public string Code { get; set; }    
        public string Name { get; set; }
        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
