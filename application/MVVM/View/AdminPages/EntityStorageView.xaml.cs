using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityStorageView.xaml
/// </summary>
public partial class EntityStorageView : UserControl
{
	public EntityStorageView(EntityStorageViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}