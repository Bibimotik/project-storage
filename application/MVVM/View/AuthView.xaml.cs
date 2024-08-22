using System.Net;
using System.Net.Mail;
using System.Windows;

using application.MVVM.View.Auth;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.Auth;
using application.Services;

using DotNetEnv;


namespace application.MVVM.View;

public partial class AuthView : Window
{
	public AuthView(AuthViewModel authViewModel)
	{
		DataContext = authViewModel;
		InitializeComponent();
	}
}
