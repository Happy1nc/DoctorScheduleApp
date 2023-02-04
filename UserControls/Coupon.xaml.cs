﻿using DoctorScheduleApp.Database;
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
    /// Логика взаимодействия для самого Талона
    /// </summary>
    public partial class Coupon : UserControl
    {
        private static Appointment _appointment = new Appointment();
        public Coupon(Appointment appointment)
        {
            InitializeComponent();
            _appointment = appointment;
            DataContext = _appointment;
            TbDate.Text = appointment.DoctorSchedule.Date.ToString("d MMMM yyyy");
            TbStart.Text = appointment.StartTime.ToString(@"hh\:mm");
            TbEnd.Text = appointment.EndTime.ToString(@"hh\:mm");

        }
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.UserPages;
            printDialog.PrintVisual(spTicket, "");
        }
    }
}
