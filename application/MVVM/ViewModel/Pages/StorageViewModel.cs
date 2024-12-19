using System.Collections.ObjectModel;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StorageViewModel : ObservableObject
{
    private readonly IStorageRepository _storageRepository;
	public ObservableCollection<StorageDataResult> Storage { get; } = [];
    private string _orderBy = "";

    public static event Action? OpenAddStorage;
    public static event Action<Guid>? OpenStorageProducts;
    public static event Action<Guid>? OpenEditStorage;

	public Guid SelectedStorageId { get; private set; }

	public StorageViewModel(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

    public async Task LoadStorageAsync(Guid entityId, string searchQuery = "")
    {
        var storageData = await _storageRepository.GetStorageDataAsync(entityId, searchQuery, _orderBy);
        Storage.Clear();

        foreach (var storage in storageData)
        {
            Storage.Add(storage);
        }
    }

	[RelayCommand]
	public void TriggerAddStorage()
	{
		OpenAddStorage?.Invoke();
	}

	[RelayCommand]
    public void TriggerShowStorage(Guid storageId)
    {
        OpenStorageProducts?.Invoke(storageId);
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

        await LoadStorageAsync(EntityModel.OurUserModel.EntityId);
    }
    
    [RelayCommand]
    public async Task DeleteStorage(Guid storageId)
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите удалить выбранный элемент?",
			"Подтверждение удаления"
			))
			return;

		var isDeleted = await _storageRepository.MarkStorageAsDeletedAsync(storageId);
	    if (isDeleted)
	    {
		    await LoadStorageAsync(EntityModel.OurUserModel.EntityId);
	    }
	}

	[RelayCommand]
	public void EditStorage(Guid storageId)
	{
		OpenEditStorage?.Invoke(storageId);
	}
}
