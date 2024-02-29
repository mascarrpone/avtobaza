/*
   Этот класс представляет страницу администратора. Здесь реализованы методы для загрузки и отображдения данных о сотрудниках, 
   обработки двойного клика по карточке для открытия окна редактирования и поиска по фио.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using avtobaza.Model;
using HashPasswords;


namespace avtobaza.Pages
{
    public partial class Admin : Page
    {
        
        public List<SotrudInfo> allEmployeeInfo;
        public SotrudInfo SelectedEmployee { get; set; }
        public Admin()
        {
            InitializeComponent();
            Card.MouseDoubleClick += Card_MouseDoubleClick;
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var db = Helper.getContext();
            var employees = db.Sotrudniki// загрузка сотрудников из бд включая связанные таблицы
                    .Include("Dolzhnosti")
                    .Include("Users")
                    .ToList();
           
            allEmployeeInfo = employees.Select(emp => new SotrudInfo // преобразование данных из бд в класс 
            {
                IDSotrudnika = emp.IDSotrudnika,
                UserRol = emp.Dolzhnosti.Nazvaniye,
                UserFIO = $"{emp.Familiya} {emp.Imya} {emp.Otchestvo}",
                LastName = emp.Familiya,
                FirstName = emp.Imya,
                Patronymic = emp.Otchestvo,
                UserPhone = emp.NomerTel,
                UserPassport = emp.Passport,
                IDUsera = emp.IDUsera,
                Login = emp.Users.Login,
                Parol = emp.Users.Parol,
                Prava = emp.VoditelPrava,
            }).ToList();

            Card.ItemsSource = allEmployeeInfo;
        }
        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // получение выбранного сотрудника
            var selectedItem = Card.SelectedItem as SotrudInfo;
            if (selectedItem != null)
            {  
                EditEmployee editEmployee = new EditEmployee(selectedItem, Card);
                NavigationService.Navigate(editEmployee);
            }
        }  
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            // Фильтрация списка сотрудников по фио
            var filteredEmployeeInfos = allEmployeeInfo
                .Where(empInfo => empInfo.UserFIO.ToLower().Contains(searchText))
                .ToList();

            Card.ItemsSource = filteredEmployeeInfos;
        }
        public void SetGreetingAndName(string greeting, string fullNameA)
        {
            userInfoTextBlock.Text = $"{greeting}, {fullNameA}!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            NavigationService.Navigate(add);
        }
    }
}
