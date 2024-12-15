using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.EntityStorageModel;

namespace application.MVVM.ViewModel.AdminPages;
public partial class EntityStorageViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private ObservableCollection<T> data = [];
	public EntityStorageViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.Entity_storage);

		Data = new ObservableCollection<T>(entities);
	}

	[RelayCommand]
	private async Task Delete(Guid id)
	{
		await _tablesRepository.DeleteData<T>(TableNames.Entity_storage, id);

		var entityToRemove = Data.FirstOrDefault(e => e.Id == id);
		if (entityToRemove != null)
			Data.Remove(entityToRemove);
	}
}
