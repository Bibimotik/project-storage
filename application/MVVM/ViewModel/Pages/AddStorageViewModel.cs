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

	[RelayCommand]
	public void AddEntity()
	{
		Console.WriteLine("dmkwmdllmdwdl");
		EntityStorageModel storageModel = new(Guid.NewGuid());

		//_entityStorageRepository.InsertEntityStorage("", storageModel);
	}

	[RelayCommand]
	public void TriggerBackStorage() => OpenStorage?.Invoke();
}
