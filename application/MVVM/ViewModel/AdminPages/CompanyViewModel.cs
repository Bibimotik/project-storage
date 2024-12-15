using System.Windows.Controls;
using System.Windows.Data;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.AdminPages;

public partial class CompanyViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private IList<EntityModel> data = [];

	public CompanyViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		Data = await _tablesRepository.GetData<EntityModel>(TableNames.Company);
	}
}
