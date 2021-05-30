using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AargonTools.Models;

namespace AargonTools.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<ItemData> Items {get;set;}
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
            
        }
    }
}