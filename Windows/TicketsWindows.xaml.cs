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
    /// Логика взаимодействия для TicketsWindows.xaml
    /// </summary>
    public partial class TicketsWindows : Window
    {
        public TicketsWindows()
        {
            InitializeComponent();
            List();
            tbSearch.TextChanged += tbSearch_TextChanged;

        }
        private string _currentInput = "";
        private string _currentSearch = "";
        private string _currentText = "";

        private int _selectionStart;
        private int _selectionLenght;
        private static readonly string[] SearchValues = { "Гурьба", "Шурова", "Степченко", "Сафонова", "Медведева", "Романов", "Каур" };

        public void List()
        {
            Database.HospitalBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

            var data = Database.HospitalBaseEntities.GetContext();
            List<UserControls.Coupon> items = new List<UserControls.Coupon>();

            foreach (var appointment in data.Appointments.ToList())
            {
                lvTickets.Items.Add(new UserControls.Coupon(appointment));
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = tbSearch.Text;
            if (input.Length > _currentInput.Length && input != _currentSearch)
            {
                _currentSearch = SearchValues.FirstOrDefault(p => p.Contains(input));
                if (_currentSearch != null)
                {
                    _currentText = _currentSearch;
                    _selectionStart = input.Length;
                    _selectionLenght = _currentSearch.Length - input.Length;

                    tbSearch.Text = _currentText;
                    tbSearch.Select(_selectionStart, _selectionLenght);
                }
            }
            _currentInput = input;
            Searching(input);


        }
        private void Searching(string Doctor)
        {
            var data = Database.HospitalBaseEntities.GetContext();
            var search = from doctor in data.Appointments
                         where doctor.DoctorSchedule.Doctor.LastName.ToUpper().Contains(Doctor)
                         orderby doctor.DoctorSchedule.Doctor.LastName
                         select doctor;
            lvTickets.Items.Clear();
            foreach (Database.Appointment doc in search)
            {
                lvTickets.Items.Add(new UserControls.Coupon(doc));
            }

        }

        private void rBtnDesc_Click(object sender, RoutedEventArgs e)
        {
            var data = Database.HospitalBaseEntities.GetContext();
            var search = from doctor in data.Appointments

                         orderby doctor.Id descending
                         select doctor;
            lvTickets.Items.Clear();
            foreach (Database.Appointment doc in search)
            {
                lvTickets.Items.Add(new UserControls.Coupon(doc));
            }
        }

        private void rBtnAsc_Click(object sender, RoutedEventArgs e)
        {
            var data = Database.HospitalBaseEntities.GetContext();
            var search = from doctor in data.Appointments

                         orderby doctor.Id ascending
                         select doctor;
            lvTickets.Items.Clear();
            foreach (Database.Appointment doc in search)
            {
                lvTickets.Items.Add(new UserControls.Coupon(doc));
            }
        }
    }
}

