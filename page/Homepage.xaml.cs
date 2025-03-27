namespace MauiApp1.page;

using System.Diagnostics;
using MauiApp1.Viewmodel;
using MauiDemo.Model;
using Newtonsoft.Json;


[QueryProperty(nameof(UserJson), "User")]
public partial class Homepage : ContentPage
{
	private HomeViewModel _viewModel;
	// รับค่าเป็น JSON string
	public string UserJson { get; set; }
	public Homepage()
	{
		InitializeComponent();
	}
	private async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Registrationpage));
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();

		// เมื่อ OnAppearing, deserialize ค่า JSON string ที่ได้รับ
		if (!string.IsNullOrEmpty(UserJson))
		{
			var user = JsonConvert.DeserializeObject<Studentreq>(UserJson);

			if (user != null)
			{
				_viewModel = new HomeViewModel(user);
				BindingContext = _viewModel;
			}
		}
	}

	private async void OnrButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Regmepage));
	}
	// Event handler for logout button click
	private async void OnLogoutButtonClicked(object sender, EventArgs e)
	{
		bool isConfirmed = await DisplayAlert(
			"ยืนยันการออกจากระบบ", // Title of the alert
			"คุณต้องการออกจากระบบหรือไม่?", // Message in the alert
			"ออกจากระบบ", // Confirm button text
			"ยกเลิก"  // Cancel button text
		);

		if (isConfirmed)
		{
			// Perform logout actions, e.g., clearing session or navigating to login page
			Debug.WriteLine("Logged out successfully!");

			// Navigate to login page (example)
			await Shell.Current.GoToAsync(nameof(Loginpage));
		}
		else
		{
			Debug.WriteLine("Logout canceled.");
		}
	}


}