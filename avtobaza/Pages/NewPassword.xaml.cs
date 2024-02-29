using HashPasswords;
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

namespace avtobaza.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewPassword.xaml
    /// </summary>
    public partial class NewPassword : Page
    {
        private string loginOrEmail;

        public NewPassword(string loginOrEmail)
        {
            InitializeComponent();
            this.loginOrEmail = loginOrEmail;
        }

        private void btnSaveNewPassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password.Trim();
            string repeatPassword = txtRepeatPassword.Password.Trim();

            if (newPassword == repeatPassword)
            {
                PasswordHasher hasher = new PasswordHasher();
                string hashedPassword = hasher.HashPassword(newPassword);

                using (var context = Helper.getContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Login == loginOrEmail);

                    if (user != null)
                    {
                        user.Parol = hashedPassword;
                        context.SaveChanges();
                        MessageBox.Show("Новый пароль успешно сохранен.");
                        NavigationService.Navigate(new Autho()); // Переход на страницу авторизации
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают, повторите ввод.");
            }
        }


    }
}
