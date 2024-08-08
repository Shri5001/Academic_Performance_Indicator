using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface ITheoryResultRepository : IRepository<TheoryResult>
	{
		IEnumerable<TheoryResult> IncludeWhere(Expression<Func<TheoryResult, object>> includeProperty, Expression<Func<TheoryResult, bool>> filter);
		void Save();
	}
}
