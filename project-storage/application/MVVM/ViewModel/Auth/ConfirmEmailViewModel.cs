using System.Diagnostics;

using application.MVVM.Model;
using application.Services;

using CommunityToolkit.Mvvm.ComponentModel;

using CSharpFunctionalExtensions;

namespace application.MVVM.ViewModel.Auth;

partial class ConfirmEmailViewModel : ObservableObject
{
    private readonly bool _isInitializing = false;

    private const int CODE_LENGTH = 6;

    private string _code = string.Empty;

    [ObservableProperty]
    private string input_1 = string.Empty;
    [ObservableProperty]
    private string input_2 = string.Empty;
    [ObservableProperty]
    private string input_3 = string.Empty;
    [ObservableProperty]
    private string input_4 = string.Empty;
    [ObservableProperty]
    private string input_5 = string.Empty;
    [ObservableProperty]
    private string input_6 = string.Empty;
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private bool isCodeInvalid = false;
    [ObservableProperty]
    private bool isConfirmCodeInvalid = false;

    public ConfirmEmailViewModel()
    {
	    EntityModel.Model ??= new EntityModel();
	    EntityModel model = EntityModel.Model;

	    if (model.Email.Length > 20)
	    {
		    string dots = new string('.', 5);
		    Email = $"Code for {model.Email.Substring(0, 10)}{dots}{model.Email.Substring(model.Email.Length - 10)}";
	    }
	    else
	    {
		    Email = "Code for " + model.Email;
	    }
    }

    partial void OnInput_1Changed(string value) => ValidateAllInputs();
	partial void OnInput_2Changed(string value) => ValidateAllInputs();
	partial void OnInput_3Changed(string value) => ValidateAllInputs();
	partial void OnInput_4Changed(string value) => ValidateAllInputs();
	partial void OnInput_5Changed(string value) => ValidateAllInputs();
	partial void OnInput_6Changed(string value) => ValidateAllInputs();

	private void ValidateAllInputs()
	{
	    IsCodeInvalid = ValidateAndCreateModel(string.Empty);

	    if (!IsCodeInvalid)
	    {
	        IsConfirmCodeInvalid = !MatchCode();
	    }
	    else
	    {
	        IsConfirmCodeInvalid = false;
	    }
	}
	// TODO - не трогай пока тут не гамарджоба но я исправлю :(
	public bool MatchCode()
	{
	    _code = string.Concat(Input_1, Input_2, Input_3, Input_4, Input_5, Input_6);
	    
	    EntityModel model = EntityModel.Model;
	    return model.InputCode == _code;
	}

	private bool ValidateAndCreateModel(string? value)
	{
	    if (_isInitializing)
	        return false;

	    bool areAllInputsFilled = !string.IsNullOrWhiteSpace(Input_1) &&
	                              !string.IsNullOrWhiteSpace(Input_2) &&
	                              !string.IsNullOrWhiteSpace(Input_3) &&
	                              !string.IsNullOrWhiteSpace(Input_4) &&
	                              !string.IsNullOrWhiteSpace(Input_5) &&
	                              !string.IsNullOrWhiteSpace(Input_6);

	    if (!areAllInputsFilled)
	    {
	        IsCodeInvalid = true;
	        return true;
	    }
	    
	    CreateModel();
	    return false;
	}

	private void CreateModel()
	{
	    _code = string.Concat(Input_1, Input_2, Input_3, Input_4, Input_5, Input_6);
	    Debug.WriteLine("CODE:\t" + _code);

	    if (_code.Length != CODE_LENGTH)
	        return;

	    EntityModel.Model ??= new EntityModel();

	    EntityModel model = EntityModel.Model;
	    model.InputCode = _code;
	}
}
