using AcademicPerformance.Models.Repository.IRepository;

namespace AcademicPerformance.Models.Repository
{
	public class StudentEducationRepository : Repository<StudentEducation>, IStudentEducationRepository
	{
		private readonly ApplicationDbContext _db;
		public StudentEducationRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
