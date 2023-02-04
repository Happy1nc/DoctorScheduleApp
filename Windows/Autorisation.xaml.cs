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
using System.Windows.Shapes;

namespace DoctorScheduleApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для Autorisation.xaml
    /// </summary>
    public partial class Autorisation : Window
    {
        public Autorisation()
        {
            InitializeComponent();
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы запустили программу для записи на прием к врачу.\n Войдите в систему, введя логин и пароль из БД," +
                " после входа вы сможете выбрать специализацию, врача и время, на которое вы будете записаны", "Справка");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VisiblePass_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            tbPassword.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Visible;

        }

        private void VisiblePass_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tbPassword.Text = pbPassword.Password;
            pbPassword.Visibility = Visibility.Collapsed;
            tbPassword.Visibility = Visibility.Visible;
        }

        private void BtnSign_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Autorisation.SignIn(tbLogin.Text, pbPassword.Password))
            {
                Close();
            }
            else
            {
                pbPassword.Clear();
            }

        }
    }
}
