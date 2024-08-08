using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AcademicPerformance.Models
{
	public class Student
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

		public string Gender { get; set; }

		[DisplayName("Date of Birth")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? DOB { get; set; }

		public string PRN { get; set; }

		[DisplayName("Roll No")]
		public int? RollNo { get; set; }

		[Required]
		[DisplayName("Class")]
		public string ClassSection { get; set; }

		[Required]
		[Range(1, 4, ErrorMessage = "Year can be in between 1 - 4.")]
		[DisplayName("Year")]
		public int ClassYear { get; set; }

		[Required]
		[Range(1, 8, ErrorMessage = "Semester can be in between 1 - 8.")]
		[DisplayName("Semester")]
		public int Sem { get; set; }

		[Required]
		[Range(100000000000, 999999999999, ErrorMessage = "Aadhaar must be exactly 12 digits.")]
		public long Aadhaar { get; set; }

		[DisplayName("Blood Group")]
		public string BloodGroup { get; set; }

		public string Nationality { get; set; }

		public string Religion { get; set; }

		public string Caste { get; set; }

		[DisplayName("Caste Category")]
		public string CasteCategory { get; set; }

		public string Address { get; set; }

		[DisplayName("Mobile Number")]
		[Range(1000000000, 9999999999, ErrorMessage = "Mobile must be exactly 10 digits.")]
		public long Mobile { get; set; }

		[DisplayName("About Yourself")]
		public string About { get; set; }

		[DisplayName("Hands on Experience")]
		public string HandsOn { get; set; }

		[DisplayName("Internships")]
		public string Interships { get; set; }

		[DisplayName("Other Activities")]
		public string OtherActivities { get; set; }

		[DisplayName("Image")]
		public string ImageUrl { get; set; }

		[NotMapped] // Exclude this property from database mapping
		[DisplayName("Upload Image")]
		public IFormFile ImageFile { get; set; }
	}
}
