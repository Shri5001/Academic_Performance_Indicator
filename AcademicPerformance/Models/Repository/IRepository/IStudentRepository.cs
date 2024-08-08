using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IStudentRepository : IRepository<Student>
	{
		IEnumerable<Student> IncludeWhere(Expression<Func<Student, object>> includeProperty, Expression<Func<Student, bool>> filter);
		void Save();
	}
}
