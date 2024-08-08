namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IStudentCertificationRepository : IRepository<StudentCertification>
	{
		void Save();
	}
}
