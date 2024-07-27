using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}
	
	private void RadioButton_Checked(object sender, RoutedEventArgs e)
	{
		//RadioButton radioButton = (RadioButton)sender;
		
		//if (radioButton != null && radioButton.Name == "Account")
		//{
		//	MainContent.Navigate(new Uri("D:/Учеба/project-storage/application/MVVM/View/Pages/Account.xaml", UriKind.Relative));
		//}

		/*switch (radioButton.Name)
		{
			case "Account": MainContent.Navigate(new Uri("../Pages/Account.xaml", UriKind.Relative)); break;
			case "Statistics": MainContent.Navigate(new Uri("../Pages/Statistics.xaml", UriKind.Relative)); break;
			case "Sales": MainContent.Navigate(new Uri("../Pages/Sales.xaml", UriKind.Relative)); break;
			case "Storage": MainContent.Navigate(new Uri("../Pages/Storage.xaml", UriKind.Relative)); break;
			case "Staff": MainContent.Navigate(new Uri("../Pages/Staff.xaml", UriKind.Relative)); break;
			case "Support": MainContent.Navigate(new Uri("../Pages/Support.xaml", UriKind.Relative)); break;
			case "Info": MainContent.Navigate(new Uri("../Pages/Info.xaml", UriKind.Relative)); break;
			
			default: break;
		}*/
	}
}