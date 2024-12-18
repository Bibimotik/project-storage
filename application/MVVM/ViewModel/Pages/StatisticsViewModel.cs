using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Linq;
using System.Threading.Tasks;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using LiveChartsCore.SkiaSharpView.Painting;

namespace application.MVVM.ViewModel.Pages
{
    public partial class StatisticsViewModel : ObservableObject
    {
        private readonly IStatisticsRepository _statisticsRepository;

        [ObservableProperty]
        private DateTime? startDate;

        [ObservableProperty]
        private DateTime? endDate;

        [ObservableProperty]
        private string validationMessage;

        [ObservableProperty]
        private ISeries[] series;

        [ObservableProperty]
        private Axis[] xAxes;

        [ObservableProperty]
        private ISeries[] categorySeries;

        [ObservableProperty]
        private ISeries[] percentageSeries;

        public StatisticsViewModel(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;

            StartDate = new DateTime(2000, 1, 1);
            EndDate = DateTime.UtcNow;

            XAxes = new[] 
            {
                new Axis 
                {
                    LabelsRotation = 15,
                    Labels = Array.Empty<string>()
                }
            };

            LoadStatisticsAsync().ConfigureAwait(false);
        }

        [RelayCommand]
	private async Task LoadStatisticsAsync()
	{
	    if (!ValidateDates())
	    {
	        return;
	    }

	    try
	    {
	        var orderResults = await _statisticsRepository.GetOrdersByDateAsync(
	            EntityModel.OurUserModel.EntityId,
	            startDate.Value,
	            endDate.Value
	        );

	        var orderLabels = orderResults.Select(r => r.Date.ToString("yyyy-MM-dd")).ToArray();
	        var orderValues = orderResults.Select(r => (double)r.Count).ToArray();

	        Series = new ISeries[]
	        {
	            new ColumnSeries<double>
	            {
	                Values = orderValues,
	                Name = "Orders"
	            }
	        };

	        XAxes = new[]
	        {
	            new Axis
	            {
	                LabelsRotation = 15,
	                Labels = orderLabels
	            }
	        };

	        // Пироговый график
	        var categoryResults = await _statisticsRepository.GetProductSalesByCategoryAsync(
		        EntityModel.OurUserModel.EntityId,
		        startDate.Value,
		        endDate.Value
	        );

	        var random = new Random();
	        CategorySeries = categoryResults.Select(result => new PieSeries<double>
	        {
		        Values = new[] { (double)result.TotalCount },
		        Name = result.Product,
		        DataLabelsFormatter = point => $"{result.Product}",
		        Fill = new SolidColorPaint(new SkiaSharp.SKColor(
			        (byte)random.Next(256), 
			        (byte)random.Next(256),
			        (byte)random.Next(256)))
	        }).ToArray();

	        // Точечный график
	        var percentageResults = await _statisticsRepository.GetProductSalesPercentageAsync(
	            EntityModel.OurUserModel.EntityId,
	            startDate.Value,
	            endDate.Value
	        );

	        PercentageSeries = new ISeries[]
	        {
	            new ScatterSeries<double>
	            {
	                Values = percentageResults.Select(r => r.Percentage).ToArray(),
	                Name = "Product Sales Percentage"
	            }
	        };
	    }
	    catch (Exception ex)
	    {
	        Series = Array.Empty<ISeries>();
	        XAxes = Array.Empty<Axis>();
	        CategorySeries = Array.Empty<ISeries>();
	        PercentageSeries = Array.Empty<ISeries>();
	        Console.WriteLine($"Error loading statistics: {ex.Message}");
	    }
	}

        private bool ValidateDates()
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ValidationMessage = "Both start and end dates are required.";
                return false;
            }

            if (startDate.Value > endDate.Value)
            {
                ValidationMessage = "Start date cannot be later than end date.";
                return false;
            }

            if (endDate.Value < startDate.Value)
            {
                ValidationMessage = "End date cannot be earlier than start date.";
                return false;
            }

            ValidationMessage = string.Empty;
            return true;
        }
    }
}
