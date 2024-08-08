using AcademicPerformance.Models.Repository.IRepository;

namespace AcademicPerformance.Models.Repository
{
	public class TeacherRepository : Repository<Teacher>, ITeacherRepository
	{
		private readonly ApplicationDbContext _db;
		public TeacherRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
