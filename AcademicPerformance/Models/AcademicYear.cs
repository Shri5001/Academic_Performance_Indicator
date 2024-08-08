namespace AcademicPerformance.Models
{
	public class AcademicYear
	{
		public int Year { get; set; }

		// Method to get a list of academic years
		public static List<AcademicYear> GetAcademicYears(int startYear, int numberOfYears)
		{
			List<AcademicYear> academicYears = new List<AcademicYear>();

			// Generate academic years starting from the startYear
			for (int i = 0; i < numberOfYears; i++)
			{
				AcademicYear year = new AcademicYear
				{
					Year = startYear - i
				};
				academicYears.Add(year);
			}

			return academicYears;
		}
	}

	//<select asp-for="SelectedYear" class="form-control" asp-items="@(new SelectList(YourNamespace.Models.AcademicYear.GetAcademicYears(DateTime.Now.Year, 10), "Year", "Year"))">
	//</select>
}
