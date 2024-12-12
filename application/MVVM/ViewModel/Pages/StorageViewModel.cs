using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageViewModel : ObservableObject
{
	public static event Action? OpenAddStorage;

	public StorageViewModel()
	{
	}

	[RelayCommand]
	public void TriggerAddStorage()
	{

		OpenAddStorage?.Invoke();
	}
}