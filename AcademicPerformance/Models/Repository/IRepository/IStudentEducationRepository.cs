namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IStudentEducationRepository : IRepository<StudentEducation>
	{
		void Save();
	}
}
