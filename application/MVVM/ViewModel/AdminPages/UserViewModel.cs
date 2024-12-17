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


	//public ObservableCollection<Dictionary<string, object>> Users { get; } = new();

	//// Хранение оригинальных данных
	//private readonly Dictionary<Guid, Dictionary<string, object>> _originalValues = new();

	//[RelayCommand]
	//public void SaveChanges()
	//{
	//	// Находим измененные строки
	//	var changedUsers = Users.Where(user =>
	//	{
	//		var id = (Guid)user["Id"];
	//		if (!_originalValues.TryGetValue(id, out var original))
	//			return false;

	//		return user.Any(kv =>
	//			original.ContainsKey(kv.Key) &&
	//			!Equals(original[kv.Key]?.ToString()?.Trim(), kv.Value?.ToString()?.Trim()));
	//	}).ToList();

	//	if (!changedUsers.Any())
	//	{
	//		// Нет изменений
	//		return;
	//	}

	//	foreach (var user in changedUsers)
	//	{
	//		var id = (Guid)user["Id"];
	//		// Сохранение пользователя в БД
	//		SaveUser(user);

	//		// Обновляем оригинальные значения
	//		if (_originalValues.ContainsKey(id))
	//		{
	//			foreach (var kv in user)
	//			{
	//				_originalValues[id][kv.Key] = kv.Value;
	//			}
	//		}
	//	}
	//}

	//private void SaveUser(Dictionary<string, object> user)
	//{
	//	// Логика сохранения в базу данных
	//}

	//[RelayCommand]
	//public void ResetAllChanges()
	//{
	//	foreach (var user in Users)
	//	{
	//		var id = (Guid)user["Id"];
	//		if (_originalValues.TryGetValue(id, out var original))
	//		{
	//			foreach (var kv in original)
	//			{
	//				user[kv.Key] = kv.Value;
	//			}
	//		}
	//	}
	//}

	//public void LoadUsers()
	//{
	//	Users.Clear();
	//	_originalValues.Clear();

	//	// Пример загрузки данных
	//	var user1 = new Dictionary<string, object>
	//	{
	//		{ "Id", Guid.NewGuid() },
	//		{ "Name", "Alice" },
	//		{ "Email", "alice@example.com" }
	//	};

	//	var user2 = new Dictionary<string, object>
	//	{
	//		{ "Id", Guid.NewGuid() },
	//		{ "Name", "Bob" },
	//		{ "Email", "bob@example.com" }
	//	};

	//	Users.Add(user1);
	//	Users.Add(user2);

	//}

	private async Task LoadData()
	{
		var entities = await _tablesRepository.GetData<T>(TableNames.User);

		Data = new ObservableCollection<T>(entities);

		//// Сохраняем оригинальные значения
		//foreach (var user in Data)
		//{
		//	var id = (Guid)user["Id"];
		//	_originalValues[id] = new Dictionary<string, object>(user);
		//}
	}
}