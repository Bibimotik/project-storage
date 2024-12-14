using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StaffViewModel : ObservableObject
{
	public static event Action? OpenAddStaff;

	public StaffViewModel()
	{
	}
	
	[RelayCommand]
	public void TriggerAddStaff() => OpenAddStaff?.Invoke();
}