using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentServise = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentServise = SelectedService;

            DataContext = _currentServise;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");
            if(_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (_currentServise.DiscountInt < 0 && _currentServise.DiscountInt >= 100)
                errors.AppendLine("Укажите скидку");
            if (_currentServise.DurationI > 240 || _currentServise.DurationI == 0)
                errors.AppendLine("Укажите длительность услуги до 240 минут и больше 0");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = gostenova_avtoserviceEntities1.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentServise.Title).ToList();
            
           
                var context = gostenova_avtoserviceEntities1.GetContext();
                bool duplicateExists = context.Service.Any(p => p.Title == _currentServise.Title && p.ID != _currentServise.ID);
                if (duplicateExists)
                {
                    MessageBox.Show("Такая услуга уже существует");
                    return;
                }
                if (_currentServise.ID == 0)
                {                   
                   context.Service.Add(_currentServise);
                }
                try
                {
                    gostenova_avtoserviceEntities1.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            
           
           
        }
    }
}
