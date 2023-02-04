using DoctorScheduleApp.Database;
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
    /// Логика взаимодействия для FreeTime.xaml
    /// </summary>
    public partial class FreeTime : Window
    {
        public FreeTime()
        {
            InitializeComponent();
        }
        private Database.Doctor selectedDoctor;

        public FreeTime(Database.Doctor selectedDoctor)
        {
            InitializeComponent();
            this.selectedDoctor = selectedDoctor;
            TbDoctor.Text = selectedDoctor.LastName;
            TbSpecialization.Text = selectedDoctor.Specialization.Name;
            GenerateSchedule(selectedDoctor);
        }
        private void GenerateSchedule(Doctor selectedDoctor)
        {
            var startDate = DateTime.Now.Date;
            startDate = new DateTime(2023, 01, 31).Date;
            var endDate = startDate.AddDays(5);

            var headers = new Utils.ScheduleGenerator(startDate, endDate, selectedDoctor.DoctorSchedules.ToList()).GenerateHeaders();
            var schedules = new Utils.ScheduleGenerator(startDate, endDate, selectedDoctor.DoctorSchedules.ToList()).GenerateAppointments(headers);
            LoadSchedule(headers, schedules);
        }
        private void LoadSchedule(List<Database.ScheduleHeader> headers, List<List<Database.ScheduleAppointment>> scheduleAppointments)
        {
            TbTime.Text = "";

            for (int i = 0; i < headers.Count(); i++)
            {
                String result = $"{headers[i].Date.ToString("ddd (dd.MM.yyyy): \n")}";
                TimeSpan start = default;
                TimeSpan end = default;

                for (int j = 0; j < scheduleAppointments.Count; j++)
                {
                    if (scheduleAppointments[j][i].AppointmentType != Database.AppointmentType.Free)
                    {
                        if (start != default)
                        {
                            String startTime = start.ToString(@"hh\:mm");
                            String endTime = end.ToString(@"hh\:mm");

                            result += $"\t {startTime} - {endTime} \n";
                            start = default;
                            end = default;
                        }
                        continue;
                    }
                    else
                    {
                        if (start == default)
                        {
                            start = scheduleAppointments[j][i].StartTime;
                            end = scheduleAppointments[j][i].EndTime;
                        }
                        else
                        {
                            end = scheduleAppointments[j][i].EndTime;
                        }
                    }
                }
                if (result.Length > $"{headers[i].Date.ToString("ddd (dd.MM.yyyy): \n")}".Length)
                {
                    TbTime.Text += result;
                }
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.UserPages;
            printDialog.PrintVisual(StackPanel, "");
        }
    }
}
