using System.Collections.ObjectModel;
using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class RolesViewModel : ObservableObject
{
	private readonly IRolesRepository _rolesRepository;
	private readonly INavigationService _navigationService;

	public ObservableCollection<RoleDataResult> Roles { get; } = [];

	[ObservableProperty]
	private UserRole currentRole;

	public RolesViewModel(IRolesRepository rolesRepository, INavigationService navigationService)
	{
		_rolesRepository = rolesRepository;
		_navigationService = navigationService;
	}

	[RelayCommand]
	private void LoadRole()
	{
		CurrentRole = EntityModel.OurUserModel.Role;
	}
	[RelayCommand]
	private void ExitFromRole()
	{
		_navigationService.ShowMain();	
	}

	public async Task LoadRolesAsync(Guid userId)
	{
		var rolesData = await _rolesRepository.GetEntityDataAsync(userId);
		Roles.Clear();

		foreach (var role in rolesData)
		{
			Roles.Add(role);
		}
	}

	public bool IsNoRoleVisible => CurrentRole == UserRole.NoRole;
	public bool IsWorkerVisible => CurrentRole != UserRole.NoRole;

	partial void OnCurrentRoleChanged(UserRole value)
	{
		OnPropertyChanged(nameof(IsNoRoleVisible));
		OnPropertyChanged(nameof(IsWorkerVisible));
	}
}