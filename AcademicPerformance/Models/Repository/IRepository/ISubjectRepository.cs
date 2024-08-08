namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface ISubjectRepository : IRepository<Subject>
	{
		void Save();

		IEnumerable<Subject> IncludeBranch();
	}
}
