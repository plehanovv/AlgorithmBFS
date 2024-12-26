using coursework;

namespace coursework
{
    // Класс для хранения входных данных алгоритма поиска пути.
    public class InputData
    {
        // Граф, представляющий пространство для поиска пути (например, лабиринт).
        public Graph G { get; set; }

        // Координаты начальной точки пути в формате (X, Y).
        public (int X, int Y) StartVertex { get; set; }

        // Координаты конечной точки пути в формате (X, Y).
        public (int X, int Y) FinishVertex { get; set; }

        // Конструктор для инициализации графа, начальной и конечной точек.
        public InputData(Graph graph, (int, int) start, (int, int) finish)
        {
            G = graph; // Инициализация графа.
            StartVertex = start; // Установка начальной точки пути.
            FinishVertex = finish; // Установка конечной точки пути.
        }
    }
}
