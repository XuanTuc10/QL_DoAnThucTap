namespace QL_DoAnThucTap.Models.EF
{
    public class ConfirmEmail : BaseEntity
    {
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirm { get; set; } = false;
    }
}
