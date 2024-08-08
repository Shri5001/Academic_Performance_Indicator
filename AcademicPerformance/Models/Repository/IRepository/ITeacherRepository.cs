namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface ITeacherRepository : IRepository<Teacher>
	{
		void Save();
	}
}
