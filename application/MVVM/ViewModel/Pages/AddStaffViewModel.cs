using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class AddStaffViewModel : ObservableObject
{
	private readonly IStaffRepository _addStaffRepository;
	private readonly IEntityRepository _entityRepository;
	public static event Action? OpenStaff;

	public AddStaffViewModel(IStaffRepository staffRepository, IEntityRepository entityRepository)
	{
		_addStaffRepository = staffRepository;
		_entityRepository = entityRepository;
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

		bool exists = await _addStaffRepository.CheckIfStaffExists(
			EntityModel.OurUserModel.EntityId,
			managerModel.User_ID,
			managerModel.Access
		);

		if (exists)
		{
			MessageBox.Show("Пользователь с такой должностью уже существует.");
			return;
		}

		// Выполняем вставку
		await _addStaffRepository.InsertStaff(EntityModel.OurUserModel.EntityId, managerModel);
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
		if(string.Equals(email, EntityModel.OurUserModel.Email))
		{
			MessageBox.Show("Нельзя добавить самого себя");
			return;
		}

		try
		{
			var (userId, userName) = await _entityRepository.GetUser(EntityModel.OurUserModel.Id, email);

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