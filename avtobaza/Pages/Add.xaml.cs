using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
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
using avtobaza.Model;
using HashPasswords;

namespace avtobaza.Pages
{
    /// <summary>
    /// класс страницы добавления нового сотрудника в базу
    /// </summary>
    public partial class Add : Page
    {
        public Add()
        {
            InitializeComponent();
        }
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            using (var context = new avtobazaEntities1())
            {
                // получаем введенные данные
                string firstName = txtImya.Text;
                string lastName = txtFamil.Text;
                string surname = txtOtchestvo.Text;
                string password = txtPassw.Text;
                long phoneNumber;

                // проверка корректности введеных данных
                if (!long.TryParse(txtNomer.Text/*данные на вход*/, out phoneNumber /*выходные данные*/) || txtNomer.Text.Length != 11)
                {
                    MessageBox.Show("Некорректный ввод номера телефона");
                    return;
                }

                long passport;
                if (!long.TryParse(txtNomerPassport.Text, out passport) || txtNomerPassport.Text.Length != 10)
                {
                    MessageBox.Show("Некорректный ввод паспорта");
                    return;
                }

                long driverLicense;
                if (!long.TryParse(txtPrava.Text, out driverLicense) || txtPrava.Text.Length != 10)
                {
                    MessageBox.Show("Некорректный ввод водительских прав");
                    return;
                }

                // хэширование пароля
                PasswordHasher hasher = new PasswordHasher();
                string hashedPassword = hasher.HashPassword(password);

                Users newUser = new Users
                {
                    Login = txtLog.Text,
                    Parol = hashedPassword,
                };
                context.Users.Add(newUser);

                int roles = 0;
                if (cmbRol.Text == "Администратор")
                {
                    roles = 3;
                }
                if (cmbRol.Text == "Диспетчер")
                {
                    roles = 1;
                }
                if (cmbRol.Text == "Водитель")
                {
                    roles = 2;
                }

                Sotrudniki newSotrudnik = new Sotrudniki
                {
                    Imya = firstName,
                    Familiya = lastName,
                    Otchestvo = surname,
                    NomerTel = phoneNumber,
                    Passport = passport,
                    VoditelPrava = driverLicense,
                    IDUsera = newUser.IDUsera,
                    IDDolzhnosti = roles,
                };
                context.Sotrudniki.Add(newSotrudnik);

                // валидация данных
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(newSotrudnik);

                if (Validator.TryValidateObject(newSotrudnik, validationContext, validationResults, true))
                {
                    // при успешной валидации, сохранение в базу
                    context.Sotrudniki.Add(newSotrudnik);

                    try
                    {
                        context.SaveChanges();
                        ClearInputFields();
                        cmbRol.SelectedValue = roles;
                        MessageBox.Show("Пользователь успешно добавлен!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}");
                    }
                }
                else
                {
                    // если валидация не прошла, выводим сообщения об ошибках
                    foreach (var validationResult in validationResults)
                    {
                        MessageBox.Show(validationResult.ErrorMessage);
                    }
                }
            }
        }
        private void ClearInputFields()
        {
            txtFamil.Clear();
            txtImya.Clear();
            txtOtchestvo.Clear();
            txtNomer.Clear();
            txtNomerPassport.Clear();
            txtPrava.Clear();
            txtLog.Clear();
            txtPassw.Clear();
        }
    }
}
