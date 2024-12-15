using System.Windows;

using application.Abstraction;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel.Pages;

public partial class AddSaleViewModel : ObservableObject
{
	private readonly IParserINNService _parserInnService;
	public static event Action? OpenSales;
	[ObservableProperty]
	private string inn;

	[ObservableProperty]
	private string kpp;

	[ObservableProperty]
	private string ogrn;

	[ObservableProperty]
	private string fullName;

	[ObservableProperty]
	private string address;

	[ObservableProperty]
	private string paymentAccount;

	[ObservableProperty]
	private string toBIK;

	[ObservableProperty]
	private string toBank;

	[ObservableProperty]
	private string toCorAccount;

	[ObservableProperty]
	private string fromCorAccount;

	[ObservableProperty]
	private string fromBIK;

	[ObservableProperty]
	private string fromBank;

	[ObservableProperty]
	private DateTime planDateShipment;

	[ObservableProperty]
	private string shippingAddress;

	[ObservableProperty]
	private DateTime applicationDate;
	
	[ObservableProperty]
	private Guid entityStorageId;

	[ObservableProperty]
	private string deliveryPoint;

	[ObservableProperty]
	private string deliveryAddress;

	[ObservableProperty]
	private DateTime planDateReceipt;

	[ObservableProperty]
	private string transporterFullName;

	[ObservableProperty]
	private string transporterShortName;

	[ObservableProperty]
	private string comment;

	[ObservableProperty]
	private double vat;

	public AddSaleViewModel()
	{
		PlanDateShipment = DateTime.Now;
		ApplicationDate = DateTime.Now;
		PlanDateReceipt = DateTime.Now;
	}
	
	[RelayCommand]
	public void TriggerSales() => OpenSales?.Invoke();
	
	[RelayCommand]
	public async Task GetParserDataINN(string inputINN)
	{
		var (parserData, error) = await _parserInnService.GetParserDataINN(Inn);

		if (!string.IsNullOrEmpty(error))
		{
			MessageBox.Show(error);
			return;
		}
		if (parserData != null)
		{
			Kpp = parserData.Kpp;
			FullName = parserData.FullName;
			Ogrn = parserData.Ogrn;
		}
	}
}