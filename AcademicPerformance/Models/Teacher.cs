using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace AcademicPerformance.Models
{
	public class Teacher
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		[Required]
		public int BranchId { get; set; }

		[ForeignKey("BranchId")]
		public Branch Branch { get; set; }

		public string Qualification { get; set; }

		[DisplayName("Is this teacher HOD?")]
		public int isHOD { get; set; }
	}
}
