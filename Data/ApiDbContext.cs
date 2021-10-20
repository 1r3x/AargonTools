using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AargonTools.Models;

namespace AargonTools.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<TestApiData> TestApiData {get;set;}
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public virtual DbSet<WebApiLogs> WebApiLogs { get;set;}
        public virtual DbSet<MoveAccountApiLogs> MoveAccountApiLogs { get;set;}
        public virtual DbSet<ApiMoveSetting> ApiMoveSettings { get; set; }
        public virtual DbSet<ApiMoveLog> ApiMoveLogs { get; set; }
        public virtual DbSet<InteractResult> InteractResults { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
            
        }
    }
}