using application.Abstraction.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.EntityModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class CompanyViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private IList<T> data = [];

	public CompanyViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		Data = await _tablesRepository.GetData<T>(TableNames.Company);
	}
}
