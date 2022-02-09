using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    
        public partial class modelContext : IdentityDbContext
        {
            public modelContext()
            {
            }

            public modelContext(DbContextOptions<modelContext> options)
                : base(options)
            {
            }



        public virtual DbSet<Auxi> Auxi { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<InterviewCv> InterviewCv { get; set; }
        public virtual DbSet<InterviewTeam> InterviewTeam { get; set; }
        public virtual DbSet<Multi> Multi { get; set; }
        public virtual DbSet<PersonCv> PersonCv { get; set; }
        public virtual DbSet<Prediction> Prediction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ADRIAN;Database=model;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Auxi>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Accepted)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionCv)
                    .HasColumnName("FunctionCV")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModeApply)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OffertStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefusedReason)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NameDepartment)
                    .IsRequired()
                    .HasColumnName("name_department")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observation)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_M_Matrix7")
                    .IsClustered(false);

                entity.ToTable("Employee", "matrix");

                entity.HasIndex(e => e.EmployeeName)
                    .HasName("EmployeeName");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CompanyShortName).HasMaxLength(10);

                entity.Property(e => e.Email)
                    .HasColumnName("EMail")
                    .HasMaxLength(128);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmploymentDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Team)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(128);
            });

            modelBuilder.Entity<Functions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdDepartment).HasColumnName("id_department");

                entity.Property(e => e.NameFunction)
                    .IsRequired()
                    .HasColumnName("name_function")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InterviewCv>(entity =>
            {
                entity.ToTable("InterviewCV", "CV");

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.DateAnswer).HasColumnType("datetime");

                entity.Property(e => e.EmploymentDate).HasColumnType("datetime");

                entity.Property(e => e.InterviewDate).HasColumnType("datetime");

                entity.Property(e => e.PersonCvid).HasColumnName("PersonCVId");

                entity.Property(e => e.RefusedObservation).HasMaxLength(200);

                entity.Property(e => e.TestResult).HasMaxLength(200);

                entity.Property(e => e.UpdatedAt).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.HasOne(d => d.PersonCv)
                    .WithMany(p => p.InterviewCv)
                    .HasForeignKey(d => d.PersonCvid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterviewCV_PersonCV");
            });

            modelBuilder.Entity<InterviewTeam>(entity =>
            {
                entity.ToTable("InterviewTeam", "CV");

                entity.Property(e => e.InterviewCvid).HasColumnName("InterviewCVId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.InterviewTeam)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterviewTeam_Employee");

                entity.HasOne(d => d.InterviewCv)
                    .WithMany(p => p.InterviewTeam)
                    .HasForeignKey(d => d.InterviewCvid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterviewTeam_InterviewTeam");
            });

            modelBuilder.Entity<Multi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Multi");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.DateAnswer).HasColumnType("datetime");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmploymentDate).HasColumnType("datetime");

                entity.Property(e => e.InterviewDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NameDepartment)
                    .IsRequired()
                    .HasColumnName("name_department")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameFunction)
                    .IsRequired()
                    .HasColumnName("name_function")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observation).HasMaxLength(500);

                entity.Property(e => e.TestResult).HasMaxLength(200);
            });

            modelBuilder.Entity<PersonCv>(entity =>
            {
                entity.ToTable("PersonCV", "CV");

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CityAddress).HasMaxLength(200);

                entity.Property(e => e.CountyAddress).HasMaxLength(200);

                entity.Property(e => e.CvreciveDate)
                    .HasColumnName("CVReciveDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateApply).HasColumnType("datetime");

                entity.Property(e => e.Experience).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Observation).HasMaxLength(500);

                entity.Property(e => e.Studies).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });

            modelBuilder.Entity<Prediction>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Prediction");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Experience).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NameDepartment)
                    .IsRequired()
                    .HasColumnName("name_department")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameFunction)
                    .IsRequired()
                    .HasColumnName("name_function")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observation).HasMaxLength(500);

                entity.Property(e => e.Studies).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
