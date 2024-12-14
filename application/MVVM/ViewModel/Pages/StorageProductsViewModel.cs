using System.Collections.ObjectModel;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageProductsViewModel : ObservableObject
{
	private readonly IStorageProductsRepository _productsRepository;
	public ObservableCollection<ProductDataResult> Products { get; } = new();

	public StorageProductsViewModel(IStorageProductsRepository productsRepository)
	{
		_productsRepository = productsRepository;
	}

	public async Task LoadProductsAsync(Guid storageId)
	{
		var products = await _productsRepository.GetProductsDataAsync(storageId);
		Products.Clear();

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}
}