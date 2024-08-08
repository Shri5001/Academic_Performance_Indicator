using AcademicPerformance.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AcademicPerformance.Models.Repository
{
	public class StudentRepository : Repository<Student>, IStudentRepository
	{
		private readonly ApplicationDbContext _db;
		public StudentRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public IEnumerable<Student> IncludeWhere(Expression<Func<Student, object>> includeProperty, Expression<Func<Student, bool>> filter)
		{
			IQueryable<Student> query = _db.Students.Where(filter).Include(u => u.User);
			return query.ToList();
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
