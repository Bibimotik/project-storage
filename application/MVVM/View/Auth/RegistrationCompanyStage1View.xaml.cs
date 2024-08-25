using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;

using application.MVVM.Model;
using application.MVVM.ViewModel;

namespace application.MVVM.View.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegistrationStage1.xaml
    /// </summary>
    public partial class RegistrationCompanyStage1View : UserControl
    {
        public RegistrationCompanyStage1View()
        {
            InitializeComponent();
        }
    }
}
