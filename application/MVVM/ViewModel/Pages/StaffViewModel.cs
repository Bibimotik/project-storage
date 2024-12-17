using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class StaffViewModel : ObservableObject
{
	private readonly IStaffRepository _staffRepository;
	public static event Action? OpenAddStaff;
	public ObservableCollection<StaffMember> StaffMembers { get; } = new();

	public StaffViewModel(IStaffRepository staffRepository)
	{
		_staffRepository = staffRepository;
	}

	public async Task LoadStaffAsync(Guid entityId)
	{
		StaffMembers.Clear();
		var staff = await _staffRepository.GetStaffByEntityIdAsync(entityId);
		foreach (var member in staff)
		{
			StaffMembers.Add(member);
		}
	}
    
	[RelayCommand]
	public void TriggerAddStaff() => OpenAddStaff?.Invoke();
	
	[RelayCommand]
	public async Task DeleteStaffMemberAsync(Guid staffId)
	{
		var isDeleted = await _staffRepository.DeleteStaffMemberAsync(staffId);
		if (isDeleted)
		{
			await LoadStaffAsync(EntityModel.OurUserModel.EntityId);
		}
	}
}