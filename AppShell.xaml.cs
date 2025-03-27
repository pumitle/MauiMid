using MauiApp1.page;

namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Homepage), typeof(Homepage));
		Routing.RegisterRoute(nameof(Registrationpage), typeof(Registrationpage));
		Routing.RegisterRoute(nameof(Regmepage), typeof(Regmepage));
		Routing.RegisterRoute(nameof(Loginpage), typeof(Loginpage));

	}
}
