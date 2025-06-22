// using API.Entities;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// namespace API.Data;

// public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole,
//  int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
//  IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
// {
//     // public DbSet<AppUser> Users { get; set; }
//     public DbSet<UserLike> Likes { get; set; }
//     public DbSet<Message> Messages { get; set; }
//     public DbSet<Group> Groups { get; set; }
//     public DbSet<Connection> Connections { get; set; }

//     protected override void OnModelCreating(ModelBuilder builder)
//     {
//         base.OnModelCreating(builder);

//         builder.Entity<AppUser>()
//             .HasMany(ur => ur.UserRoles)
//             .WithOne(u => u.User)
//             .HasForeignKey(ur => ur.UserId)
//             .IsRequired();
//         builder.Entity<AppRole>()
//             .HasMany(ur => ur.UserRoles)
//             .WithOne(u => u.Role)
//             .HasForeignKey(ur => ur.RoleId)
//             .IsRequired();

//         builder.Entity<UserLike>()
//             .HasKey(k => new { k.SourceUserId, k.TargetUserId });
//         builder.Entity<UserLike>()
//             .HasOne(s => s.SourceUser)
//             .WithMany(l => l.LikedUsers)
//             .HasForeignKey(s => s.SourceUserId)
//             .OnDelete(DeleteBehavior.Cascade);
//         builder.Entity<UserLike>()
//             .HasOne(s => s.TargetUser)
//             .WithMany(l => l.LikedByUsers)
//             .HasForeignKey(s => s.TargetUserId)
//             .OnDelete(DeleteBehavior.NoAction);

//         builder.Entity<Message>()
//         .HasOne(x => x.Recipient)
//         .WithMany(x => x.MessagesReceived)
//         .OnDelete(DeleteBehavior.Restrict);

//         builder.Entity<Message>()
//         .HasOne(x => x.Sender)
//         .WithMany(x => x.MessagesSent)
//         .OnDelete(DeleteBehavior.Restrict);


//     }
// }
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace API.Data;

public class DataContext(DbContextOptions options) 
    : IdentityDbContext<AppUser, AppRole,
        int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
{
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

        builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        // SQLite compatibility: check Database.ProviderName
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // Map Identity string columns to TEXT
            builder.Entity<AppRole>(entity =>
            {
                entity.Property(r => r.Name).HasColumnType("TEXT");
                entity.Property(r => r.NormalizedName).HasColumnType("TEXT");
                entity.Property(r => r.ConcurrencyStamp).HasColumnType("TEXT");
            });

            builder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.UserName).HasColumnType("TEXT");
                entity.Property(u => u.NormalizedUserName).HasColumnType("TEXT");
                entity.Property(u => u.Email).HasColumnType("TEXT");
                entity.Property(u => u.NormalizedEmail).HasColumnType("TEXT");
                entity.Property(u => u.ConcurrencyStamp).HasColumnType("TEXT");
                entity.Property(u => u.SecurityStamp).HasColumnType("TEXT");
                entity.Property(u => u.PasswordHash).HasColumnType("TEXT");
                entity.Property(u => u.PhoneNumber).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.Property(l => l.LoginProvider).HasColumnType("TEXT");
                entity.Property(l => l.ProviderKey).HasColumnType("TEXT");
                entity.Property(l => l.ProviderDisplayName).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.Property(t => t.LoginProvider).HasColumnType("TEXT");
                entity.Property(t => t.Name).HasColumnType("TEXT");
                entity.Property(t => t.Value).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.Property(c => c.ClaimType).HasColumnType("TEXT");
                entity.Property(c => c.ClaimValue).HasColumnType("TEXT");
            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.Property(rc => rc.ClaimType).HasColumnType("TEXT");
                entity.Property(rc => rc.ClaimValue).HasColumnType("TEXT");
            });

            // Map all other string properties to TEXT
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var stringProps = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(string));
                foreach (var prop in stringProps)
                {
                    builder.Entity(entityType.ClrType)
                           .Property(prop.Name)
                           .HasColumnType("TEXT");
                }
            }

            // Convert decimal to double if any
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var decimalProps = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));
                foreach (var prop in decimalProps)
                {
                    builder.Entity(entityType.ClrType)
                           .Property(prop.Name)
                           .HasConversion<double>();
                }
            }

            // Convert DateTimeOffset to DateTime if used
            var dtoConverter = new ValueConverter<DateTimeOffset, DateTime>(
                v => v.UtcDateTime,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var dtoProps = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset));
                foreach (var prop in dtoProps)
                {
                    builder.Entity(entityType.ClrType)
                           .Property(prop.Name)
                           .HasConversion(dtoConverter);
                }
            }
        }
    }
}
