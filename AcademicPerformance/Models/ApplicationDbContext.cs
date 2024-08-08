using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AcademicPerformance.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Branch> Branches { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<ApplicationUser> AppUsers { get; set; }
		public DbSet<EvaluationReport> EVReports { get; set; }
		public DbSet<TheoryResult> TheoryResults { get; set; }
		public DbSet<StudentCertification> StudentCertifications { get; set; }
		public DbSet<StudentEducation> StudentEducations { get; set; }
		public DbSet<TeacherSubject> TeacherSubjects { get; set; }
		public DbSet<Jobs> Jobs { get; set; }
		public DbSet<Exam> Exams { get; set; }
		public DbSet<ExamSubject> ExamSubjects { get; set; }
		public DbSet<TheoryMarks> TheoryMarks { get; set; }
		public DbSet<PracticalMarks> PracticalMarks { get; set; }
		public DbSet<JobsApply> JobsApplies { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Branch>()
				.HasMany(e => e.Subjects)
				.WithOne(e => e.Branch)
				.HasForeignKey(e => e.BranchId)
				.HasPrincipalKey(e => e.Id);

			modelBuilder.Entity<Branch>()
				.HasMany(e => e.Students)
				.WithOne(e => e.Branch)
				.HasForeignKey(e => e.BranchId)
				.HasPrincipalKey(e => e.Id);

			modelBuilder.Entity<Teacher>()
				.Property(u => u.Qualification)
				.IsRequired(false);
			modelBuilder.Entity<Teacher>()
				.Property(u => u.isHOD)
				.HasDefaultValue(0);

			modelBuilder.Entity<ApplicationUser>()
				.Property(u => u.FullName)
				.IsRequired(false);

			modelBuilder.Entity<Student>()
				.Property(u => u.Gender).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.DOB).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.RollNo).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.BloodGroup).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Nationality).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Religion).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Caste).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.CasteCategory).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Address).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Mobile).HasDefaultValue(0);
			modelBuilder.Entity<Student>()
				.Property(u => u.About).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.HandsOn).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.Interships).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.OtherActivities).IsRequired(false);
			modelBuilder.Entity<Student>()
				.Property(u => u.ImageUrl).IsRequired(false);

			modelBuilder.Entity<Student>()
				.Property(u => u.Sem).HasDefaultValue(5);

			modelBuilder.Entity<Student>()
				.HasIndex(u => u.PRN).IsUnique(true);
			modelBuilder.Entity<Student>()
				.HasIndex(u => u.Aadhaar).IsUnique(true);

			modelBuilder.Entity<EvaluationReport>()
				.Property(u => u.ReportData).IsRequired(false);
			modelBuilder.Entity<EvaluationReport>()
				.Property(u => u.SubjectId).HasDefaultValue(1);

			modelBuilder.Entity<TheoryResult>()
				.Property(u => u.ReportData).IsRequired(false);
			modelBuilder.Entity<TheoryResult>()
				.Property(u => u.SubjectId).HasDefaultValue(1);

			modelBuilder.Entity<StudentEducation>()
				.Property(u => u.Percentage).IsRequired(false);

			modelBuilder.Entity<Subject>()
				.Property(u => u.Code).HasDefaultValue("1234");
			modelBuilder.Entity<Subject>()
				.Property(u => u.BranchYear).HasDefaultValue(3);
			modelBuilder.Entity<Subject>()
				.Property(u => u.Sem).HasDefaultValue(5);
			modelBuilder.Entity<Subject>()
				.HasIndex(u => u.Code).IsUnique(true);
			modelBuilder.Entity<Subject>()
				.Property(u => u.isTheory).HasDefaultValue(1);
			modelBuilder.Entity<Subject>()
				.Property(u => u.isTPractical).HasDefaultValue(1);
			modelBuilder.Entity<Subject>()
				.Property(u => u.isAudit).HasDefaultValue(0);

			modelBuilder.Entity<StudentEducation>()
				.Property(u => u.FileUrl).IsRequired(false);
			modelBuilder.Entity<StudentCertification>()
				.Property(u => u.FileUrl).IsRequired(false);

			modelBuilder.Entity<ExamSubject>()
				.Property(u => u.Year).HasDefaultValue(3);
			modelBuilder.Entity<ExamSubject>()
				.Property(u => u.Sem).HasDefaultValue(5);


			base.OnModelCreating(modelBuilder);
		}
	}
}
