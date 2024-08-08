using AcademicPerformance.Models.Repository.IRepository;
using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository
{
	public class StudentCertificationRepository : Repository<StudentCertification>, IStudentCertificationRepository
	{
		private readonly ApplicationDbContext _db;
		public StudentCertificationRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
