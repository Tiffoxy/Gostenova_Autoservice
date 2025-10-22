using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Gostenova_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectesService)
        {
            InitializeComponent();
            if(SelectesService != null)
                this._currentService = SelectesService;
            DataContext = _currentService;

            var _currentClient = gostenova_avtoserviceEntities1.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }
        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if(StarDate.Text =="")
                errors.AppendLine("Укажите дату услуги");
            if(TBStart.Text =="")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StarDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                gostenova_avtoserviceEntities1.GetContext().ClientService.Add(_currentClientService);

            try
            {
                gostenova_avtoserviceEntities1.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            string[] start = s.Split(':');
            if (start.Length != 2)
            {
                TBEnd.Text = "Ошибка в формате времени";
                return;
            }

            if (s.Length != 5 || s[2] != ':')
            {
                TBEnd.Text = "Ошибка в формате времени.(Пиши часы и минуты через :)";
                return;
            }
            if (!int.TryParse(start[0], out int startHour) || !int.TryParse(start[1], out int startMin))
            {
                TBEnd.Text = "Ошибка в формате времени";
                return;
            }
            if (startHour < 0 || startHour > 23 || startMin < 0 || startMin > 59)
            {
                TBEnd.Text = "Ошибка в формате времени";
                return;
            }

            int sum = startHour * 60 + startMin + _currentService.DurationI;
            int endHour = sum / 60;
            int endMin = sum % 60;
            endHour = endHour % 24;
            s = endHour.ToString() + ":" + endMin.ToString();
            TBEnd.Text = s;
        }
    
    }
}
