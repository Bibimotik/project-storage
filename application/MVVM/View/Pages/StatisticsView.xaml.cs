using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StatisticsView : UserControl
{
	private readonly StatisticsViewModel _viewModel;

	public StatisticsView(StatisticsViewModel viewModel)
    {
		_viewModel = viewModel;
	    DataContext = viewModel;
        InitializeComponent();
    }

	private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
	{
		await _viewModel.LoadStatisticsAsync();
	}
}