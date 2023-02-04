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
    /// Логика взаимодействия для Ticket.xaml
    /// </summary>
    public partial class Ticket : Window
    {
        public Ticket(string Specialization, string Name, TimeSpan startTime, TimeSpan endTime)
        {
            InitializeComponent();

            TbSpecialization.Text = Specialization;
            TbName.Text = Name;
            TbTime.Text = $"{startTime.ToString(@"hh\:mm")} - {endTime.ToString(@"hh\:mm")}";
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var queshion = MessageBox.Show("Распечатать расписание врача?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (queshion == MessageBoxResult.Yes)
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.PageRangeSelection = PageRangeSelection.UserPages;
                printDialog.PrintVisual(spTicket, "");
            }
            else
            {
            }
        }
    }
}
