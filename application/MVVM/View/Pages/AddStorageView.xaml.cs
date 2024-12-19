using System.Windows;
using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;
/// <summary>
/// Логика взаимодействия для AddStorageView.xaml
/// </summary>
public partial class AddStorageView : UserControl
{
	public AddStorageView(AddStorageViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();

		viewModel.LoadStorage();
		Label.Content = "Add storage";
		EditButton.Visibility = Visibility.Collapsed;
	}
	public AddStorageView(AddStorageViewModel viewModel, Guid storageId)
	{
		DataContext = viewModel;
		InitializeComponent();

		viewModel.LoadStorage(storageId);

		Label.Content = "Edit storage";
		SaveButton.Visibility = Visibility.Collapsed;
	}
}
