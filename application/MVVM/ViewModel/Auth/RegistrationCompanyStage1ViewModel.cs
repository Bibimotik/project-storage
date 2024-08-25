using System.Diagnostics;
using System.Net.Http;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;
using application.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Newtonsoft.Json;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationCompanyStage1ViewModel : ObservableObject
{
	private readonly Dictionary<string, Action<string?>> _validationActions;
	private readonly bool _isInitializing = false;

	[ObservableProperty]
	private string inn = string.Empty;
	[ObservableProperty]
	private string kpp = string.Empty;
	[ObservableProperty]
	private string fullName = string.Empty;
	[ObservableProperty]
	private string shortName = string.Empty;
	[ObservableProperty]
	private string legalAddress = string.Empty;
	[ObservableProperty]
	private string postalAddress = string.Empty;
	[ObservableProperty]
	private string ogrn = string.Empty;

	[ObservableProperty]
	private bool isInvalidInn = false;
	[ObservableProperty]
	private bool isInvalidKpp = false;
	[ObservableProperty]
	private bool isInvalidFullName = false;
	[ObservableProperty]
	private bool isInvalidShortName = false;
	[ObservableProperty]
	private bool isInvalidLegalAddress = false;
	[ObservableProperty]
	private bool isInvalidPostalAddress = false;
	[ObservableProperty]
	private bool isInvalidOgrn = false;

	public RegistrationCompanyStage1ViewModel()
	{
		_isInitializing = true;

		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(EntityModel.INN), value => IsInvalidInn = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.KPP), value => IsInvalidKpp = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.FullName), value => IsInvalidFullName = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.ShortName), value => IsInvalidShortName = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.LegalAddress), value => IsInvalidLegalAddress = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.PostalAddress), value => IsInvalidPostalAddress = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.OGRN), value => IsInvalidOgrn = ValidateAndCreateModel(value) }
		};

		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		Inn = model.INN;
		Kpp = model.KPP;
		FullName = model.FullName;
		ShortName = model.ShortName;
		LegalAddress = model.LegalAddress;
		PostalAddress = model.PostalAddress;
		Ogrn = model.OGRN;

		_isInitializing = false;
	}

	partial void OnInnChanged(string value) => IsInvalidInn = ValidateAndCreateModel(value);
	partial void OnKppChanged(string value) => IsInvalidKpp = ValidateAndCreateModel(value);
	partial void OnFullNameChanged(string value) => IsInvalidFullName = ValidateAndCreateModel(value);
	partial void OnShortNameChanged(string value) => IsInvalidShortName = ValidateAndCreateModel(value);
	partial void OnLegalAddressChanged(string value) => IsInvalidLegalAddress = ValidateAndCreateModel(value);
	partial void OnPostalAddressChanged(string value) => IsInvalidPostalAddress = ValidateAndCreateModel(value);
	partial void OnOgrnChanged(string value) => IsInvalidOgrn = ValidateAndCreateModel(value);

	private bool ValidateAndCreateModel(string? value)
	{
		if (_isInitializing)
			return false;

		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}

	private void OnInvalided(string property)
	{
		Debug.WriteLine("invalided " + property);
		if (_validationActions.TryGetValue(property, out var validate))
		{
			validate(string.Empty);
		}
	}

	private void CreateModel()
	{
		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Company;
		model.Id = Guid.NewGuid();
		model.INN = Inn;
		model.KPP = Kpp;
		model.FullName = FullName;
		model.ShortName = ShortName;
		model.LegalAddress = LegalAddress;
		model.PostalAddress = PostalAddress;
		model.OGRN = Ogrn;
	}
	
	[RelayCommand]
	public async Task GetParserDataINN(string inputINN)
	{
		var parserInnService = new ParserINNService();
		var parserData = await parserInnService.GetParserDataAsync(inputINN);

		if (parserData != null)
		{
			EntityModel.Model ??= new EntityModel();

			EntityModel model = EntityModel.Model;
			model.INN = inputINN;
			model.KPP = parserData.Kpp;
			model.FullName = parserData.FullName;
			model.ShortName = parserData.ShortName;
			model.OGRN = parserData.Ogrn;
			model.Director = parserData.Director;

			Kpp = parserData.Kpp;
			FullName = parserData.FullName;
			ShortName = parserData.ShortName;
			Ogrn = parserData.Ogrn;
		}
		else
		{
			MessageBox.Show("Не удалось получить данные.");
		}
	}

}