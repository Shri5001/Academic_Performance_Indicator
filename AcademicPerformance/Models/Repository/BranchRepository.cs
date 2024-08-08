using AcademicPerformance.Models.Repository.IRepository;

namespace AcademicPerformance.Models.Repository
{
	public class BranchRepository : Repository<Branch>, IBranchRepository
	{
		private readonly ApplicationDbContext _db;
		public BranchRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
