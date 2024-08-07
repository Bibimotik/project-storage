using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationCompanyStage1ViewModel : ObservableObject
{
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

	partial void OnInnChanged(string value) => CreateModel();
	partial void OnKppChanged(string value) => CreateModel();
	partial void OnFullNameChanged(string value) => CreateModel();
	partial void OnShortNameChanged(string value) => CreateModel();
	partial void OnLegalAddressChanged(string value) => CreateModel();
	partial void OnPostalAddressChanged(string value) => CreateModel();
	partial void OnOgrnChanged(string value) => CreateModel();

	private void CreateModel()
	{
		RegistrationModel.Model = RegistrationModel.CreateCompany(Inn, Kpp, FullName, ShortName, LegalAddress, PostalAddress, Ogrn);
	}
}
