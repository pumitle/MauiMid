using System.Diagnostics;
using MauiApp1.Viewmodel;

namespace MauiApp1.page;

public partial class Regmepage : ContentPage
{
	public Regmepage()
	{
		InitializeComponent();
		BindingContext = new RegmeViewModel();
	}

	// Event handler for the button click
	private async void OnWithdrawCourseClicked(object sender, EventArgs e)
	{
		// Show confirmation alert
		bool isConfirmed = await DisplayAlert(
			"ยืนยันการถอนวิชา", // Title of the alert
			"คุณต้องการถอนวิชานี้จริงๆ หรือไม่?", // Message in the alert
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

	// Optional: Example method to handle withdrawal logic (if needed)
	private async Task WithdrawalFunction()
	{
		// Your withdrawal logic goes here, e.g., calling the ViewModel method or API
		Debug.WriteLine("Executing withdrawal logic...");
	}
}