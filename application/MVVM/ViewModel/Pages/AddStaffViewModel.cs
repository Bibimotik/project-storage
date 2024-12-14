using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class AddStaffViewModel : ObservableObject
{
	private readonly IAddStaffRepository _addStaffRepository;
	public static event Action? OpenStaff;

	public AddStaffViewModel(IAddStaffRepository staffRepository)
	{
		_addStaffRepository = staffRepository;
	}
	
	[ObservableProperty]
	private string? email;
	
	[ObservableProperty]
	private string? user_id;
	
	[ObservableProperty]
	private string? access;
	
	[ObservableProperty]
	private string? name;

	[RelayCommand]
	public async Task AddStaffAsync()
	{
		MessageBox.Show("edledlpedlp");
		if (string.IsNullOrWhiteSpace(User_id))
		{
			//TODO - можно дописать красивую валидацию
			MessageBox.Show("Заполните все поля");
			return;
		}
		
		EntityManagerModel managerModel = new(
			Guid.NewGuid(),
			Guid.Parse(User_id),
			Access
		);

		//TODO - передавать uuid нашего entity
		Guid entityId = Guid.Parse("cde4838c-5b14-4eb7-8eb9-24ee52db62b3");
		await _addStaffRepository.InsertStaff(entityId, managerModel);
	}

	[RelayCommand]
	public async Task GetUser()
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			User_id = "Введите email.";
			Name = string.Empty;
			return;
		}

		try
		{
			var (userId, userName) = await _addStaffRepository.GetUser(email);
        
			User_id = $"{userId}";
			Name = userName;
		}
		catch (InvalidOperationException ex)
		{
			User_id = ex.Message;
			Name = string.Empty;
		}
		catch (Exception ex)
		{
			User_id = $"Произошла ошибка: {ex.Message}";
			Name = string.Empty;
		}
	}

	[RelayCommand]
	public void TriggerStaff() => OpenStaff?.Invoke();
}