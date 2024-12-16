using System.Collections.ObjectModel;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

namespace application.MVVM.ViewModel.Pages;

public partial class SalesViewModel : ObservableObject
{
	public static event Action? OpenAddSale;
	
	[RelayCommand]
	public void TriggerAddStaff() => OpenAddSale?.Invoke();
	private readonly ISaleRepository _saleRepository;

	public ObservableCollection<OrderModel> Orders { get; set; } = new();
	private string _orderBy = "";

	public SalesViewModel(ISaleRepository saleRepository)
	{
		_saleRepository = saleRepository;
	}

	public async Task LoadOrdersAsync(Guid entityId, string searchQuery = "")
	{
		Orders.Clear();
		
		var orders = await _saleRepository.GetOrdersByEntityIdAsync(entityId, searchQuery, _orderBy);

		foreach (var order in orders)
		{
			Orders.Add(order);
		}
	}
}