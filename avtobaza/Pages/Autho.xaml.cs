using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using avtobaza.Model;
using HashPasswords;
using System.Windows.Threading;
using System.Net;
using System.Net.Mail;

namespace avtobaza.Pages
{
    /// <summary>
    /// класс страницы авторизации, который нужен для входа в систему
    /// </summary>
    public partial class Autho : Page
    {
        // Переменные для управления попытками входа и блокировкой
        private bool needCaptcha = false;
        private int loginAttempts = 0;
        private const int MaxLoginAttempts = 3;
        private bool isLocked = false;
        private DispatcherTimer lockoutTimer;
        private int remainingTime = 10;

        public Autho()
        {
            InitializeComponent();
            HideCaptcha();
            GenerateCaptcha();
        }
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWorkingHours())
            {
                MessageBox.Show("Вход запрещен. Рабочее время с 10:00 до 19:00.");
                return;
            }

            if (isLocked)
            {
                MessageBox.Show("Попробуйте снова через 10 секунд.");
                return;
            }

            // Получение логина и пароля из полей ввода
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();
            PasswordHasher hasher = new PasswordHasher();
            string hashedPassword = hasher.HashPassword(password);

            // Проверка наличия такого пользователя в базе данных
            using (var context = Helper.getContext())
            {
                var users = context.Users.FirstOrDefault(u => u.Login == login);

                if (users != null && users.Parol == hashedPassword)
                {
                    var sotrud = context.Sotrudniki.FirstOrDefault(s => s.IDUsera == users.IDUsera);

                    if (sotrud != null)
                    {
                        var position = context.Dolzhnosti.FirstOrDefault(d => d.IDDolzhnosti == sotrud.IDDolzhnosti)?.Nazvaniye;

                        if (!string.IsNullOrEmpty(position))//навигация в зависимости от роли сотрудника
                        {
                            NavigateToNextPage(position, sotrud);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Роль пользователя не определена.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не связан с сотрудником.");
                    }
                }
                else
                {
                    loginAttempts++;

                    if (loginAttempts >= MaxLoginAttempts)
                    {
                        // Блокировка после 3 неудачных попыток
                        isLocked = true;
                        loginAttempts = 0;
                        LockoutTimerText.Visibility = Visibility.Visible;
                        MessageBox.Show("Превышено количество попыток. Попробуйте через 10 секунд.");

                        // запуск таймера блокировки
                        lockoutTimer = new DispatcherTimer();
                        lockoutTimer.Interval = TimeSpan.FromSeconds(1);
                        lockoutTimer.Tick += LockoutTimer_Tick;
                        lockoutTimer.Start();
                    }
                    else
                    {
                        WrongtLoginOrPassword();
                    }
                }
            }
        }
        // Проверка рабочее ли время
        private bool IsWorkingHours()
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;

            return (hour >= 10 && hour < 19);
        }

        // приветствие в зависимости от времени суток
        private string GetGreeting()
        {
            string greeting = "";
            DateTime now = DateTime.Now;
            int hour = now.Hour;

            if (hour >= 10 && hour < 12)
                greeting = "Доброе утро";
            else if (hour >= 12 && hour < 17)
                greeting = "Добрый день";
            else if (hour >= 17 && hour < 19)
                greeting = "Добрый вечер";
            return greeting;
        }

        // Обработчик таймера для управления блокировкой
        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            remainingTime--;
            LockoutTimerText.Text = $"Оставшееся время: {remainingTime} секунд";

            if (remainingTime <= 0)
            {
                lockoutTimer.Stop();
                LockoutTimerText.Visibility = Visibility.Collapsed;
                isLocked = false;
                remainingTime = 10;

                txtLogin.IsEnabled = true;
                txtPassword.IsEnabled = true;
                txtboxCaptcha.IsEnabled = true;
                btnEnter.IsEnabled = true;
                btnEnterGuests.IsEnabled = true;
            }
            else
            {
                // Блокировка полей 
                txtLogin.IsEnabled = false;
                txtPassword.IsEnabled = false;
                txtboxCaptcha.IsEnabled = false;
                btnEnter.IsEnabled = false;
                btnEnterGuests.IsEnabled = false;
            }
        }

        // Навигация в зависимости от роли пользователя
        private void NavigateToNextPage(string position, Model.Sotrudniki sotrud)
        {
            switch (position)
            {
                case "Администратор":
                    var adminPage = new Admin();
                    string fullNameA = $"{sotrud.Familiya} {sotrud.Imya} {sotrud.Otchestvo}";
                    adminPage.SetGreetingAndName(GetGreeting(), fullNameA);
                    NavigationService.Navigate(adminPage);
                    break;

                case "Диспетчер":
                    var dispatcherPage = new Dispatcher();
                    string fullNameD = $"{sotrud.Familiya} {sotrud.Imya} {sotrud.Otchestvo}";
                    dispatcherPage.SetGreetingAndName(GetGreeting(), fullNameD);
                    NavigationService.Navigate(dispatcherPage);
                    break;

                case "Водитель":
                    var driverPage = new Driver();
                    string fullNameV = $"{sotrud.Familiya} {sotrud.Imya} {sotrud.Otchestvo}";
                    driverPage.SetGreetingAndName(GetGreeting(), fullNameV);
                    NavigationService.Navigate(driverPage);
                    break;

                default:
                    break;
            }
        }

        private void WrongtLoginOrPassword()
        {
            if (!needCaptcha)
            {
                MessageBox.Show("Неверный логин или пароль.");
                needCaptcha = true;
                ShowCaptcha();
            }
            else if (txtboxCaptcha.Text != txtBlockCaptcha.Text)
            {
                MessageBox.Show("Неправильная капча.");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
                GenerateCaptcha();
            }
        }

        private void GenerateCaptcha()
        {
            Random random = new Random();
            int randnNum = random.Next(1, 4);

            switch (randnNum)
            {
                case 1:
                    txtBlockCaptcha.Text = "ghghg";
                    txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case 2:
                    txtBlockCaptcha.Text = "dkff";
                    txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case 3:
                    txtBlockCaptcha.Text = "rdrmm";
                    txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
                default:
                    break;
            }
        }
        private void HideCaptcha()
        {
            txtboxCaptcha.Visibility = Visibility.Hidden;
            txtBlockCaptcha.Visibility = Visibility.Hidden;
        }
        private void ShowCaptcha()
        {
            txtboxCaptcha.Visibility = Visibility.Visible;
            txtBlockCaptcha.Visibility = Visibility.Visible;
        }


        private void txtboxCaptcha_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            txtLoginOrPochta.Visibility = Visibility.Visible;
            txtBoxLoginOrPochta.Visibility = Visibility.Visible;
            btnContinue.Visibility = Visibility.Visible;

          
        }
        private string verificationCode;
        private string loginOrEmail;
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            loginOrEmail = txtBoxLoginOrPochta.Text.Trim();

            using (var context = Helper.getContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == loginOrEmail);

                if (user != null)
                {
                    var sotrud = context.Sotrudniki.FirstOrDefault(s => s.IDUsera == user.IDUsera);

                    if (sotrud != null)
                    {
                        // Отправка кода подтверждения на почту пользователя
                        string userEmail = sotrud.Pochta;
                        verificationCode = SendVerificationCode(userEmail);

                        txtBlockVerificationCode.Visibility = Visibility.Visible;
                        txtEnteredVerificationCode.Visibility = Visibility.Visible;
                        btnConfirmCode.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Почта для указанного пользователя не найдена.");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с указанным логином не найден.");
                }
            }

            btnContinue.Visibility = Visibility.Collapsed;
            txtBlockVerificationCode.Visibility = Visibility.Visible;
            txtEnteredVerificationCode.Visibility = Visibility.Visible;
            
        }
        
        private string SendVerificationCode(string userEmail)
        {
            try
            {
                string code = GenerateVerificationCode();
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "anonim.f02@gmail.com";
                string senderPassword = "qpoaabdnrgzglqqu";

                // Создание объекта сообщения
                using (MailMessage mailMessage = new MailMessage(senderEmail, userEmail, "Код подтверждения", $"Ваш код подтверждения: {code}"))
                {
                    mailMessage.IsBodyHtml = true; 
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailMessage);
                    }
                }

                MessageBox.Show($"Код подтверждения был отправлен на почту {userEmail}.");
                return code;  // Возвращаем код
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке кода подтверждения: {ex.Message}");
                return null;
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            int verificationCode = random.Next(1000, 9999);
            return verificationCode.ToString();
        }

        private void btnConfirmCode_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = txtEnteredVerificationCode.Text.Trim();

            if (enteredCode == verificationCode)
            {
                NavigationService.Navigate(new NewPassword(loginOrEmail));
            }
            else
            {
                MessageBox.Show("Введен неверный код подтверждения. Попробуйте еще раз.");
            }
        }
    }
}

