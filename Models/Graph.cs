using coursework;

namespace coursework
{
    public class Graph
    {
        public int[,] Matrix { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }

        // Конструктор для генерации случайного лабиринта
        public Graph(int size)
        {
            Rows = size;
            Cols = size;
            Matrix = GenerateRandomLabyrinth(size);
        }

        // Конструктор для работы с уже готовым лабиринтом (например, загруженным из файла)
        public Graph(int[,] labyrinth)
        {
            Matrix = labyrinth;
            Rows = labyrinth.GetLength(0);
            Cols = labyrinth.GetLength(1);
        }

        // Метод для генерации случайного лабиринта
        private int[,] GenerateRandomLabyrinth(int size)
        {
            var random = new Random();
            var labyrinth = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    labyrinth[i, j] = (i == 0 && j == 0) ? 0 : random.Next(2); // 0 - проходимо, 1 - непроходимо
                }
            }

            return labyrinth;
        }
    }
}