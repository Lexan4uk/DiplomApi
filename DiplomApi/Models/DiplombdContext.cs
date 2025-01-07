using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiplomApi.Models;

public partial class DiplombdContext : DbContext
{
    public DiplombdContext()
    {
    }

    public DiplombdContext(DbContextOptions<DiplombdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<BoquetCompleted> BoquetCompleteds { get; set; }

    public virtual DbSet<BoquetConstructor> BoquetConstructors { get; set; }

    public class EmailInput
    {
        public string Email { get; set; }
    }
    public class AccountInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class EditName
    {
        public string Name { get; set; }
    }
    public class EditPhone
    {
        public string PhoneNumber { get; set; }
    }
    public class EditPassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("PK__Accounts__CA90DA7BC10B60BF");

            entity.ToTable(tb => tb.HasTrigger("trg_InsertUserProfile"));

            entity.Property(e => e.Token)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("token");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Adresses__3213E83FD4046055");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<BoquetCompleted>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BoquetCo__3213E83FC31940C3");

            entity.ToTable("BoquetCompleted");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Cover)
                .HasMaxLength(255)
                .HasColumnName("cover");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Link)
                .HasMaxLength(100)
                .HasColumnName("link");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OldPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("old_price");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Promo)
                .HasDefaultValue(false)
                .HasColumnName("promo");
            entity.Property(e => e.Composition)
                .HasDefaultValue(false)
                .HasColumnName("composition");
        });

        modelBuilder.Entity<BoquetConstructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BoquetCo__3213E83F409FF6CA");

            entity.ToTable("BoquetConstructor");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cover)
                .HasMaxLength(255)
                .HasColumnName("cover");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("PK__UserProf__CA90DA7B29AF6D75");

            entity.Property(e => e.Token)
                .ValueGeneratedNever()
                .HasColumnName("token");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.TokenNavigation).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.Token)
                .HasConstraintName("FK__UserProfi__token__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
