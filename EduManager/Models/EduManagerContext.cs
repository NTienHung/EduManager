using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduManagerAPI.Models;

public partial class EduManagerContext : DbContext
{
    public static EduManagerContext Ins = new EduManagerContext();
    public EduManagerContext()
    {
        if (Ins == null)
            Ins = this;
    }

    public EduManagerContext(DbContextOptions<EduManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn")); }


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => new { e.ClassId, e.LessonId });

            entity.ToTable("Attendance");

            entity.HasIndex(e => e.LessonId, "IX_Attendance_lessonId");

            entity.Property(e => e.ClassId)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("classId");
            entity.Property(e => e.LessonId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("lessonId");
            entity.Property(e => e.Attendance1).HasColumnName("attendance");
            entity.Property(e => e.Comment)
                .HasMaxLength(70)
                .HasColumnName("comment");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Evaluation).HasColumnName("evaluation");

            entity.HasOne(d => d.Class).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Attendances).HasForeignKey(d => d.LessonId);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasIndex(e => e.BranchId, "IX_Classes_branchId");

            entity.HasIndex(e => e.TeacherId, "IX_Classes_teacherId");

            entity.Property(e => e.Id)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.BranchId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("branchId");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.TeacherId).HasColumnName("teacherId");

            entity.HasOne(d => d.Branch).WithMany(p => p.Classes)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(d => d.Students).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Class>().WithMany().HasForeignKey("ClassId"),
                    j =>
                    {
                        j.HasKey("ClassId", "StudentId");
                        j.ToTable("ClassStudents");
                        j.HasIndex(new[] { "StudentId" }, "IX_ClassStudents_studentId");
                        j.IndexerProperty<string>("ClassId")
                            .HasMaxLength(7)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("classId");
                        j.IndexerProperty<Guid>("StudentId").HasColumnName("studentId");
                    });
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Courses_categoryId");

            entity.Property(e => e.Id)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("categoryId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Lessons_courseId");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("courseId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3213E83FB0C7D5B4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedDate).HasColumnName("addedDate");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiryDate");
            entity.Property(e => e.IsRevoked).HasColumnName("isRevoked");
            entity.Property(e => e.IsUsed).HasColumnName("isUsed");
            entity.Property(e => e.JwtId)
                .HasMaxLength(450)
                .HasColumnName("jwtId");
            entity.Property(e => e.Token)
                .IsUnicode(false)
                .HasColumnName("token");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("userId");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("fullName");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Teachers_customerId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("fullName");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");

            entity.HasOne(d => d.Customer).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
