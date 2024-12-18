using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

using Microsoft.Win32;

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
		var orders = await _saleRepository.GetOrdersByEntityIdAsync(entityId, searchQuery, _orderBy);
		Orders.Clear();

		foreach (var order in orders)
		{
			Orders.Add(order);
		}
	}
	
	[RelayCommand]
	public async Task DeleteStorage(Guid orderId)
	{
		var isDeleted = await _saleRepository.MarkSaleAsDeletedAsync(orderId);
		if (isDeleted)
		{
			await LoadOrdersAsync(EntityModel.OurUserModel.EntityId);
		}
	}
	
	[RelayCommand]
	public async Task OnSortChanged(string sortOption)
	{
		if (sortOption == "По дате от старой к новой")
		{
			_orderBy = "ASC";
		}
		else if (sortOption == "По дате от новой к старой")
		{
			_orderBy = "DESC";
		}
		else
		{
			_orderBy = "";
		}

		await LoadOrdersAsync(EntityModel.OurUserModel.EntityId);
	}
	
	[RelayCommand]
	public async Task SelectAndProcessDocx(Guid orderId)
	{
		var openFileDialog = new OpenFileDialog
		{
			Filter = "Word Documents (*.docx)|*.docx",
			Title = "Выберите шаблон .docx"
		};

		if (openFileDialog.ShowDialog() == true)
		{
			string selectedFilePath = openFileDialog.FileName;

			try
			{
				var orders = await _saleRepository.GetOrdersByOrderIdAsync(orderId);
				var firstOrder = orders.FirstOrDefault();

				if (firstOrder != null)
				{
					await RunPythonScript(selectedFilePath, firstOrder);
				}
				else
				{
					MessageBox.Show("Заказы не найдены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при обработке документа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
	
	private async Task RunPythonScript(string templateDocxPath, OrderModel order)
	{
	    try
	    {
	        string pythonExecutable = "python";
	        string pythonScriptPath = @"..\..\..\..\parser\docxFile\main.py";

	        string arguments = string.Join(" ", new[]
	        {
		        $"\"{templateDocxPath}\"",
		        $"\"{order.Plan_Date_Shipment.ToString("dd MMMM yyyy")}\"",
		        $"\"{order.FullName}\"",
		        $"\"{order.Comment}\"",
		        $"\"{order.FullName}\"",
		        $"\"{order.Shipping_Address}\"",
		        $"\"{order.INN}\"",
		        $"\"{order.OGRN}\"",
		        $"\"{order.FromCor_Account}\"",
		        $"\"{order.FromCor_Account}\"",
		        $"\"{order.FromBIK}\"",
		        $"\"{order.FromBank}\"",
		        $"\"{order.Delivery_Point}\"",
		        $"\"{order.INN}\"",
		        $"\"{order.OGRN}\"",
		        $"\"{order.ToCor_Account}\"",
		        $"\"{order.ToCor_Account}\"",
		        $"\"{order.ToBIK}\"",
		        $"\"{order.ToBank}\"",
		        $"\"{order.KPP}\"",
		        $"\"{order.KPP}\"",
		        $"\"{order.Delivery_Address}\""
	        });

	        var processStartInfo = new ProcessStartInfo
	        {
	            FileName = pythonExecutable,
	            Arguments = $"\"{pythonScriptPath}\" {arguments}",
	            RedirectStandardOutput = true,
	            RedirectStandardError = true,
	            UseShellExecute = false,
	            CreateNoWindow = true
	        };

	        using var process = Process.Start(processStartInfo);
	        if (process == null)
	        {
	            throw new InvalidOperationException("Не удалось запустить Python-скрипт.");
	        }

	        string output = await process.StandardOutput.ReadToEndAsync();
	        string errors = await process.StandardError.ReadToEndAsync();

	        process.WaitForExit();

	        if (!string.IsNullOrEmpty(errors))
	        {
	            MessageBox.Show($"Ошибка при выполнении скрипта:\n{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
	        }
	        else
	        {
	            MessageBox.Show("Документ успешно сгенерирован и сохранён в папке Загрузки.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
	        }
	    }
	    catch (Exception ex)
	    {
	        MessageBox.Show($"Ошибка при запуске скрипта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
	    }
	}
}