using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository
{
	public class TheoryResultRepository : Repository<TheoryResult>, ITheoryResultRepository
	{
		private readonly ApplicationDbContext _db;
		public TheoryResultRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public IEnumerable<TheoryResult> IncludeWhere(Expression<Func<TheoryResult, object>> includeProperty, Expression<Func<TheoryResult, bool>> filter)
		{
			IQueryable<TheoryResult> query = _db.TheoryResults.Where(filter).Include(u => u.Branch);
			return query.ToList();
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
