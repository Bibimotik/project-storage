using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.SupportModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class SupportViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private ObservableCollection<T> data = [];

	public SupportViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.Support);

		Data = new ObservableCollection<T>(entities);
	}
}
