using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StatisticsView : UserControl
{
    public StatisticsView(StatisticsViewModel viewModel)
    {
	    DataContext = viewModel;
        InitializeComponent();
    }
}