using application.Abstraction;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class AccountViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;

	public AccountViewModel(IAuthService authService, INavigationService navigationService)
	{
		_authService = authService;
		_navigationService = navigationService;
	}

	[RelayCommand]
	private void Exit()
	{
		_authService.ClearAuthData();
		_navigationService.ShowAuth();
	}
}
