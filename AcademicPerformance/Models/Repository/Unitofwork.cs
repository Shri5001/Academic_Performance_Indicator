using AcademicPerformance.Models.Repository.IRepository;

namespace AcademicPerformance.Models.Repository
{
	public class Unitofwork : IUnitofwork
	{
		public IBranchRepository Branch { get; set ; }
		public ISubjectRepository Subject { get; set; }
		public ITeacherRepository Teacher { get; set; }
		public IStudentRepository Student { get; set; }
		public IEVRepository EVReport { get; set; }
		public IStudentCertificationRepository StudentCertification { get; set; }
		public IStudentEducationRepository StudentEducation { get; set; }

		private readonly ApplicationDbContext _db;
		public Unitofwork(ApplicationDbContext db)
		{
			_db = db;
			Branch = new BranchRepository(_db);
			Subject = new SubjectRepository(_db);
			Teacher = new TeacherRepository(_db);
			Student = new StudentRepository(_db);
			EVReport = new EVRepository(_db);
			StudentCertification = new StudentCertificationRepository(_db);
			StudentEducation = new StudentEducationRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
