using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoctorScheduleApp.Classes
{
    class Autorisation
    {
        public static int UserId;
        public static int ClientId;


        public static bool SignIn(string Login, string Password)
        {
            var db = Database.HospitalBaseEntities.GetContext();
            var sign = from user in db.Clients
                       where user.Login == Login
                       where user.Password == Password
                       select user;

            var userId = from user in db.Clients
                         where user.Login == Login && user.Password == Password
                         select user.Id;

            if (sign == null || !sign.Any())
            {
                Login = "";
                Password = "";
                MessageBox.Show("Неверный логин или пароль", "Внимание!", MessageBoxButton.OK);
                return false;
            }
            else
            {
                Login = "";
                Password = "";
                MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK);
                MainWindow windowApp = new MainWindow();
                windowApp.Show();
                return true;
            }
        }

    }
}
