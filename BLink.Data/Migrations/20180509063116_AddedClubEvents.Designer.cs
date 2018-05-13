using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BLink.Data;
using BLink.Models.Enums;

namespace BLink.Data.Migrations
{
    [DbContext(typeof(BlinkDbContext))]
    [Migration("20180509063116_AddedClubEvents")]
    partial class AddedClubEvents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BLink.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("BLink.Models.ClubEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClubId");

                    b.Property<string>("Description");

                    b.Property<int>("EventType");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("ClubEvents");
                });

            modelBuilder.Entity("BLink.Models.ClubEventMember", b =>
                {
                    b.Property<int>("ClubEventId");

                    b.Property<int>("MemberId");

                    b.HasKey("ClubEventId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("ClubEventMember");
                });

            modelBuilder.Entity("BLink.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("PhotoPath");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BLink.Models.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("InvitedPlayerId");

                    b.Property<int?>("InvitingClubId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("InvitedPlayerId");

                    b.HasIndex("InvitingClubId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("BLink.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClubId");

                    b.Property<int?>("CountryId");

                    b.Property<string>("FirstName");

                    b.Property<double?>("Height");

                    b.Property<string>("LastName");

                    b.Property<string>("PhotoPath");

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("CountryId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("BLink.Models.MemberPositions", b =>
                {
                    b.Property<int>("MemberId");

                    b.Property<int>("PostitionId");

                    b.HasKey("MemberId", "PostitionId");

                    b.HasIndex("PostitionId");

                    b.ToTable("MemberPositions");
                });

            modelBuilder.Entity("BLink.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BLink.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser");

                    b.Property<int>("MemberId");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("ApplicationUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("BLink.Models.Club", b =>
                {
                    b.HasOne("BLink.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("BLink.Models.ClubEvent", b =>
                {
                    b.HasOne("BLink.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId");
                });

            modelBuilder.Entity("BLink.Models.ClubEventMember", b =>
                {
                    b.HasOne("BLink.Models.ClubEvent", "ClubEvent")
                        .WithMany("InvitedMembers")
                        .HasForeignKey("ClubEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BLink.Models.Member", "Member")
                        .WithMany("ClubEvents")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BLink.Models.Invitation", b =>
                {
                    b.HasOne("BLink.Models.Member", "InvitedPlayer")
                        .WithMany()
                        .HasForeignKey("InvitedPlayerId");

                    b.HasOne("BLink.Models.Club", "InvitingClub")
                        .WithMany()
                        .HasForeignKey("InvitingClubId");
                });

            modelBuilder.Entity("BLink.Models.Member", b =>
                {
                    b.HasOne("BLink.Models.Club", "Club")
                        .WithMany("Members")
                        .HasForeignKey("ClubId");

                    b.HasOne("BLink.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("BLink.Models.MemberPositions", b =>
                {
                    b.HasOne("BLink.Models.Member", "Member")
                        .WithMany("MemberPositions")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BLink.Models.Position", "Position")
                        .WithMany("MemberPositions")
                        .HasForeignKey("PostitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BLink.Models.ApplicationUser", b =>
                {
                    b.HasOne("BLink.Models.Member", "Member")
                        .WithOne("IdentityUser")
                        .HasForeignKey("BLink.Models.ApplicationUser", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
