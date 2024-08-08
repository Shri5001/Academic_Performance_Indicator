using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class Subject
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[DisplayName("Subject Code")]
		public string Code { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int BranchId { get; set; }

		[ForeignKey("BranchId")]
		public Branch Branch { get; set; }

		[Required]
		[DisplayName("Branch/Department Year")]
		public int BranchYear { get; set; }

		[Required]
		[DisplayName("Semester")]
		public int Sem { get; set; }

		[Required]
		[DisplayName("Is subject has theory?")]
		public int isTheory { get; set; }

		[Required]
		[DisplayName("Is subject has practical?")]
		public int isTPractical { get; set; }

		[Required]
		[DisplayName("Is subject has audit?")]
		public int isAudit { get; set; }
	}
}
