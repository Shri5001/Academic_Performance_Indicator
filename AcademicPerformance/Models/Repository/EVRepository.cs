using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository
{
	public class EVRepository : Repository<EvaluationReport>, IEVRepository
	{
		private readonly ApplicationDbContext _db;
		public EVRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public IEnumerable<EvaluationReport> IncludeWhere(Expression<Func<EvaluationReport, object>> includeProperty, Expression<Func<EvaluationReport, bool>> filter)
		{
			IQueryable<EvaluationReport> query = _db.EVReports.Where(filter).Include(u => u.Branch);
			return query.ToList();
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
