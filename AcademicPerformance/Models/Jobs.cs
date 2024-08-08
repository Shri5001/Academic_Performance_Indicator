using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicPerformance.Models
{
	public class Jobs
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public int BranchId { get; set; }

		[ForeignKey("BranchId")]
		public Branch Branch { get; set; }

		[DisplayName("Job Details")]
		public string Details { get; set; }

	}
}
