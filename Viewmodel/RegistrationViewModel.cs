using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiDemo1.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace MauiApp1.Viewmodel
{
	public class RegistrationViewModel : ObservableObject
	{
		// Observable collection to bind to UI
		public List<Course> Courses { get; set; }

		public RegistrationViewModel()
		{
			// Initialize the courses list
			Courses = new List<Course>();

			// Load the courses asynchronously
			LoadCoursesAsync();
		}

		// Async method to load courses
		private async Task LoadCoursesAsync()
		{
			var students = await LoadStudentsAsync();

			// Populate the Courses property with the data from JSON
			Courses.AddRange(students);
		}

		// Method to load students from the JSON file
		private async Task<List<Course>> LoadStudentsAsync()
		{
			try
			{
				// เปิดไฟล์จากแพ็คเกจของแอป
				using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
				using var reader = new StreamReader(stream);
				var json = await reader.ReadToEndAsync();

				Debug.WriteLine(json); // เช็คข้อมูล JSON ที่โหลดมา

				// แปลง JSON เป็นอ็อบเจกต์ Croustreq
				var data = JsonConvert.DeserializeObject<Croustreq>(json);

				// บันทึกข้อมูลรายละเอียดของคอร์ส
				if (data?.Courses != null && data.Courses.Any())
				{
					foreach (var course in data.Courses)
					{
						Debug.WriteLine($"CourseId: {course.CourseId}");
						Debug.WriteLine($"CourseName: {course.CourseName}");
						Debug.WriteLine($"Description: {course.CourseDescription}");
						Debug.WriteLine($"CreditHours: {course.CreditHours}");
						Debug.WriteLine($"Department: {course.Department}");
						Debug.WriteLine($"Semester: {course.Semester}");
						Debug.WriteLine($"AvailableSeats: {course.AvailableSeats}");
						Debug.WriteLine("------------");
					}
				}
				else
				{
					Debug.WriteLine("No courses found.");
				}

				// ส่งคืนรายการคอร์สจากอ็อบเจกต์ Croustreq
				return data?.Courses ?? new List<Course>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error reading JSON: {ex.Message}");
				return new List<Course>(); // ส่งคืนรายการคอร์สว่างหากเกิดข้อผิดพลาด
			}
		}

	}

}
