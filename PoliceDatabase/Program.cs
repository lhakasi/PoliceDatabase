using System;
using System.Collections.Generic;
using System.Linq;

namespace PoliceDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal();

            terminal.Work();
        }
    }

    class Terminal
    {
        private Database _database = new Database();

        public void Work()
        {
            const string CommandAddNewCriminal = "1";
            const string CommandFindCriminal = "2";
            const string CommandExit = "3";

            bool isWorking = true;

            while (isWorking)
            {
                Console.Clear();

                ShowHeader();

                Console.WriteLine($"{CommandAddNewCriminal} - добавить преступника в базу");
                Console.WriteLine($"{CommandFindCriminal} - найти преступника");
                Console.WriteLine($"{CommandExit} - закрыть терминал");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAddNewCriminal:
                        _database.AddNewCriminal();
                        break;

                    case CommandFindCriminal:
                        _database.FindCriminal();
                        break;

                    case CommandExit:
                        isWorking = false;
                        break;

                    default:
                        StorageOfErrors.ShowError(Errors.InvalidCommand);
                        break;
                }
            }
        }

        private void ShowHeader()
        {
            string header = "   *** База данных полиции ***  ";

            Console.WriteLine(new string('=', header.Length));
            Console.WriteLine(header);
            Console.WriteLine(new string('=', header.Length));
        }
    }

    class Criminal
    {
        private string _surname;
        private string _name;
        private string _middleName;
        private string _status;

        public Criminal(string surname, string name, string middleName, string nationality, int height, int weight, bool isArrested)
        {
            _surname = surname;
            _name = name;
            _middleName = middleName;
            Nationality = nationality;
            Height = height;
            Weight = weight;
            IsArrested = isArrested;

            if (IsArrested)
                _status = "Арестован";
            else
                _status = "В розыске";
        }

        public string Nationality { get; private set; }

        public int Height { get; private set; }
        public int Weight { get; private set; }

        public bool IsArrested { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{_surname} {_name} {_middleName} - " +
                $"Рост: {Height}см. Вес: {Weight}кг. Национальность: {Nationality}. Статус: {_status}");
        }
    }

    class Database
    {
        private List<Criminal> _criminals;

        public Database()
        {
            _criminals = new List<Criminal>()
            {
                new Criminal("Пчелов", "Лунтик", "Селенович", "русский", 100, 95, false),
                new Criminal("МакКлауд", "Дункан", "Горценкевич", "русский", 100, 95, false),
                new Criminal("Крокодилов", "Геннадий", "Чебурашкевич", "русский", 100, 95, true),
            };
        }

        public void AddNewCriminal()
        { 
            Console.Clear();
            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите отчество: ");
            string middleName = Console.ReadLine();

            Console.Write("Введите национальность: ");
            string nationality = Console.ReadLine().ToLower();

            int height = 0;
            FillData(ref height, "рост");

            int weight = 0; 
            FillData(ref weight, "вес");

            bool isArrested = false;
            bool isIncorrect = true;

            while (isIncorrect)
            {
                Console.Write("Заключен под стражу в настоящее время? (да/нет): ");

                string status = Console.ReadLine();

                if (status.ToLower() == "да")
                {
                    isArrested = true;
                    isIncorrect = false;
                }
                else if (status.ToLower() == "нет")
                {
                    isArrested = false;
                    isIncorrect = false;
                }
                else
                {
                    StorageOfErrors.ShowError(Errors.IncorrectInput);
                    isIncorrect = true;
                }
            }

            _criminals.Add(new Criminal(surname, name, middleName, nationality, height, weight, isArrested));
        }

        public void FindCriminal()
        {
            Console.Clear();

            int height = 0;
            FillData(ref height, "рост");

            int weight = 0;
            FillData(ref weight, "вес");

            Console.Write("Введите национальность: ");
            string nationality = Console.ReadLine().ToLower();

            var filteredCriminals = _criminals.Where(criminal =>
            criminal.Height == height
            && criminal.Weight == weight
            && criminal.Nationality == nationality
            && criminal.IsArrested == false);

            foreach (Criminal criminal in filteredCriminals)
                criminal.ShowInfo();

            Console.ReadKey();
        }

        private void FillData(ref int number, string data)
        { 
            bool isIncorrect = true;

            while (isIncorrect)
            {
                Console.Write($"Введите {data}:");
                
                number = GetNumber();
                
                if (number == 0)
                    StorageOfErrors.ShowError(Errors.IncorrectInput);
                else                                    
                    isIncorrect = false;                
            }
        }

        private int GetNumber()
        {
            if (int.TryParse(Console.ReadLine(), out int number) == false || number < 0)            
                return 0;            
            else            
                return number;
        }
    }

    class StorageOfErrors
    {
        public static void ShowError(Errors error)
        {
            Dictionary<Errors, string> errors = new Dictionary<Errors, string>
            {
                { Errors.InvalidCommand, "Недопустимая команда" },
                { Errors.IncorrectInput, "Некорректный ввод" }
            };

            if (errors.ContainsKey(error))
                Console.WriteLine(errors[error]);

            Console.ReadKey();
        }
    }

    enum Errors
    {
        InvalidCommand,
        IncorrectInput        
    }
}
