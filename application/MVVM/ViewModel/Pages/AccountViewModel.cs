using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class AccountViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;
	private readonly IAccountRepository _accountRepository;
	private readonly IEntityRepository _entityRepository;
	private readonly IPasswordHash _passwordHash;

	[ObservableProperty]
	private EntityType currentType;
	[ObservableProperty]
	private EntityModel accountData;
	[ObservableProperty]
	private bool isPasswordReadOnly = false;

	[ObservableProperty]
	private string password = string.Empty;
	[ObservableProperty]
	private bool isInvalidPassword = false;

	public AccountViewModel(
		IAuthService authService,
		INavigationService navigationService,
		IAccountRepository accountRepository,
		IEntityRepository entityRepository,
		IPasswordHash passwordHash)
	{
		_authService = authService;
		_navigationService = navigationService;
		_accountRepository = accountRepository;
		_entityRepository = entityRepository;
		_passwordHash = passwordHash;
	}

	[RelayCommand]
	private void LoadType()
	{
		Password = string.Empty;
		IsPasswordReadOnly = false;

		AccountData = new EntityModel
		{
			Id = EntityModel.OurUserModel.Id,
			INN = EntityModel.OurUserModel.INN,
			KPP = EntityModel.OurUserModel.KPP,
			OGRN = EntityModel.OurUserModel.OGRN,
			FullName = EntityModel.OurUserModel.FullName,
			ShortName = EntityModel.OurUserModel.ShortName,
			LegalAddress = EntityModel.OurUserModel.LegalAddress,
			PostalAddress = EntityModel.OurUserModel.PostalAddress,
			Director = EntityModel.OurUserModel.Director,
			FirstName = EntityModel.OurUserModel.FirstName,
			SecondName = EntityModel.OurUserModel.SecondName,
			ThirdName = EntityModel.OurUserModel.ThirdName,
			Phone = EntityModel.OurUserModel.Phone,
			Email = EntityModel.OurUserModel.Email,
			Password = EntityModel.OurUserModel.Password,
			Logo = EntityModel.OurUserModel.Logo,
			EntityType = EntityModel.OurUserModel.EntityType
		};
		CurrentType = EntityModel.OurUserModel.EntityType;
	}
	[RelayCommand]
	private async Task Save()
	{
		//if(EntityModel.OurUserModel == AccountData)
		//{
		//	MessageBox.Show("Вы не изменили данные акканута");
		//	return;
		//}
		//if() // расшифровывать пароль, сравинвать значения и зашифровывать новый
		// но расшифровать нельзя

	   if (await _entityRepository.Update(AccountData))
			MessageBox.Show("Данные обновленны");
	   else
			MessageBox.Show("Данные для обновления не валидны");
	}
	partial void OnPasswordChanged(string value)
	{
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);

			if (!regex.IsMatch(value))
			{
				IsInvalidPassword = true;
				return;
			}

			IsInvalidPassword = false;
			AccountData.Password = _passwordHash.Generate(value);
		}
		catch (RegexMatchTimeoutException)
		{
			IsInvalidPassword = false;
			AccountData.Password = _passwordHash.Generate(value);
			return;
		}
	}
	[RelayCommand]
	private void Exit()
	{
		_authService.ClearAuthData();
		_navigationService.ShowAuth();
	}
	[RelayCommand]
	private void ChangePassword()
	{
		IsPasswordReadOnly = !IsPasswordReadOnly;
	}
	[RelayCommand]
	private async Task DeleteAccount()
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите удалить аккаунт?",
			"Подтверждение удаления"
			))
			return;

		await _accountRepository.MarkUserAsDeletedAsync(EntityModel.OurUserModel.Id);

		_authService.ClearAuthData();
		_navigationService.ShowAuth();
	}

	public bool IsUserVisible => CurrentType == EntityType.User;
	public bool IsCompanyVisible => CurrentType == EntityType.Company;

	partial void OnCurrentTypeChanged(EntityType value)
	{
		OnPropertyChanged(nameof(IsUserVisible));
		OnPropertyChanged(nameof(IsCompanyVisible));
	}
}