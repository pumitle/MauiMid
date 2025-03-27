using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using MauiApp1.page;
using MauiDemo.Model; //เรียกใช้Model ในไฟล์แยก
// using NewMUAI.Pages;

namespace MauiApp1.Viewmodel;
public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string username = "";

    [ObservableProperty]
    string password = "";

    [RelayCommand]
    public async void Login()
    {
        var students = await LoadStudentsAsync();


        var user = students.FirstOrDefault(s => s.Email == Username && s.Password == Password);
        if (user != null)
        {
            // ใช้ Join เพื่อรวมรายวิชา
            var currentCourses = user.CurrentTermCourses.Any()
                ? string.Join(", ", user.CurrentTermCourses)
                : "None";

            var previousCourses = user.PreviousTermCourses.Any()
                ? string.Join("\n", user.PreviousTermCourses
                    .Select((term, index) => $"- Previous Term {index + 1}: {string.Join(", ", term)}"))
                : "No previous term courses";

            // แสดงข้อมูลใน Debug Console
            Debug.WriteLine("✅ Login Success:");
            Debug.WriteLine($"- ID: {user.Id}");
            Debug.WriteLine($"- Name: {user.Name}");
            Debug.WriteLine($"- Profile Image: {user.ProfileImage}");
            Debug.WriteLine($"- Email: {user.Email}");
            Debug.WriteLine($"- Password: {user.Password}");
            Debug.WriteLine($"- Student ID: {user.Profile.StudentId}");
            Debug.WriteLine($"- Faculty: {user.Profile.Faculty}");
            Debug.WriteLine($"- Department: {user.Profile.Department}");
            Debug.WriteLine($"- Current Term Courses: {currentCourses}");
            Debug.WriteLine(previousCourses);

            Debug.WriteLine($"✅ Login Success: {user.Name}");
            await Shell.Current.DisplayAlert("Success", $"Welcome {user.Name}!", "OK");
            await Shell.Current.GoToAsync($"{nameof(Homepage)}?User={JsonConvert.SerializeObject(user)}");


        }
        else
        {
            Debug.WriteLine("❌ Login Failed");
            await Shell.Current.DisplayAlert("Error", "Invalid Email or Password", "OK");
        }

    }
    private async Task<List<Studentreq>> LoadStudentsAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            Debug.WriteLine(json); // ตรวจสอบ JSON ที่โหลดมา

            // แปลง JSON เป็น StudentData
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


    [RelayCommand]
    async Task GotoPage(String page)
    {
        await Shell.Current.GoToAsync(page);

    }
}