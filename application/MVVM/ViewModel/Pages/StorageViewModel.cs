using System.Collections.ObjectModel;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageViewModel : ObservableObject
{
	private readonly IStorageRepository _storageRepository;
	public ObservableCollection<StorageDataResult> Storage { get; } = new();

	public static event Action? OpenAddStorage;
	public static event Action? OpenStorageProducts;

	public StorageViewModel(IStorageRepository storageRepository)
	{
		_storageRepository = storageRepository;
	}

	public async Task LoadStorageAsync(Guid entityId)
	{
		var storageData = await _storageRepository.GetStorageDataAsync(entityId);
		Storage.Clear();

		foreach (var storage in storageData)
		{
			Storage.Add(storage);
		}
	}

	[RelayCommand]
	public void TriggerAddStorage() => OpenAddStorage?.Invoke();
	[RelayCommand]
	public void TriggerShowStorage() => OpenStorageProducts?.Invoke();
}