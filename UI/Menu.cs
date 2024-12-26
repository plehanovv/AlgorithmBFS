using coursework;
using System;
using System.IO;
using System.Diagnostics;

namespace coursework
{
    class Menu
    {
        // Метод для отображения главного меню программы.
        public void ShowMenu()
        {
            int choice = 0;
            bool validInput = false;

            // Запрашиваем ввод до тех пор, пока пользователь не введет корректный выбор.
            while (!validInput)
            {
                Console.WriteLine("Выберите один из вариантов:");
                Console.WriteLine("1. Создать лабиринт вручную.");
                Console.WriteLine("2. Загрузить лабиринт из файла.");
                
                string input = Console.ReadLine();
                
                // Проверка на корректный выбор.
                if (int.TryParse(input, out choice) && (choice == 1 || choice == 2))
                {
                    validInput = true; // Выход из цикла, если введен правильный выбор.
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите 1 или 2.");
                }
            }

            InputData inputData;

            // Выбор варианта: создание лабиринта вручную или загрузка из файла.
            if (choice == 1)
            {
                inputData = CreateLabyrinthManually();
            }
            else
            {
                inputData = LoadLabyrinthFromFile();
            }

            var decision = new Decision(inputData);

            // Замер времени выполнения алгоритма.
            var stopwatch = Stopwatch.StartNew();
            decision.RunAlgorithm();
            stopwatch.Stop();

            // Обработка результатов работы алгоритма.
            if (decision.OutData.Way != null)
            {
                Console.WriteLine("Путь найден:");
                foreach (var step in decision.OutData.Way)
                {
                    Console.WriteLine($"({step.X}, {step.Y})");
                }

                Console.WriteLine("Хотите сохранить путь в файл? (y/n)");
                var saveChoice = Console.ReadLine();
                if (saveChoice?.ToLower() == "y")
                {
                    Console.WriteLine("Введите название файла (например, path.txt): ");
                    string fileName = Console.ReadLine(); // Получаем имя файла от пользователя.

                    // Получаем путь к корневой папке проекта.
                    string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent
                        .FullName;

                    // Путь до папки WayResults в корне проекта.
                    string directoryPath = Path.Combine(projectRoot, "WayResults");

                    // Создаем папку, если она не существует.
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Генерируем уникальное имя файла с меткой времени.
                    string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string uniqueFileName =
                        $"{Path.GetFileNameWithoutExtension(fileName)}_{timeStamp}{Path.GetExtension(fileName)}";

                    // Полный путь до файла.
                    string filePath = Path.Combine(directoryPath, uniqueFileName);

                    // Сохраняем путь в файл.
                    decision.OutData.SavePathToFile(filePath);

                    Console.WriteLine($"Путь сохранён в файл: {uniqueFileName}");
                }
            }
            else
            {
                Console.WriteLine("Путь не найден.");
            }

            Console.WriteLine($"Время, затраченное на поиск пути: {stopwatch.ElapsedMilliseconds} мс");
            Console.ReadLine();
        }

        // Метод для ручного создания лабиринта.
        private InputData CreateLabyrinthManually()
        {
            int rows = 0, cols = 0;
            bool validInput = false;

            // Запрашиваем количество строк и столбцов, пока не введены корректные значения.
            while (!validInput)
            {
                Console.WriteLine("Введите количество строк лабиринта (M): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out rows) && rows > 0)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите положительное целое число.");
                }
            }

            validInput = false;

            while (!validInput)
            {
                Console.WriteLine("Введите количество столбцов лабиринта (N): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out cols) && cols > 0)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите положительное целое число.");
                }
            }

            var labyrinth = new int[rows, cols];

            // Ввод лабиринта построчно.
            Console.WriteLine("Введите лабиринт построчно (0 - проходимо, 1 - непроходимо, в формате: 0,1,0):");
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"Строка {i + 1}:");
                string line = Console.ReadLine();
                var values = line.Split(',');

                for (int j = 0; j < cols; j++)
                {
                    bool isValidValue = false;

                    // Попробуем парсить значение для каждой ячейки.
                    while (!isValidValue)
                    {
                        if (values.Length <= j)
                        {
                            Console.WriteLine($"Ошибка: для строки {i + 1} недостает значений. Пожалуйста, введите все элементы.");
                            line = Console.ReadLine();  // Повторный ввод строки.
                            values = line.Split(',');
                        }
                        else if (!int.TryParse(values[j], out labyrinth[i, j]) || (labyrinth[i, j] != 0 && labyrinth[i, j] != 1))
                        {
                            Console.WriteLine("Ошибка: введено неправильное значение. Введите 0 или 1.");
                            line = Console.ReadLine();  // Повторный ввод строки.
                            values = line.Split(',');
                        }
                        else
                        {
                            isValidValue = true; // Если значение корректно, выходим из цикла.
                        }
                    }
                }
            }

            DisplayLabyrinth(labyrinth);

            // Ввод начальной и конечной точек.
            var start = GetValidCoordinates("начальной");
            var end = GetValidCoordinates("конечной");

            var graph = new Graph(labyrinth);
            return new InputData(graph, start, end);
        }

        // Метод для ввода и проверки координат.
        private (int X, int Y) GetValidCoordinates(string pointType)
        {
            (int X, int Y) coordinates = (-1, -1);
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"Введите координаты {pointType} точки (в формате: x y): ");
                string input = Console.ReadLine();
                var parts = input.Split(' ');

                if (parts.Length == 2 && int.TryParse(parts[0], out coordinates.X) && int.TryParse(parts[1], out coordinates.Y))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Неверный формат координат. Пожалуйста, введите два целых числа, разделенных пробелом.");
                }
            }

            return coordinates;
        }

        // Метод для загрузки лабиринта из файла.
        private InputData LoadLabyrinthFromFile()
        {
            Console.WriteLine("Введите путь к файлу с лабиринтом (например, labyrinth.txt): ");
            string filePath = Console.ReadLine();

            // Загружаем данные из файла.
            var (labyrinth, start, end) = LoadLabyrinthAndPointsFromFile(filePath);

            DisplayLabyrinth(labyrinth);

            var graph = new Graph(labyrinth);
            return new InputData(graph, start, end);
        }

        // Метод для загрузки лабиринта и точек из файла.
        private (int[,] labyrinth, (int X, int Y) start, (int X, int Y) end) LoadLabyrinthAndPointsFromFile(
            string filePath)
        {
            // Проверяем существование файла.
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден: " + filePath);
                throw new FileNotFoundException("Файл не найден: " + filePath);
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Последние две строки файла содержат начальную и конечную точки.
                string startLine = lines[^2];
                string endLine = lines[^1];

                var startParts = startLine.Split(',');
                var endParts = endLine.Split(',');

                var start = (int.Parse(startParts[0]), int.Parse(startParts[1]));
                var end = (int.Parse(endParts[0]), int.Parse(endParts[1]));

                // Загружаем лабиринт из остальных строк.
                int rows = lines.Length - 2;
                int cols = lines[0].Split(',').Length;

                var labyrinth = new int[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    var values = lines[i].Split(',');
                    for (int j = 0; j < cols; j++)
                    {
                        labyrinth[i, j] = int.Parse(values[j]);
                    }
                }

                return (labyrinth, start, end);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обработке файла: " + ex.Message);
                throw;
            }
        }

        // Метод для отображения лабиринта в консоли.
        private void DisplayLabyrinth(int[,] labyrinth)
        {
            Console.WriteLine("Текущий лабиринт:");
            for (int i = 0; i < labyrinth.GetLength(0); i++)
            {
                for (int j = 0; j < labyrinth.GetLength(1); j++)
                {
                    Console.Write(labyrinth[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
    }
}
