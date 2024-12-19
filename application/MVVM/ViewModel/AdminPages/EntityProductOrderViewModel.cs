using System.Collections.ObjectModel;
using System.Diagnostics;

using application.Abstraction.Interfaces;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using static application.Abstraction.EntityAbstraction;

using T = application.MVVM.Model.EntityProductOrderModel;

namespace application.MVVM.ViewModel.AdminPages;

public partial class EntityProductOrderViewModel : ObservableObject
{
	private readonly ITablesRepository _tablesRepository;

	public EntityProductOrderViewModel(ITablesRepository tablesRepository)
	{
		_tablesRepository = tablesRepository;

		_ = LoadData();
	}


	[ObservableProperty]
	private ObservableCollection<T> data = [];

	private readonly HashSet<T> _modifiedRows = [];
	// Метод для добавления строки в список изменённых
	public void MarkAsModified(T row)
	{
		if (row != null && !_modifiedRows.Contains(row))
		{
			_modifiedRows.Add(row);
		}
	}

	[RelayCommand]
	private async Task Delete(Guid id)
	{
		if (!TableHelper.ShowConfirmationMessage(
			"Вы действительно хотите удалить выбранный элемент?",
			"Подтверждение удаления"
			))
			return;

		await _tablesRepository.DeleteData<T>(TableNames.Entity_product_order, id);

		var entityToRemove = Data.FirstOrDefault(e => e.ID == id);
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

		foreach (var row in _modifiedRows)
			await _tablesRepository.UpdateData(TableNames.User, row);
	}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.Entity_product_order);

		Data = new ObservableCollection<T>(entities);
	}
}
