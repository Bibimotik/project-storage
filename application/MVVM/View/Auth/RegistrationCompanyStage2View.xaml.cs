using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace application.MVVM.View.Auth;
/// <summary>
/// Логика взаимодействия для RegistrationCompanyStage2View.xaml
/// </summary>
public partial class RegistrationCompanyStage2View : UserControl
{
	public RegistrationCompanyStage2View()
	{
		InitializeComponent();
	}

	private void PasswordBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}
}
