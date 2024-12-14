using application.MVVM.Model;
using application.Repositories;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class CompanyViewModel : ObservableObject
{
	private readonly TablesRepository _tablesRepository;

	[ObservableProperty]
	private IList<EntityModel> data = [];

	public CompanyViewModel(TablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		Data = await _tablesRepository.GetCompanies();
	}
}
