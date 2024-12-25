using coursework;
using System;
using System.IO;
using System.Diagnostics;

namespace coursework
{
    class Menu
    {
        public void ShowMenu()
        {
            Console.WriteLine("Выберите один из вариантов:");
            Console.WriteLine("1. Создать лабиринт вручную.");
            Console.WriteLine("2. Загрузить лабиринт из файла.");
            int choice = int.Parse(Console.ReadLine());

            InputData inputData;
            if (choice == 1)
            {
                inputData = CreateLabyrinthManually();
            }
            else if (choice == 2)
            {
                inputData = LoadLabyrinthFromFile();
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                return;
            }

            var decision = new Decision(inputData);

            // Измеряем время выполнения алгоритма
            var stopwatch = Stopwatch.StartNew();
            decision.RunAlgorithm();
            stopwatch.Stop();

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
                    string fileName = Console.ReadLine();   // Получаем имя файла от пользователя

                    // Получаем путь к корневой папке проекта
                    string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent
                        .FullName;

                    // Путь до папки WayResults в корне проекта
                    string directoryPath = Path.Combine(projectRoot, "WayResults");

                    // Проверяем, существует ли папка, если нет, создаем её
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Добавляем к имени файла метку времени для уникальности
                    string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string uniqueFileName =
                        $"{Path.GetFileNameWithoutExtension(fileName)}_{timeStamp}{Path.GetExtension(fileName)}";

                    // Формируем полный путь с уникальным именем файла
                    string filePath = Path.Combine(directoryPath, uniqueFileName);

                    // Сохраняем путь в указанный файл
                    decision.OutData.SavePathToFile(filePath);

                    Console.WriteLine($"Путь сохранён в файл: {uniqueFileName}");
                }
            }
            else
            {
                Console.WriteLine("Путь не найден.");
            }

            Console.WriteLine($"Время, затраченное на поиск пути: {stopwatch.ElapsedMilliseconds} мс");
        }

        private InputData CreateLabyrinthManually()
        {
            Console.WriteLine("Введите количество строк лабиринта (M): ");
            int rows = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество столбцов лабиринта (N): ");
            int cols = int.Parse(Console.ReadLine());

            var labyrinth = new int[rows, cols];

            Console.WriteLine("Введите лабиринт построчно (0 - проходимо, 1 - непроходимо):");
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"Строка {i + 1}:");
                var values = Console.ReadLine().Split(',');
                for (int j = 0; j < cols; j++)
                {
                    labyrinth[i, j] = int.Parse(values[j]);
                }
            }

            DisplayLabyrinth(labyrinth);

            Console.WriteLine("Введите координаты начальной точки (x y): ");
            var start = ParseCoordinates(Console.ReadLine());

            Console.WriteLine("Введите координаты конечной точки (x y): ");
            var end = ParseCoordinates(Console.ReadLine());

            var graph = new Graph(labyrinth);
            return new InputData(graph, start, end);
        }

        private InputData LoadLabyrinthFromFile()
        {
            Console.WriteLine("Введите путь к файлу с лабиринтом (например, labyrinth.txt): ");
            string filePath = Console.ReadLine();

            // Загрузим данные из файла
            var (labyrinth, start, end) = LoadLabyrinthAndPointsFromFile(filePath);

            DisplayLabyrinth(labyrinth);

            var graph = new Graph(labyrinth);
            return new InputData(graph, start, end);
        }

        private (int[,] labyrinth, (int X, int Y) start, (int X, int Y) end) LoadLabyrinthAndPointsFromFile(
            string filePath)
        {
            // Проверяем, существует ли файл
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден: " + filePath);
                throw new FileNotFoundException("Файл не найден: " + filePath);
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Предполагается, что последние две строки содержат start и end
                string startLine = lines[^2];
                string endLine = lines[^1];

                var startParts = startLine.Split(',');
                var endParts = endLine.Split(',');

                var start = (int.Parse(startParts[0]), int.Parse(startParts[1]));
                var end = (int.Parse(endParts[0]), int.Parse(endParts[1]));

                // Лабиринт - все строки до start и end
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

        private (int X, int Y) ParseCoordinates(string input)
        {
            var parts = input.Split(' ');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}