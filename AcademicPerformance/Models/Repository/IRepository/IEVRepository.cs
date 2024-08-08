using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IEVRepository : IRepository<EvaluationReport>
	{
		IEnumerable<EvaluationReport> IncludeWhere(Expression<Func<EvaluationReport, object>> includeProperty, Expression<Func<EvaluationReport, bool>> filter);
		void Save();
	}
}
