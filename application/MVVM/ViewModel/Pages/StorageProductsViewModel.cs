using System.Collections.ObjectModel;
using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageProductsViewModel : ObservableObject
{
	private readonly IStorageProductsRepository _productsRepository;
	public Guid StorageId { get; private set; }
	private string _orderBy = "";

	public static event Action<Guid>? OpenAddProduct;
	public static event Action<Guid, Guid>? OpenEditProduct;
	public static event Action? CloseAddProduct;

	public ObservableCollection<ProductDataResult> Products { get; } = [];

	public StorageProductsViewModel(IStorageProductsRepository productsRepository)
	{
		_productsRepository = productsRepository;
	}

	public async Task LoadProductsAsync(Guid storageId, string searchQuery = "")
	{
		StorageId = storageId;
		var products = await _productsRepository.GetProductsDataAsync(storageId, searchQuery, _orderBy);
		Products.Clear();

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}
	
	[RelayCommand]
	public async Task OnSortChanged(string sortOption)
	{
		if (sortOption == "По алфавиту от А до Я")
		{
			_orderBy = "ASC";
		}
		else if (sortOption == "По алфавиту от Я до А")
		{
			_orderBy = "DESC";
		}
		else
		{
			_orderBy = "";
		}

		await LoadProductsAsync(StorageId);
	}

	[RelayCommand]
	public void TriggerAddProduct()
	{
		OpenAddProduct?.Invoke(StorageId);
	}
	
	[RelayCommand]
	private void Back()
	{
		CloseAddProduct?.Invoke();
	}

	[RelayCommand]
	public void EditProduct(Guid productId)
	{
		OpenEditProduct?.Invoke(StorageId, productId);
	}

	[RelayCommand]
	public async Task DeleteProduct(Guid productId)
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите удалить выбранный элемент?",
			"Подтверждение удаления"
			))
			return;

		var isDeleted = await _productsRepository.MarkProductAsDeletedAsync(productId);
		if (isDeleted)
		{
			await LoadProductsAsync(StorageId);
		}
	}
}