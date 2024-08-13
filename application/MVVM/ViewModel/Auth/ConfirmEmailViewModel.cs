using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class ConfirmEmailViewModel : ObservableObject
{
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

	partial void OnInput_1Changed(string value) => CreateModel();
	partial void OnInput_2Changed(string value) => CreateModel();
	partial void OnInput_3Changed(string value) => CreateModel();
	partial void OnInput_4Changed(string value) => CreateModel();
	partial void OnInput_5Changed(string value) => CreateModel();
	partial void OnInput_6Changed(string value) => CreateModel();

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
