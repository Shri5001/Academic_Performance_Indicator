using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AcademicPerformance.Models.Repository
{
	public class SubjectRepository : Repository<Subject>, ISubjectRepository
	{
		private readonly ApplicationDbContext _db;
		public SubjectRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Save()
		{
			_db.SaveChanges();
		}

		public IEnumerable<Subject> IncludeBranch()
		{
			IQueryable<Subject> query = dbSet;
			query = query.Include(u => u.Branch);
			return query.ToList();
		}
	}
}
