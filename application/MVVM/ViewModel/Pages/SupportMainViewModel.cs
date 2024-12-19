using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class SupportMainViewModel : ObservableObject
{
    private readonly ISupportRepository _supportRepository;
    private readonly Dictionary<string, Action<string?>> _validationActions;
    private readonly bool _isInitializing;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string message;
    
    [ObservableProperty]
    private string guest;

    [ObservableProperty]
    private List<string> selectedFileNames = new();

    [ObservableProperty]
    private bool isInvalidEmail = false;

    [ObservableProperty]
    private bool isInvalidMessage = false;

    [ObservableProperty]
    private byte[] image;

    public ICommand SelectFileCommand { get; }

    public SupportMainViewModel(ISupportRepository supportRepository)
    {
        _supportRepository = supportRepository;
        SelectFileCommand = new RelayCommand(SelectFile);

        _isInitializing = true;

        AuthViewModel.Invalided += OnInvalided;

        _validationActions = new Dictionary<string, Action<string?>>
        {
            { nameof(Email), value => IsInvalidEmail = ValidateAndUpdateModel(value) },
            { nameof(Message), value => IsInvalidMessage = ValidateAndUpdateModel(value) }
        };

        EntityModel.Model ??= new EntityModel();

        Email = EntityModel.Model.Email;
        Message = EntityModel.Model.Message;

        _isInitializing = false;
    }

    partial void OnEmailChanged(string value)
    {
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        if (!regex.IsMatch(value))
        {
            IsInvalidEmail = true;
            Debug.WriteLine("Invalid email format.");
            return;
        }

        IsInvalidEmail = ValidateAndUpdateModel(value);
    }

    partial void OnMessageChanged(string value) => IsInvalidMessage = ValidateAndUpdateModel(value);

    [ObservableProperty]
    private string selectedFilePath = "Select File";

    private void SelectFile()
    {
	    var openFileDialog = new OpenFileDialog
	    {
		    Filter = "Logo Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
		    Multiselect = true
	    };

	    if (openFileDialog.ShowDialog() == true)
	    {
		    SelectedFileNames.Clear();
		    SelectedFileNames.AddRange(openFileDialog.FileNames);

		    if (SelectedFileNames.Any())
		    {
			    try
			    {
				    Image = ImageHelper.ConvertImageToByteArray(SelectedFileNames.First());
				    SelectedFilePath = SelectedFileNames.First();
			    }
			    catch (Exception ex)
			    {
				    MessageBox.Show($"Error loading image: {ex.Message}");
				    SelectedFilePath = "Error loading file";
			    }
		    }
	    }
	    else
	    {
		    SelectedFilePath = "Select File";
	    }
    }

    private bool ValidateAndUpdateModel(string? value)
    {
        if (_isInitializing)
            return false;

        return string.IsNullOrWhiteSpace(value);
    }

    private void OnInvalided(string property)
    {
        if (_validationActions.TryGetValue(property, out var validate))
        {
            validate(string.Empty);
        }
    }

    [RelayCommand]
    public async Task SendToSupportAsync()
    {
        try
        {
	        SupportModel supportModel = new(
		        Guid.NewGuid(),
		        EntityModel.OurUserModel.EntityId!,
		        null,
		        message,
		        image
	        );
	        
	        await _supportRepository.SentToSupport(supportModel);

            MessageBox.Show($"Запрос отправлен успешно. ID: {supportModel.Id}");
            ClearFields();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }

    private void ClearFields()
    {
        Email = string.Empty;
        Message = string.Empty;
        SelectedFileNames.Clear();
        Image = Array.Empty<byte>();
    }
}
