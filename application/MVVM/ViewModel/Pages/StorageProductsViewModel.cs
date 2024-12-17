using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageProductsViewModel : ObservableObject
{
	private readonly IStorageProductsRepository _productsRepository;
	public Guid StorageId { get; private set; }

	public static event Action<Guid>? OpenAddProduct;
	public static event Action? CloseAddProduct;

	public ObservableCollection<ProductDataResult> Products { get; } = [];

	public StorageProductsViewModel(IStorageProductsRepository productsRepository)
	{
		_productsRepository = productsRepository;
	}

	public async Task LoadProductsAsync(Guid storageId, string searchQuery = "")
	{
		StorageId = storageId;
		Products.Clear();
		var products = await _productsRepository.GetProductsDataAsync(storageId, searchQuery, _orderBy);

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}

	[RelayCommand]
	public void TriggerAddProduct(Guid storageId)
	{
		OpenAddProduct?.Invoke(StorageId);
	}
	[RelayCommand]
	private void Back()
	{
		CloseAddProduct?.Invoke();
	}
}