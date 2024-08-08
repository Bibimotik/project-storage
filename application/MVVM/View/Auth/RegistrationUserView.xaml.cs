using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace application.MVVM.View.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegistrationUserView.xaml
    /// </summary>
    public partial class RegistrationUserView : UserControl
    {
        public RegistrationUserView()
        {
            InitializeComponent();
        }
        
        private void Password_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex inputRegex = new Regex(@"^[a-zA-Z0-9\s!.,/|<>{}`~;№%:?*()-=_+@#$^&]*$");
		
            Match match = inputRegex.Match(e.Text);
            if (!match.Success) 
            {
                e.Handled = true;
            }
        }
        
        private void Password_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        
        private void CheckBox_Click1(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                passwordTextBox.Text = passwordPasswordBox.Password;
                passwordTextBox.Visibility = Visibility.Visible;
                passwordPasswordBox.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordPasswordBox.Password = passwordTextBox.Text; 
                passwordTextBox.Visibility = Visibility.Hidden;
                passwordPasswordBox.Visibility = Visibility.Visible;
            }
        }
        
        private void CheckBox_Click2(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                confirmPasswordTextBox.Text = confirmPasswordPasswordBox.Password;
                confirmPasswordTextBox.Visibility = Visibility.Visible;
                confirmPasswordPasswordBox.Visibility = Visibility.Hidden;
            }
            else
            {
                confirmPasswordPasswordBox.Password = confirmPasswordTextBox.Text; 
                confirmPasswordTextBox.Visibility = Visibility.Hidden;
                confirmPasswordPasswordBox.Visibility = Visibility.Visible;
            }
        }
    }
}
