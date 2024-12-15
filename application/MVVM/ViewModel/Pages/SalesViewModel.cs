using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class SalesViewModel : ObservableObject
{
	public static event Action? OpenAddSale;
	
	[RelayCommand]
	public void TriggerAddStaff() => OpenAddSale?.Invoke();
}