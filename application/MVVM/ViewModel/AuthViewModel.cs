using System.Windows.Input;

using application.Utilities;

namespace application.MVVM.ViewModel
{
	public class AuthViewModel : BaseViewModel
	{
		private bool _authTypeLogin { get; set; }
		public bool AuthTypeLogin
		{
			get { return _authTypeLogin; }
			set { _authTypeLogin = value; OnPropertyChanged(nameof(AuthTypeLogin)); }
		}
		private bool _authTypeSignup { get; set; }
		public bool AuthTypeSignup
		{
			get { return _authTypeSignup; }
			set { _authTypeSignup = value; OnPropertyChanged(nameof(AuthTypeSignup)); }
		}

		public ICommand SwithToLoginCommand { get; set; }
		public ICommand SwithToSignUpCommand { get; set; }

		public AuthViewModel()
		{
			AuthTypeLogin = true;
			AuthTypeSignup = false;

			SwithToLoginCommand = new RelayCommand(ChangeToLogIn);
			SwithToSignUpCommand = new RelayCommand(ChangeToSignUp);
		}
		
		private void ChangeToLogIn(object obj)
		{
			AuthTypeLogin = true;
			AuthTypeSignup = false;
		}

		private void ChangeToSignUp(object obj)
		{
			AuthTypeLogin = false;
			AuthTypeSignup = true;
		}
	}
}