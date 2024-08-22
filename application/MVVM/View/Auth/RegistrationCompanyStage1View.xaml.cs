using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;

namespace application.MVVM.View.Auth;
/// <summary>
/// Логика взаимодействия для RegistrationStage1.xaml
/// </summary>
public partial class RegistrationCompanyStage1View : UserControl
{
	public RegistrationCompanyStage1View()
	{
		// TODO - вот тут это :( <3 пофикси пжпжпжпжжп
		InitializeComponent();
		
		EntityModel model = EntityModel.Model;
		Console.WriteLine("Inn" + model.INN);
		Console.WriteLine("Kpp" + model.KPP);
		Console.WriteLine("FullName" + model.FullName);
		Console.WriteLine("ShortName" + model.ShortName);
		Console.WriteLine("LegalAddress" + model.LegalAddress);
		Console.WriteLine("PostalAddress" + model.PostalAddress);
		Console.WriteLine("OGRN" + model.OGRN);

			if (!string.IsNullOrEmpty(model.INN))
			{
				INN.Text = model.INN;
			}

			if (!string.IsNullOrEmpty(model.KPP))
			{
				KPP.Text = model.KPP;
			}
        
			if (!string.IsNullOrEmpty(model.FullName))
			{
				FullName.Text = model.FullName;
			}

			if (!string.IsNullOrEmpty(model.ShortName))
			{
				ShortName.Text = model.ShortName;
			}

			if (!string.IsNullOrEmpty(model.LegalAddress))
			{
				LegalAddress.Text = model.LegalAddress;
			}

			if (!string.IsNullOrEmpty(model.PostalAddress))
			{
				PostalAddress.Text = model.PostalAddress;
			}

			if (!string.IsNullOrEmpty(model.OGRN))
			{
				OGRN.Text = model.OGRN;
			}
		
	}
}
