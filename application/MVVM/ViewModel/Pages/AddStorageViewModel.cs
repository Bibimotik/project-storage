using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;
public partial class AddStorageViewModel : ObservableObject
{
	private readonly IEntityStorageRepository _entityStorageRepository;
	public static event Action? OpenStorage;

	public AddStorageViewModel(IEntityStorageRepository entityStorageRepository)
	{
		_entityStorageRepository = entityStorageRepository;
	}
	
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

	[RelayCommand]
	public async Task AddEntityAsync()
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

		EntityStorageModel storageModel = new(
			Guid.NewGuid(),
			Point!,
			Country!,
			City!,
			Address!,
			Index!
		);

		//TODO - передавать uuid нашего entity
		Guid entityId = Guid.Parse("52d6177b-bac0-447d-9ff3-fdf10e344b6e");
		await _entityStorageRepository.InsertEntityStorage(entityId, storageModel);

		ClearFields();
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
	public void TriggerBackStorage() => OpenStorage?.Invoke();
}
