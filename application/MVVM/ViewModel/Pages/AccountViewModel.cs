using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using application.Repositories;

namespace application.MVVM.ViewModel.Pages;

public partial class AccountViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;
	private readonly IAccountRepository _accountRepository;

	public AccountViewModel(IAuthService authService, INavigationService navigationService, IAccountRepository accountRepository)
	{
		_authService = authService;
		_navigationService = navigationService;
		_accountRepository = accountRepository;
	}

	[RelayCommand]
	private async void Exit()
	{
		_authService.ClearAuthData();
		_navigationService.ShowAuth();
	}

	[RelayCommand]
	private async void DeleteAccount()
	{
		var entity = await _accountRepository.GetEntityIdAsync(EntityModel.OurUserModel.Id);

		MessageBox.Show($"Type: {entity.Type}, TypeId: {entity.Type_Id}");
	}
}