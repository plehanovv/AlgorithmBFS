using coursework;

namespace coursework
{
    public class OutputData
    {
        public List<(int X, int Y)> Way { get; set; } = new List<(int X, int Y)>();
        
        public void SavePathToFile(string filePath)
        {
            try
            {
                var lines = Way.Select(p => $"{p.X},{p.Y}").ToList(); // Преобразуем координаты в строки
                File.WriteAllLines(filePath, lines); // Записываем в файл
                Console.WriteLine($"Путь успешно сохранен в файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении пути в файл: " + ex.Message);
            }
        }
    }
}
