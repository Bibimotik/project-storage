using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class AddProductViewModel : ObservableObject
{
	private readonly IAddProductRepository _productRepository;
	public static event Action? OpenStorage;

	[RelayCommand]
	public void TriggerBackProduct()
	{
		OpenStorage?.Invoke();
	}
}