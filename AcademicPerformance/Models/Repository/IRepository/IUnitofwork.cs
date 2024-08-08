namespace AcademicPerformance.Models.Repository.IRepository
{
	public interface IUnitofwork
	{
		public IBranchRepository Branch { get; set; }
		public ISubjectRepository Subject { get; set; }
		public ITeacherRepository Teacher { get; set; }
		public IStudentRepository Student { get; set; }
		public IEVRepository EVReport { get; set; }
		public IStudentCertificationRepository StudentCertification { get; set; }
		public IStudentEducationRepository StudentEducation { get; set; }
		void Save();
	}
}
