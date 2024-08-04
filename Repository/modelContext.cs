using HR.DataModels;
using HR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR.Repository
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

        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<Interviews> Interviews { get; set; }
        public virtual DbSet<InterviewTeams> InterviewTeams { get; set; }
        public virtual DbSet<Candidates> Candidates { get; set; }
        public virtual DbSet<Predictions> Predictions { get; set; }
        public virtual DbSet<JobApplicationDetails> JobApplicationDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the view
            modelBuilder.Entity<JobApplicationDetails>().HasNoKey().ToView("JobApplicationDetails");

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name").HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DateAdded).HasColumnType("datetime");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Observation).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("id").IsClustered(false);
                entity.ToTable("Employees");
                entity.HasIndex(e => e.Name).HasName("Name");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CompanyShortName).HasMaxLength(10);
                entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(128);
                entity.Property(e => e.Name).HasMaxLength(150).IsUnicode(false);
                entity.Property(e => e.EmploymentDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Team).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(128);
            });

            modelBuilder.Entity<Functions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(e => e.Name).IsRequired().HasColumnName("Name").HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Interviews>(entity =>
            {
                entity.ToTable("Interviews");
                entity.Property(e => e.AddedAt).HasColumnType("datetime");
                entity.Property(e => e.AddedBy).HasMaxLength(100);
                entity.Property(e => e.Comments).HasMaxLength(200);
                entity.Property(e => e.DateAnswer).HasColumnType("datetime");
                entity.Property(e => e.EmploymentDate).HasColumnType("datetime");
                entity.Property(e => e.InterviewDate).HasColumnType("datetime");
                entity.Property(e => e.CandidateId).HasColumnName("CandidateId");
                entity.Property(e => e.RefusedObservation).HasMaxLength(200);
                entity.Property(e => e.TestResult).HasMaxLength(200);
                entity.Property(e => e.UpdatedAt).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.HasOne(d => d.Candidate).WithMany(p => p.Interview).HasForeignKey(d => d.CandidateId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Interview_Candidate");
            });

            modelBuilder.Entity<InterviewTeams>(entity =>
            {
                entity.ToTable("InterviewTeams");
                entity.Property(e => e.InterviewId).HasColumnName("InterviewId");
                entity.HasOne(d => d.Employee).WithMany(p => p.InterviewTeam).HasForeignKey(d => d.EmployeeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InterviewTeam_Employee");
                entity.HasOne(d => d.Interview).WithMany(p => p.InterviewTeam).HasForeignKey(d => d.InterviewId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_InterviewTeam_InterviewTeam");
            });

            modelBuilder.Entity<Candidates>(entity =>
            {
                entity.ToTable("Candidates");
                entity.Property(e => e.AddedAt).HasColumnType("datetime");
                entity.Property(e => e.AddedBy).HasMaxLength(100);
                entity.Property(e => e.BirthDate).HasColumnType("date");
                entity.Property(e => e.City).HasMaxLength(200);
                entity.Property(e => e.County).HasMaxLength(200);
                entity.Property(e => e.ReciveDate).HasColumnName("ReciveDate").HasColumnType("datetime");
                entity.Property(e => e.DateApply).HasColumnType("datetime");
                entity.Property(e => e.Experience).HasMaxLength(500);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Observation).HasMaxLength(500);
                entity.Property(e => e.Studies).HasMaxLength(500);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            });

            modelBuilder.Entity<Predictions>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Predictions");
                entity.Property(e => e.BirthDate).HasColumnType("date");
                entity.Property(e => e.Experience).HasMaxLength(500);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DepartmentName).IsRequired().HasColumnName("DepartmentName").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.FunctionName).IsRequired().HasColumnName("FunctionName").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Observation).HasMaxLength(500);
                entity.Property(e => e.Studies).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
