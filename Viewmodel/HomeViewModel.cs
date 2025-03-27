namespace MauiApp1.Viewmodel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.page;
using MauiDemo.Model;
public class HomeViewModel : ObservableObject
{
	private Studentreq _user;

	public HomeViewModel(Studentreq user)
	{
		_user = user;
	}

	public Studentreq User
	{
		get => _user;
		set => SetProperty(ref _user, value);
	}
}

