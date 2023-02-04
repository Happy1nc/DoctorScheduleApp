using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace DoctorScheduleApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ComboSpecialization.ItemsSource = App.DataBase.Specializations.ToList();
            ComboSpecialization.SelectedIndex = 0;
            var dateNow = DateTime.Today;
            dpStartDate.SelectedDate = dateNow;
            dpEndDate.SelectedDate = dateNow.AddDays(5);

        }


        private void ComboSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpecialization = ComboSpecialization.SelectedItem as Database.Specialization;
            if (selectedSpecialization != null)
            {
                var doctors = App.DataBase.Doctors.ToList().Where(p => p.Specialization == selectedSpecialization);
                ComboDoctor.ItemsSource = doctors;
                ComboDoctor.SelectedIndex = 0;

            }
        }

        private void ComboDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void GenerateSchedule(Database.Doctor selectedDoctor)
        {
            var startDate = Convert.ToDateTime(dpStartDate.SelectedDate);
            var endDate = Convert.ToDateTime(dpEndDate.SelectedDate);
            var scheduleGenerator = new Utils.ScheduleGenerator(startDate, endDate, selectedDoctor.DoctorSchedules.ToList());

            var headers = scheduleGenerator.GenerateHeaders();
            var appointments = scheduleGenerator.GenerateAppointments(headers);
            if (dpStartDate.SelectedDate != null || dpEndDate.SelectedDate != null)
            {
                if (startDate > endDate)
                {
                    MessageBox.Show("Дата начала не может быть больше даты окончания - введите корректные границы выборки", "Внимание: совет!", MessageBoxButton.OK, MessageBoxImage.Information);
                    dpStartDate.SelectedDate = Convert.ToDateTime(DateTime.Now.Date);
                    dpEndDate.SelectedDate = Convert.ToDateTime(startDate.AddDays(5));
                }
                else if (endDate < Convert.ToDateTime(DateTime.Today) || startDate < Convert.ToDateTime(DateTime.Today))
                {
                    MessageBox.Show("Ошибка вывода записей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    dpStartDate.SelectedDate = Convert.ToDateTime(DateTime.Now.Date);
                    dpEndDate.SelectedDate = Convert.ToDateTime(startDate.AddDays(5));
                }
                else
                {
                    LoadSchedule(headers, appointments);
                }
            }
            else
            {
                LoadSchedule(headers, appointments);
            }

        }

        private void LoadSchedule(List<Database.ScheduleHeader> headers, List<List<Database.ScheduleAppointment>> appointments)
        {
            DGridSchedule.Columns.Clear();
            for (int i = 0; i < headers.Count(); i++)
            {
                FrameworkElementFactory headerFactory = new FrameworkElementFactory(typeof(UserControls.SheduleHeaderControl));
                headerFactory.SetValue(DataContextProperty, headers[i]);

                var headerTemplate = new DataTemplate
                {
                    VisualTree = headerFactory

                };
                FrameworkElementFactory cellFactory = new FrameworkElementFactory(typeof(UserControls.ScheduleAppointmentControl));
                cellFactory.SetBinding(DataContextProperty, new Binding($"[{i}]"));

                cellFactory.AddHandler(MouseDownEvent, new MouseButtonEventHandler(BtnAppointment_MouseDown), true);



                var cellTemplate = new DataTemplate
                {
                    VisualTree = cellFactory
                };


                var columnTemplate = new DataGridTemplateColumn
                {
                    HeaderTemplate = headerTemplate,
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    CellTemplate = cellTemplate

                };

                DGridSchedule.Columns.Add(columnTemplate);
            }

            DGridSchedule.ItemsSource = appointments;
        }

        private void BtnAppointment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var currentControl = sender as UserControls.ScheduleAppointmentControl;
            var currentAppointment = currentControl.DataContext as Database.ScheduleAppointment;

            if (currentAppointment != null && currentAppointment.ScheduleId > 0
                && currentAppointment.AppointmentType == Database.AppointmentType.Free)
            {
                App.DataBase.Appointments.Add(new Database.Appointment
                {
                    DoctorScheduleId = currentAppointment.ScheduleId,
                    StartTime = currentAppointment.StartTime,
                    EndTime = currentAppointment.EndTime,
                    ClientId = 1
                });

                App.DataBase.SaveChanges();
                Windows.Ticket ticket = new Windows.Ticket(ComboSpecialization.Text, ComboDoctor.Text, currentAppointment.StartTime, currentAppointment.EndTime);
                ticket.ShowDialog();

                MessageBox.Show("Вы записались на приём к врачу");

                ComboDoctor_SelectionChanged(ComboDoctor, null);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var queshion = MessageBox.Show("Напечатать расписание выбранного врача?", "Внимание: вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (queshion == MessageBoxResult.Yes)
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.PageRangeSelection = PageRangeSelection.UserPages;
                printDialog.PrintVisual(Data, "");
            }
            else
            {

            }
        }

        private void BtnFreeTime_Click(object sender, RoutedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            Windows.FreeTime freeTime = new Windows.FreeTime(selectedDoctor);
            freeTime.ShowDialog();
        }


        private void BtnTicketsList_Click(object sender, RoutedEventArgs e)
        {
            Windows.TicketsWindows tickets = new Windows.TicketsWindows();
            tickets.Show();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;

            GenerateSchedule(selectedDoctor);
        }

        private void dpEndDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void dpEndDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void dpStartDate_CalendarOpened(object sender, RoutedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void dpStartDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void dpStartDate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }

        private void dpEndDate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Database.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
            }
        }
    }
}
