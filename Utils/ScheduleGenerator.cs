using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorScheduleApp.Utils
{
    class ScheduleGenerator
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private List<Database.DoctorSchedule> _doctorSchedule;

        public ScheduleGenerator(DateTime startDate, DateTime endDate, List<Database.DoctorSchedule> schedule)
        {
            _startDate = startDate;
            _endDate = endDate;
            _doctorSchedule = schedule
                .Where(p => p.Date >= _startDate && p.Date <= _endDate.Date).ToList();
        }

        public List<Database.ScheduleHeader> GenerateHeaders()
        {
            var result = new List<Database.ScheduleHeader>();

            var startDate = _startDate;
            while (startDate.Date < _endDate.Date)
            {
                result.Add(new Database.ScheduleHeader
                {
                    Date = startDate.Date
                });

                startDate = startDate.AddDays(1);
            }
            return result;
        }


        public List<List<Database.ScheduleAppointment>> GenerateAppointments(List<Database.ScheduleHeader> headers)
        {
            var result = new List<List<Database.ScheduleAppointment>>();

            if (_doctorSchedule.Count() > 0)
            {
                var minStartTime = _doctorSchedule.Min(p => p.StartTime);
                var maxEndTime = _doctorSchedule.Max(p => p.EndTime);

                var startTime = minStartTime;
                while (startTime < maxEndTime)
                {
                    var appointmentsPerInterval = new List<Database.ScheduleAppointment>();


                    foreach (var header in headers)
                    {
                        var currentSchedule = _doctorSchedule.FirstOrDefault(p => p.Date == header.Date);

                        var scheduleAppointment = new Database.ScheduleAppointment
                        {
                            ScheduleId = currentSchedule?.Id ?? -1,
                            StartTime = startTime,
                            EndTime = startTime.Add(TimeSpan.FromMinutes(30))
                        };

                        if (currentSchedule != null)
                        {
                            var busyAppointment = currentSchedule.Appointments.FirstOrDefault(p => p.StartTime == startTime);

                            if (busyAppointment != null)
                            {
                                scheduleAppointment.AppointmentType = Database.AppointmentType.Busy;
                            }
                            else
                            {
                                if (startTime < currentSchedule.StartTime || startTime >= currentSchedule.EndTime)
                                {
                                    scheduleAppointment.AppointmentType = Database.AppointmentType.DayOff;
                                }
                                else
                                {
                                    scheduleAppointment.AppointmentType = Database.AppointmentType.Free;

                                }
                            }
                        }
                        else
                        {
                            scheduleAppointment.AppointmentType = Database.AppointmentType.DayOff;
                        }


                        appointmentsPerInterval.Add(scheduleAppointment);
                    }
                    result.Add(appointmentsPerInterval);
                    startTime = startTime.Add(TimeSpan.FromMinutes(30));
                }
            }
            return result;
        }
    }
}
