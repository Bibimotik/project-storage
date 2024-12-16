using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.ProductModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class ProductViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private ObservableCollection<T> data = [];

	public ProductViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.Product);

		Data = new ObservableCollection<T>(entities);
	}
}
