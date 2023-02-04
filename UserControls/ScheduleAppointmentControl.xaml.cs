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

namespace DoctorScheduleApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ScheduleAppointmentControl.xaml
    /// </summary>
    public partial class ScheduleAppointmentControl : UserControl
    {
        public ScheduleAppointmentControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Database.ScheduleAppointment currentAppointment)
            {
                BtnAppointment.Content = $"{currentAppointment.StartTime.ToString(@"hh\:mm")} " +
                    $"- {currentAppointment.EndTime.ToString(@"hh\:mm")}";

                switch (currentAppointment.AppointmentType)
                {
                    case Database.AppointmentType.Free:
                        {
                            BtnAppointment.IsEnabled = true;
                            BtnAppointment.Foreground = new SolidColorBrush(Colors.White);
                            BtnAppointment.Visibility = Visibility.Visible;
                        }
                        break;

                    case Database.AppointmentType.Busy:
                        {
                            BtnAppointment.IsEnabled = false;
                            BtnAppointment.Foreground = new SolidColorBrush(Colors.Gray);
                            BtnAppointment.Visibility = Visibility.Visible;
                        }
                        break;

                    case Database.AppointmentType.DayOff:
                        {
                            BtnAppointment.Visibility = Visibility.Hidden;
                        }
                        break;
                }
            }
        }
    }
}
