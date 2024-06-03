using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_DoAnThucTap.Models.EF
{
    public class Account : BaseEntity
    {
        [Required(ErrorMessage ="Email is requied")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requied")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("RoleId")]  
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
