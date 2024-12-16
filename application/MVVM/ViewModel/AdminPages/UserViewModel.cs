using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.EntityModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class UserViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	[ObservableProperty]
	private ObservableCollection<T> data = [];

	public UserViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.User);

		Data = new ObservableCollection<T>(entities);
	}

	[RelayCommand]
	private async Task Delete(Guid id)
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите удалить выбранный элемент?",
			"Подтверждение удаления"
			))
			return;

		await _tablesRepository.DeleteData<T>(TableNames.User, id);
		await _tablesRepository.DeleteEntity(id);

		var entityToRemove = Data.FirstOrDefault(e => e.Id == id);
		if (entityToRemove != null)
			Data.Remove(entityToRemove);
	}

	[RelayCommand]
	private async Task Save()
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите сохранить изменения?",
			"Подтверждение изменения"
			))
			return;
	}
}