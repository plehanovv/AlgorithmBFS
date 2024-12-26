using coursework;

namespace coursework
{
    // Класс для хранения выходных данных алгоритма поиска пути.
    public class OutputData
    {
        // Список координат, представляющих путь (каждая точка задается как пара X и Y).
        public List<(int X, int Y)> Way { get; set; } = new List<(int X, int Y)>();

        // Метод для сохранения пути в текстовый файл.
        public void SavePathToFile(string filePath)
        {
            try
            {
                // Преобразуем список координат в строки формата "X,Y".
                var lines = Way.Select(p => $"{p.X},{p.Y}").ToList();

                // Записываем строки в указанный файл.
                File.WriteAllLines(filePath, lines);

                // Сообщаем пользователю, что путь успешно сохранен.
                Console.WriteLine($"Путь успешно сохранен в файл: {filePath}");
            }
            catch (Exception ex)
            {
                // Обрабатываем ошибки и выводим сообщение об ошибке.
                Console.WriteLine("Ошибка при сохранении пути в файл: " + ex.Message);
            }
        }
    }
}
