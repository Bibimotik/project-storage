using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;
public partial class AddStorageViewModel : ObservableObject
{
	private readonly IEntityStorageRepository _entityStorageRepository;
	public static event Action? OpenStorage;

	private Guid? _storageId = null;

	[ObservableProperty]
	private string? point;

	[ObservableProperty]
	private string? country;

	[ObservableProperty]
	private string? city;

	[ObservableProperty]
	private string? address;

	[ObservableProperty]
	private string? index;

	public AddStorageViewModel(IEntityStorageRepository entityStorageRepository)
	{
		_entityStorageRepository = entityStorageRepository;
	}

	//true if Add, false if Edit
	[RelayCommand]
	public async Task SaveStorage(string isAddOrEdit)
	{
		if (string.IsNullOrWhiteSpace(Point) ||
			string.IsNullOrWhiteSpace(Country) ||
			string.IsNullOrWhiteSpace(City) ||
			string.IsNullOrWhiteSpace(Address) ||
			string.IsNullOrWhiteSpace(Index))
		{
			//TODO - можно дописать красивую валидацию
			MessageBox.Show("Заполните все поля");
			return;
		}

		Guid id = _storageId is null ? Guid.NewGuid() : (Guid)_storageId;

		EntityStorageModel storageModel = new(
			id,
			EntityModel.OurUserModel.EntityId,
			Point!,
			Country!,
			City!,
			Address!,
			Index!
		);

		if (Equals(isAddOrEdit, true.ToString()))
		{
			await _entityStorageRepository.InsertEntityStorage(EntityModel.OurUserModel.EntityId, storageModel);

			ClearFields();
		}
		if (Equals(isAddOrEdit, false.ToString()))
		{
			await _entityStorageRepository.UpdateEntityStorage(EntityModel.OurUserModel.EntityId, storageModel);

			TriggerBackStorage();
		}
	}

	public async void LoadStorage()
	{
		_storageId = null;
	}

	public async void LoadStorage(Guid storageId)
	{
		var storage = await _entityStorageRepository.GetEntityStorage(storageId);
		_storageId = storageId;

		Point = storage.Point;
		Country = storage.Country;
		City = storage.City;
		Address = storage.Address;
		Index = storage.Index;
	}

	private void ClearFields()
	{
		Point = string.Empty;
		Country = string.Empty;
		City = string.Empty;
		Address = string.Empty;
		Index = string.Empty;
	}

	[RelayCommand]
	public void TriggerBackStorage()
	{
		OpenStorage?.Invoke();
	}
}