using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.EntityManagerModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class EntityManagersViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private ObservableCollection<T> data = [];

	public EntityManagersViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.Entity_managers);

		Data = new ObservableCollection<T>(entities);
	}
}
