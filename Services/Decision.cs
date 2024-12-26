using coursework;

namespace coursework
{
    // Класс для управления процессом выполнения алгоритма поиска пути.
    public class Decision
    {
        // Входные данные для алгоритма.
        public InputData InData { get; set; }

        // Выходные данные, содержащие результат работы алгоритма.
        public OutputData OutData { get; set; }

        // Экземпляр класса Algorithm для выполнения поиска пути.
        private Algorithm Algorithm { get; set; }

        // Конструктор, принимающий входные данные и инициализирующий объект алгоритма.
        public Decision(InputData input)
        {
            InData = input; // Сохраняем входные данные.
            Algorithm = new Algorithm(); // Создаем экземпляр алгоритма.
        }

        // Метод для запуска алгоритма поиска пути.
        public void RunAlgorithm()
        {
            // Выполняем поиск пути с использованием метода BFS.
            OutData = Algorithm.BFS(InData);
        }
    }
}