using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class AccountViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;
	private readonly IAccountRepository _accountRepository;
	private readonly IEntityRepository _entityRepository;
	private readonly IPasswordHash _passwordHash;

	public static event Action? UpdateLogo;

	[ObservableProperty]
	private string selectedFilePath = "Select File";
	[ObservableProperty]
	private string menuIconPath = "../../../Assets/Icons/Ava.png";
	[ObservableProperty]
	private object image;
	[ObservableProperty]
	private EntityType currentType;
	[ObservableProperty]
	private EntityModel accountData = new();
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

		UpdateMenuTag();
	}

	[RelayCommand]
	private void LoadType()
	{
		if (EntityModel.OurUserModel == null)
			return;

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
		//Logo = ConvertLogoToImage(ImageHelper.ConvertImageToByteArray(menuIconPath));
		UpdateMenuTag();
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

		EntityModel.OurUserModel = AccountData;
		UpdateLogo?.Invoke();
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
	[RelayCommand]
	private void SelectFile()
	{
		var openFileDialog = new OpenFileDialog
		{
			Filter = "Logo Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
			Multiselect = false // Позволяем выбрать только один файл
        };

		if (openFileDialog.ShowDialog() == true)
		{
			try
			{
				SelectedFilePath = openFileDialog.FileName;
				var bytes = ImageHelper.ConvertImageToByteArray(SelectedFilePath);
				Image = ConvertLogoToImage(bytes);
				AccountData.Logo = bytes;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading image: {ex.Message}");
				SelectedFilePath = "Error loading file";
				Image = null;
			}
		}
		else
		{
			SelectedFilePath = "Select File";
		}
	}

	public bool IsUserVisible => CurrentType == EntityType.User;
	public bool IsCompanyVisible => CurrentType == EntityType.Company;

	partial void OnCurrentTypeChanged(EntityType value)
	{
		OnPropertyChanged(nameof(IsUserVisible));
		OnPropertyChanged(nameof(IsCompanyVisible));
	}

	private void UpdateMenuTag()
	{
		if(EntityModel.OurUserModel == null)
		{
			return; 
		}
		if (EntityModel.OurUserModel.Logo == null || EntityModel.OurUserModel.Logo.Length == 0)
		{
			Image = MenuIconPath; // Путь к стандартной иконке
			AccountData.Logo = ImageHelper.ConvertImageToByteArray(MenuIconPath);
		}
		else
		{
			Image = ConvertLogoToImage(EntityModel.OurUserModel.Logo); // Преобразуем Logo в изображение
			AccountData.Logo = EntityModel.OurUserModel.Logo;
		}
	}

	private ImageSource ConvertLogoToImage(byte[] logoBytes)
	{
		try
		{
			var image = new BitmapImage();
			using (var stream = new MemoryStream(logoBytes))
			{
				stream.Position = 0;
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;
				image.EndInit();
			}
			return image;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error converting logo to image: {ex.Message}");
			return null; // Возвращаем null, если произошла ошибка
		}
	}
}