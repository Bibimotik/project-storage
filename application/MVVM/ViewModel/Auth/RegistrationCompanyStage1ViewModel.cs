using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationCompanyStage1ViewModel : ObservableObject
{
	private readonly Dictionary<string, Action<string?>> _validationActions;
	
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
		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(Inn), value => IsInvalidInn = ValidateAndCreateModel(value) },
			{ nameof(Kpp), value => IsInvalidKpp = ValidateAndCreateModel(value) },
			{ nameof(FullName), value => IsInvalidFullName = ValidateAndCreateModel(value) },
			{ nameof(ShortName), value => IsInvalidShortName = ValidateAndCreateModel(value) },
			{ nameof(LegalAddress), value => IsInvalidLegalAddress = ValidateAndCreateModel(value) },
			{ nameof(PostalAddress), value => IsInvalidPostalAddress = ValidateAndCreateModel(value) },
			{ nameof(Ogrn), value => IsInvalidOgrn = ValidateAndCreateModel(value) }
		};
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
		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}
	
	private void OnInvalided(string property)
	{
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
}
