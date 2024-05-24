using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using QL_DoAnThucTap.Models.EF;

namespace QL_DoAnThucTap.DataContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<ConfirmEmail> confirmEmails { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<Detail> details { get; set; }
        public DbSet<Progress> progresses { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<Reminder> reminders { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Topic> topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = COVINA; Database = QL_DoAnThucTap; Trusted_Connection = True; TrustServerCertificate = True;");
        }
    }
}
