namespace AcademicPerformance.Models.VM
{
	public class StudentHomeVM
	{
		public Student Student { get; set; }
		public List<Jobs> Jobs { get; set; }
		public List<StudentEducation> Education { get; set; }
		public List<StudentCertification> Certification { get; set; }
	}
}
