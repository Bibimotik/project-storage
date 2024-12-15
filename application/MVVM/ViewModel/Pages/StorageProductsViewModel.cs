using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

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

	public ObservableCollection<ProductDataResult> Products { get; } = new();

	public StorageProductsViewModel(IStorageProductsRepository productsRepository)
	{
		_productsRepository = productsRepository;
	}

	public async Task LoadProductsAsync(Guid storageId)
	{
		StorageId = storageId;
		var products = await _productsRepository.GetProductsDataAsync(storageId);
		Products.Clear();

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
}