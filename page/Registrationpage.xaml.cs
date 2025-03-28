using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiApp1.Viewmodel;
using MauiDemo1.Model;
using Newtonsoft.Json;
namespace MauiApp1.page;

public partial class Registrationpage : ContentPage
{
	public ObservableCollection<Course> FilteredCourses { get; set; }
	public List<Course> AllCourses { get; set; }
	public Registrationpage()
	{
		InitializeComponent();
		BindingContext = new RegistrationViewModel();
		FilteredCourses = new ObservableCollection<Course>();
		AllCourses = new List<Course>(); // รายการคอร์สทั้งหมด

		// โหลดข้อมูลคอร์สจาก JSON
		LoadCoursesFromJsonAsync();
		LoadMockCourses();
	}

	// ฟังก์ชันเพื่อโหลดข้อมูลคอร์สจากไฟล์ JSON  // ฟังก์ชันเพื่อโหลดข้อมูลคอร์สจากไฟล์ JSON
	public async Task<List<Course>> LoadCoursesFromJsonAsync()
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
		using var reader = new StreamReader(stream);
		var json = await reader.ReadToEndAsync();

		var data = JsonConvert.DeserializeObject<Croustreq>(json);

		if (data?.Courses != null && data.Courses.Any())
		{
			AllCourses = new List<Course>(data.Courses); // Store all courses
			FilteredCourses.Clear();  // Clear existing filtered courses
			foreach (var course in AllCourses)
			{
				FilteredCourses.Add(course); // Add courses to FilteredCourses
			}
		}

		return AllCourses;
	}
	// ฟังก์ชันเพื่อจำลองข้อมูลคอร์ส
	private void LoadMockCourses()
	{
		// สร้างรายการคอร์สจำลอง
		AllCourses = new List<Course>
		{
			new Course { CourseId = "CS101", CourseName = "Computer Science 101", CourseDescription = "Introduction to computer science.", CreditHours = 3 },
			new Course { CourseId = "CS102", CourseName = "Data Structures", CourseDescription = "Learn about various data structures.", CreditHours = 4 },
			new Course { CourseId = "CS103", CourseName = "Algorithms", CourseDescription = "Learn algorithm design and analysis.", CreditHours = 4 },
			new Course { CourseId = "CS104", CourseName = "Database Systems", CourseDescription = "Learn about relational databases.", CreditHours = 3 },
			new Course { CourseId = "CS105", CourseName = "Operating Systems", CourseDescription = "Introduction to operating systems.", CreditHours = 3 }
		};

		// Add all courses to the FilteredCourses ObservableCollection
		FilteredCourses.Clear();
		foreach (var course in AllCourses)
		{
			FilteredCourses.Add(course);
		}
	}


	// Event handler เมื่อคลิกปุ่ม "Add Course"
	private async void OnAddCourseClicked(object sender, EventArgs e)
	{

		bool isConfirmed = await DisplayAlert(
			"ยืนยันการถเพิ่มวิชา", // Title of the alert
			"คุณต้องการเพิ่มวิชานี้จริงๆ หรือไม่?", // Message in the alert
			"ยืนยัน", // Confirm button text
			"ยกเลิก"  // Cancel button text
		);

		if (isConfirmed)
		{
			// If confirmed, proceed with the withdrawal logic
			Debug.WriteLine("Withdrawal confirmed!");

			// You can trigger the withdrawal functionality here (e.g., calling ViewModel's method)
			// Example: await WithdrawalFunction();

		}
		else
		{
			// Handle the cancel action
			Debug.WriteLine("Withdrawal canceled.");
		}
	}

	//ค้นหา
	// Event handler สำหรับการค้นหา
	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		var searchText = e?.NewTextValue?.ToLower() ?? string.Empty;

		// Filter courses based on course name or course ID
		var filteredList = AllCourses
			.Where(course =>
				course.CourseName.ToLower().Contains(searchText) || course.CourseId.ToLower().Contains(searchText))
			.ToList();

		// Clear current FilteredCourses and add the filtered results
		FilteredCourses.Clear();
		foreach (var course in filteredList)
		{
			FilteredCourses.Add(course);
		}
	}


}