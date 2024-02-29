using ConsoleApp.Model;
using System;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        using (var context = new avtobazaEntities4())
        {
            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите фамилию:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Введите отчество:");
            string surname = Console.ReadLine();

            long nomer;
            do
            {
                Console.WriteLine("Введите номер телефона:");
            } while (!long.TryParse(Console.ReadLine(), out nomer) || nomer.ToString().Length != 11);

            // Получение списка доступных должностей
            var positions = context.Dolzhnosti.ToList();

            // Вывод списка доступных должностей
            Console.WriteLine("Выберите должность из списка:");
            foreach (var position in positions)
            {
                Console.WriteLine($"{position.IDDolzhnosti} - {position.Nazvaniye}");
            }

            // Ввод пользователем номера выбранной должности
            Console.WriteLine("Введите номер должности:");
            if (!int.TryParse(Console.ReadLine(), out int selectedPositionId) ||
                !positions.Any(p => p.IDDolzhnosti == selectedPositionId))
            {
                Console.WriteLine("Некорректный ввод. Выберите номер из списка.");
                return;
            }

            string login;
            do
            {
                Console.WriteLine("Придумайте логин:");
                login = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(login))
                {
                    Console.WriteLine("Логин не может быть пустым.");
                }
            } while (string.IsNullOrWhiteSpace(login));

            string password;
            do
            {
                Console.WriteLine("Придумайте пароль:");
                password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Пароль не может быть пустым.");
                }
            } while (string.IsNullOrWhiteSpace(password));


            // Создание экземпляра PasswordHasher
            PasswordHasher hasher = new PasswordHasher();
            string hashedPassword = hasher.HashPassword(password);

            Users newUser = new Users
            {
                Login = login,
                Parol = hashedPassword 
            };

            // Добавление пользователя в контекст базы данных
            context.Users.Add(newUser);

            // Создание новой записи в таблице Sotrudniki с данными пользователя
            Sotrudniki newSotrudnik = new Sotrudniki
            {
                Imya = firstName,
                Familiya = lastName,
                Otchestvo = surname,
                NomerTel = nomer,
                IDUsera = newUser.IDUsera, // поле, связывающее с таблицей Users
                IDDolzhosti = selectedPositionId // поле, связывающее с таблицей Dolzhnosti
            };

            context.Sotrudniki.Add(newSotrudnik);
            context.SaveChanges();

            Console.WriteLine("Хэшированный пароль: " + hashedPassword);
            Console.WriteLine("Учетная запись добавлена");
        }
    }
}
