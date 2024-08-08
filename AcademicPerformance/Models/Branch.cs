using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace AcademicPerformance.Models
{
	public class Branch
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		public ICollection<Subject> Subjects { get; }
		public ICollection<Teacher> Teacher { get; }

		public ICollection<Student> Students { get; }
	}
}
