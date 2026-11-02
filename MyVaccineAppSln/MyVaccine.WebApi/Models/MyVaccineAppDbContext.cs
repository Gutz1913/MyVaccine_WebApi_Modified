using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyVaccine.WebApi.Models;

public class MyVaccineAppDbContext : IdentityDbContext<IdentityUser>
{
    public MyVaccineAppDbContext(DbContextOptions<MyVaccineAppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<UserAllergy> UserAllergies => Set<UserAllergy>();
    public DbSet<DependentAllergy> DependentAllergies => Set<DependentAllergy>();
    public DbSet<FamilyGroup> FamilyGroups => Set<FamilyGroup>();
    public DbSet<Vaccine> Vaccines => Set<Vaccine>();
    public DbSet<VaccineCategory> VaccineCategories => Set<VaccineCategory>();
    public DbSet<VaccineCategory_Vaccine> VaccineCategory_Vaccines => Set<VaccineCategory_Vaccine>();
    public DbSet<VaccineRecord> VaccineRecords => Set<VaccineRecord>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // === User Configurations === //
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.FirstName)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.Property(u => u.LastName)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.HasOne(u => u.AspNetUser)
                  .WithMany()
                  .HasForeignKey(u => u.AspNetUserId);
        });

        // === Dependent Configurations === //
        builder.Entity<Dependent>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasOne(d => d.User)
                  .WithMany(u => u.Dependents)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.FamilyGroup)
                  .WithMany(fg => fg.Dependents)
                  .HasForeignKey(d => d.FamilyGroupId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // === Allergy Configurations === //
        builder.Entity<Allergy>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(a => a.Name)
                  .IsUnique();
        });

        // === UserAllergy Configurations === //
        builder.Entity<UserAllergy>(entity =>
        {
            entity.HasKey(ua => ua.Id);

            // Relaltionship with User
            entity.HasOne(ua => ua.User)
                  .WithMany(u => u.UserAllergies)
                  .HasForeignKey(ua => ua.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Allergy
            entity.HasOne(ua => ua.Allergy)
                  .WithMany(a => a.UserAllergies)
                  .HasForeignKey(ua => ua.AllergyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === DependentAllergy Configurations === //
        builder.Entity<DependentAllergy>(entity =>
        {
            entity.HasKey(da => da.Id);

            // Relaltionship with Dependent
            entity.HasOne(da => da.Dependent)
                  .WithMany(d => d.DependentAllergies)
                  .HasForeignKey(da => da.DependentId)
                  .OnDelete(DeleteBehavior.Cascade);

            //Relationship with Allergy
            entity.HasOne(da => da.Allergy)
                  .WithMany(a => a.DependentAllergies)
                  .HasForeignKey(da => da.AllergyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === FamilyGroup Configurations === //
        builder.Entity<FamilyGroup>(entity =>
        {
            entity.HasKey(fg => fg.Id);

            entity.Property(fg => fg.Name)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.HasIndex(fg => fg.Name)
                  .IsUnique();
        });

        // === Vaccine Configurations === //
        builder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(v => v.RequiresBooster)
                  .IsRequired();

            entity.HasIndex(v => v.Name)
                  .IsUnique();
        });

        // === VaccineCategory Configurations === //
        builder.Entity<VaccineCategory>(entity =>
        {
            entity.HasKey(vc => vc.Id);

            entity.Property(vc => vc.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(vc => vc.Description)
                  .IsRequired()
                  .HasMaxLength(int.MaxValue);

            entity.HasIndex(vc => vc.Name)
                  .IsUnique();
        });

        // === VaccineCategory_Vaccine Configurations === //
        builder.Entity<VaccineCategory_Vaccine>(entity =>
        {
            entity.HasKey(vcv => vcv.Id);

            // Relationship with Vaccine
            entity.HasOne(vcv => vcv.Vaccine)
                  .WithMany(v => v.VaccineCategory_Vaccines)
                  .HasForeignKey(vcv => vcv.VaccineId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with VaccineCategory
            entity.HasOne(vcv => vcv.VaccineCategory)
                  .WithMany(vc => vc.VaccineCategory_Vaccines)
                  .HasForeignKey(vcv => vcv.VaccineCategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === VaccineRecord Configurations === //
        builder.Entity<VaccineRecord>(entity =>
        {
            entity.HasKey(vr => vr.Id);

            entity.Property(vr => vr.AdministeredLocation)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(vr => vr.AdministeredBy)
                  .IsRequired()
                  .HasMaxLength(50);

            // Relationship with User
            entity.HasOne(vr => vr.User)
                  .WithMany(u => u.VaccineRecords)
                  .HasForeignKey(vr => vr.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Dependent
            entity.HasOne(vr => vr.Dependent)
                  .WithMany(d => d.VaccineRecords)
                  .HasForeignKey(vr => vr.DependentId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Vaccine
            entity.HasOne(vr => vr.Vaccine)
                  .WithMany(v => v.VaccineRecords)
                  .HasForeignKey(vr => vr.VaccineId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
