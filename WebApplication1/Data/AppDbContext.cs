using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Account;

namespace WebApplication1.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<UserInfoModel> UserInfo { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
