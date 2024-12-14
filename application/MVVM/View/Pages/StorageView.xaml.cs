using System.Windows;
using System.Windows.Controls;
using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StorageView : UserControl
{
    private readonly StorageViewModel _viewModel;

    public StorageView(StorageViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();

        LoadStorageData();
    }

    private async void LoadStorageData()
    {
        // Используйте ID сущности для загрузки данных
        Guid entityId = Guid.Parse("52d6177b-bac0-447d-9ff3-fdf10e344b6e");
        await _viewModel.LoadStorageAsync(entityId);

        // Обновляем StackPanel с карточками
        foreach (var storage in _viewModel.Storage)
        {
            var storageCard = CreateStorageCard(storage);
            StoragePanel.Children.Add(storageCard);
        }
    }

    // Метод для создания карточки данных хранилища
    private StackPanel CreateStorageCard(StorageDataResult storage)
    {
        var storageCard = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = new System.Windows.Thickness(0, 10, 0, 10),
            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray)
        };

        var id = new TextBlock { Text = $"{storage.Id}", Visibility = Visibility.Hidden};
        var pointText = new TextBlock { Text = $"Point: {storage.Point}", FontSize = 20 };
        var countryText = new TextBlock { Text = $"Country: {storage.Country}", FontSize = 16 };
        var cityText = new TextBlock { Text = $"City: {storage.City}", FontSize = 16 };
        var addressText = new TextBlock { Text = $"Address: {storage.Address}", FontSize = 14 };
        var indexText = new TextBlock { Text = $"Index: {storage.Index}", FontSize = 14 };

        storageCard.Children.Add(pointText);
        storageCard.Children.Add(countryText);
        storageCard.Children.Add(cityText);
        storageCard.Children.Add(addressText);
        storageCard.Children.Add(indexText);
        
        storageCard.MouseLeftButtonUp += (sender, e) =>
        {
	        _viewModel.TriggerShowStorage();
        };

        return storageCard;
    }
}
