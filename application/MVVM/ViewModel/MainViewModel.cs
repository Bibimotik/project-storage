using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel;

//[ObservableObject]
public partial class MainViewModel : ObservableObject
{
	[ObservableProperty]
	private string name;

	public MainViewModel()
	{
		Name = "bibimotik";
	}
}
