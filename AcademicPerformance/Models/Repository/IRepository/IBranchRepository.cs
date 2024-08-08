namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IBranchRepository : IRepository<Branch>
	{
		void Save();
	}
}
