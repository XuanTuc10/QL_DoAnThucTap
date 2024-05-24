namespace QL_DoAnThucTap.Models.EF
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}
