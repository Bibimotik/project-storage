using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

namespace application.MVVM.ViewModel.Pages;

public partial class SupportViewModel : ObservableObject
{
	public ICommand SelectFileCommand { get; }
	private readonly bool _isInitializing = false;
	private readonly Dictionary<string, Action<string?>> _validationActions;
	
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string message = string.Empty;
	
	[ObservableProperty]
	private bool isInvalidEmail = false;
	[ObservableProperty]
	private bool isInvalidMessage = false;
	
	[ObservableProperty]
	private List<string> selectedFileNames = new();
	
	public SupportViewModel()
	{
		SelectFileCommand = new RelayCommand(SelectFile);
		
		_isInitializing = true;

		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(SupportModel.Email), value => IsInvalidEmail = ValidateAndCreateModel(value) },
			{ nameof(SupportModel.Message), value => IsInvalidMessage = ValidateAndCreateModel(value) }
		};
		
		SupportModel.Model ??= new SupportModel();

		SupportModel model = SupportModel.Model;
		
		Email = model.Email;
		Message = model.Message;
		
		_isInitializing = false;
	}
	
	partial void OnEmailChanged(string value)
	{
		try
		{
			var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			if (!regex.IsMatch(value))
			{
				IsInvalidEmail = true;
				return;
			}

			IsInvalidEmail = false;
		}
		catch (RegexMatchTimeoutException)
		{
			IsInvalidEmail = false;
			return;
		}
		
		IsInvalidEmail = ValidateAndCreateModel(value);
	}
	
	partial void OnMessageChanged(string value) => IsInvalidMessage = ValidateAndCreateModel(value);
	
	private void SelectFile()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*";
		openFileDialog.Multiselect = true;

		if (openFileDialog.ShowDialog() == true)
		{
			SelectedFileNames.Clear();

			foreach (string fileName in openFileDialog.FileNames)
			{
				SelectedFileNames.Add(fileName);
			}
		}
		
		CreateModel();
	}
	
	private bool ValidateAndCreateModel(string? value)
	{
		if (_isInitializing)
			return false;

		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}
	
	private void OnInvalided(string property)
	{
		if (_validationActions.TryGetValue(property, out var validate))
		{
			validate(string.Empty);
		}
	}
	
	private void CreateModel()
	{
		SupportModel.Model ??= new SupportModel();

		SupportModel model = SupportModel.Model;
		model.Email = Email;
		model.Message = Message;
		model.Screenshots = SelectedFileNames
			.Select(fileName => File.ReadAllBytes(fileName))
			.ToList();
	}
}