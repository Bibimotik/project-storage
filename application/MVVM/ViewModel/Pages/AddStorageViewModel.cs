using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;
public partial class AddStorageViewModel : ObservableObject
{
	public static event Action? OpenStorage;

	public AddStorageViewModel()
	{
	}

	[RelayCommand]
	public void TriggerBackStorage() => OpenStorage?.Invoke();
}
