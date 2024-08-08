using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll();
		T Get(Expression<Func<T, bool>> filter);
		IEnumerable<T> GetWhere(Expression<Func<T, bool>> filter);
		IEnumerable<T> Include(Expression<Func<T, object>> includeProperty);
		void Add(T entity);
		void Remove(T entity);
		void Update(T entity);
		void RemoveRange(IEnumerable<T> entity);
	}
}
