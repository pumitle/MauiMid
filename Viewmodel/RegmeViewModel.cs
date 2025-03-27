using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiDemo1.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using MauiDemo.Model;
namespace MauiApp1.Viewmodel;
using System.Collections.ObjectModel;

public class RegmeViewModel : ObservableObject
{
	public ObservableCollection<Studentreq> Students { get; set; } = new();
	public string StudentName { get; set; }
	public string StudentEmail { get; set; }
	public string CurrentTermCourses { get; set; }

	public ObservableCollection<Course> RegisteredCourses { get; set; } = new();

	public ObservableCollection<TermCourse> PreviousTermCourses { get; set; } = new();
	private int loggedInUserId = 1;
	private string _selectedTerm;
	public string SelectedTerm
	{
		get => _selectedTerm;
		set
		{
			if (_selectedTerm != value)
			{
				_selectedTerm = value;
				OnPropertyChanged();
				FilterPreviousTermCourses();  // Filter when the term is changed
			}
		}
	}
	public ObservableCollection<TermCourse> FilteredPreviousTermCourses { get; set; } = new();
	public RegmeViewModel()
	{
		LoadStudentDataAsync();
	}

	private async void LoadStudentDataAsync()
	{
		try
		{
			var students = await LoadStudentsAsync();
			var loggedInStudent = students.FirstOrDefault(student => student.Id == loggedInUserId);

			if (loggedInStudent != null)
			{
				// อัพเดทข้อมูลส่วนตัว
				StudentName = loggedInStudent.Name;
				StudentEmail = loggedInStudent.Email;
				CurrentTermCourses = string.Join(", ", loggedInStudent.CurrentTermCourses);

				// อัพเดทคอร์สเรียนเทอมที่แล้ว
				PreviousTermCourses.Clear();
				// เพิ่มข้อมูลเทอม
				for (int i = 0; i < loggedInStudent.PreviousTermCourses.Count; i++)
				{
					var termCourses = loggedInStudent.PreviousTermCourses[i]; // แต่ละเทอมเป็น List<string>

					// เพิ่มข้อมูลแบบถูกต้อง
					PreviousTermCourses.Add(new TermCourse
					{
						TermLabel = $"เทอมที่ {i + 1}",  // แสดงเทอม
						Courses = new ObservableCollection<string>(termCourses)  // แปลงเป็น ObservableCollection<string>
					});
				}


				// อัพเดทคอร์สที่ลงทะเบียน
				RegisteredCourses.Clear();
				foreach (var registeredCourse in loggedInStudent.RegisteredCourses)
				{
					RegisteredCourses.Add(new Course
					{
						CourseId = registeredCourse.CourseId,
						CourseName = registeredCourse.CourseName,
						Semester = registeredCourse.Semester
					});
				}

				// เพิ่มข้อมูลนักเรียน
				Students.Clear();
				Students.Add(loggedInStudent);
				Debug.WriteLine("Data loaded successfully.");
				SelectedTerm = "เทอมที่ 1";
			}
			else
			{
				Debug.WriteLine("No student found with the given ID.");
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error loading student data: {ex.Message}");
		}
	}

	private async Task<List<Studentreq>> LoadStudentsAsync()
	{
		try
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
			using var reader = new StreamReader(stream);
			var json = await reader.ReadToEndAsync();

			// ตรวจสอบ JSON
			Debug.WriteLine("Loaded JSON: " + json);

			var data = JsonConvert.DeserializeObject<StudentData>(json);
			return data?.Students ?? new List<Studentreq>();
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error reading JSON: {ex.Message}");
			return new List<Studentreq>();
		}
	}
	public class StudentData
	{
		[JsonProperty("students")]
		public List<Studentreq> Students { get; set; }
	}

	public class TermCourse
	{
		public string TermLabel { get; set; }                      // เทอมที่
		public ObservableCollection<string> Courses { get; set; }  // รายวิชา
	}
	private void FilterPreviousTermCourses()
	{
		if (string.IsNullOrEmpty(SelectedTerm))
			return;

		FilteredPreviousTermCourses.Clear();

		var selectedTermCourses = PreviousTermCourses
			.Where(course => course.TermLabel == SelectedTerm)
			.ToList();

		foreach (var course in selectedTermCourses)
		{
			FilteredPreviousTermCourses.Add(course);
		}
	}
}
