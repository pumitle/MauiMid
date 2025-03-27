using MauiApp1.Viewmodel;
using CommunityToolkit.Mvvm.ComponentModel;
namespace MauiApp1.page;

public partial class Loginpage : ContentPage
{
	public Loginpage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
	}
}

// public class Logindata
// {
// 	public String Username { get; set; } = "";
// 	public String Password { get; set; } = "";
// }