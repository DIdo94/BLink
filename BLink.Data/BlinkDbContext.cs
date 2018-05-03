using BLink.Core.Constants;
using BLink.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BLink.Data
{
    public class BlinkDbContext : IdentityDbContext<IdentityUser>
    {
        public BlinkDbContext()
        {

        }

        public BlinkDbContext(DbContextOptions<BlinkDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MemberPositions>()
                .HasKey(mp => new { mp.MemberId, mp.PostitionId });
            builder.Entity<MemberPositions>()
                .HasOne(mp => mp.Member)
                .WithMany(m => m.MemberPositions)
                .HasForeignKey(k => k.MemberId);
            builder.Entity<MemberPositions>()
                .HasOne(mp => mp.Position)
                .WithMany(p => p.MemberPositions)
                .HasForeignKey(mp => mp.PostitionId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppConstants.ConnectionStrings[AppConstants.BlinkConnectionKey]);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<MemberPositions> MemberPositions { get; set; }

        public DbSet<Invitation> Invitations { get; set; }
    }
}
