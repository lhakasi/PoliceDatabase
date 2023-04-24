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
                Console.WriteLine("================================");
                Console.WriteLine("   *** База данных полиции ***  ");
                Console.WriteLine("================================");
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
                        Console.WriteLine("Недопустимая команда");
                        break;
                }
            }
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

            if(IsArrested)            
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
            bool isIncorrect = true;

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

            while (isIncorrect)
            {
                Console.Write("Введите рост: ");
                if (int.TryParse(Console.ReadLine(), out int inputHeight) == false || inputHeight < 0)
                {
                    Console.WriteLine("Некорректный ввод");                    
                }
                else
                {
                    height = inputHeight;
                    isIncorrect = false;
                }
            }

            int weight = 0;
            isIncorrect = true;

            while (isIncorrect)
            {
                Console.Write("Введите вес: ");
                if (int.TryParse(Console.ReadLine(), out int inputWeight) == false || inputWeight < 0)
                {
                    Console.WriteLine("Некорректный ввод");                    
                }
                else
                {
                    weight = inputWeight;
                    isIncorrect = false;
                }
            }

            bool isArrested = false;
            isIncorrect = true;

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
                    Console.WriteLine("Некорректный ввод");
                    isIncorrect = true;
                }
            }

            _criminals.Add(new Criminal(surname, name, middleName, nationality, height, weight, isArrested));
        }

        public void FindCriminal()
        {
            bool isIncorrect = true;

            int height = 0;

            Console.Clear();

            while (isIncorrect)
            {
                Console.Write("Введите рост: ");
                if (int.TryParse(Console.ReadLine(), out int inputHeight) == false || inputHeight < 0)
                {
                    Console.WriteLine("Некорректный ввод");                    
                }
                else
                {
                    height = inputHeight;
                    isIncorrect = false;
                }
            }

            int weight = 0;
            isIncorrect = true;

            while (isIncorrect)
            {
                Console.Write("Введите вес: ");
                if (int.TryParse(Console.ReadLine(), out int inputWeight) == false || inputWeight < 0)
                {
                    Console.WriteLine("Некорректный ввод");                    
                }
                else
                {
                    weight = inputWeight;
                    isIncorrect = false;
                }
            }

            Console.Write("Введите национальность: ");
            string nationality = Console.ReadLine().ToLower();

            var filteredCriminals = _criminals.Where(criminal => criminal.Height == height && criminal.IsArrested == false);
            filteredCriminals = filteredCriminals.Where(criminal => criminal.Weight == weight);
            filteredCriminals = filteredCriminals.Where(criminal => criminal.Nationality == nationality);

            foreach (Criminal criminal in filteredCriminals)            
                criminal.ShowInfo();
            
            Console.ReadKey();
        }
    }
}
