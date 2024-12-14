using System.Collections.ObjectModel;
using System.Threading.Tasks;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Pages
{
	public class RolesViewModel : ObservableObject
	{
		private readonly IRolesRepository _rolesRepository;
        
		public ObservableCollection<RoleDataResult> Roles { get; } = new ObservableCollection<RoleDataResult>();

		public RolesViewModel(IRolesRepository rolesRepository)
		{
			_rolesRepository = rolesRepository;
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
	}
}