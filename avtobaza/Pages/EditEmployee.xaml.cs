using System;
using System.Collections.Generic;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using avtobaza.Model;
using HashPasswords;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;

namespace avtobaza.Pages
{
    /// <summary>
    /// класс страницы редактирования данных сотрудника 
    /// </summary>
    public partial class EditEmployee : Page
    {
        private SotrudInfo sotrudInfo;
        ListView card;
        public EditEmployee(SotrudInfo selectedItem, ListView Card)
        {
            InitializeComponent();
            sotrudInfo = selectedItem;
            card = Card;
        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // заполнение полей данными
            if (sotrudInfo != null)
            {
                txtLastName.Text = sotrudInfo.LastName;
                txtFirstName.Text = sotrudInfo.FirstName;
                txtPatronymic.Text = sotrudInfo.Patronymic;
                txtPhoneNumber.Text = sotrudInfo.UserPhone.ToString();
                txtPassport.Text = sotrudInfo.UserPassport.ToString();
                txtPrava.Text = sotrudInfo.Prava.ToString();
                txtLogin.Text = sotrudInfo.Login;
                cmbRol.Text = sotrudInfo.UserRol;
            }
        }
        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.getContext();

            // получение сущностей из базы
            var sotr = db.Sotrudniki.FirstOrDefault(s => s.IDSotrudnika == sotrudInfo.IDSotrudnika);
            var user = db.Users.FirstOrDefault(u => u.IDUsera == sotrudInfo.IDUsera);
            int roles = 0;

            if (sotr != null && user != null)
            {
                sotr.Imya = txtFirstName.Text;
                sotr.Familiya = txtLastName.Text;
                sotr.Otchestvo = txtPatronymic.Text;
                sotr.NomerTel = decimal.Parse(txtPhoneNumber.Text);
                sotr.Passport = decimal.Parse(txtPassport.Text);

                // проверка на успешное преобразование строки в число
                if (decimal.TryParse(txtPrava.Text, out decimal voditelPrava))
                {
                    sotr.VoditelPrava = voditelPrava;
                }

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
                sotr.IDDolzhnosti = roles;
                user.Login = txtLogin.Text;

                // хэширование пароля
                if (!string.IsNullOrEmpty(txtParol.Text))
                {
                    PasswordHasher passwordHasher = new PasswordHasher();
                    string hashedPassword = passwordHasher.HashPassword(txtParol.Text);
                    user.Parol = hashedPassword;
                }

                // валидация модели
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(sotr);

                // проверка наличия ошибок валидации
                if (Validator.TryValidateObject(sotr, validationContext, validationResults, true))
                {
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены");
                }
                else
                {
                    foreach (var validationResult in validationResults)
                    {
                        MessageBox.Show(validationResult.ErrorMessage);
                    }
                }
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtLastName.Clear();
            txtFirstName.Clear();
            txtPatronymic.Clear();
            txtPhoneNumber.Clear();
            txtPassport.Clear();
            txtPrava.Clear();
            txtLogin.Clear();
            txtParol.Clear();
            cmbRol.SelectedItem = null;
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)//добавление фото в разных форматах
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                // установка изображения
                photoSotrud.Source = new BitmapImage(new Uri(imagePath));
            }
        }
    }
}
