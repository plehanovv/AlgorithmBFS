using coursework;

namespace coursework
{
    public class Algorithm
    {
        // Смещения для перемещения по четырем направлениям: вверх, вправо, вниз, влево.
        private static readonly int[] dx = { -1, 0, 1, 0 }; // Изменения по оси X
        private static readonly int[] dy = { 0, 1, 0, -1 }; // Изменения по оси Y

        // Метод, реализующий поиск пути с помощью алгоритма BFS (поиск в ширину).
        public OutputData BFS(InputData data)
        {
            int rows = data.G.Matrix.GetLength(0); // Количество строк в матрице.
            int cols = data.G.Matrix.GetLength(1); // Количество столбцов в матрице.

            // Массив для хранения расстояний от стартовой точки.
            var distance = new int[rows, cols];

            // Массив для хранения информации о предыдущей точке на пути.
            var previous = new (int X, int Y)?[rows, cols];

            // Очередь для хранения точек, которые нужно обработать.
            var queue = new Queue<(int X, int Y)>();

            // Инициализируем массив расстояний максимальным значением.
            for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                distance[i, j] = int.MaxValue;

            // Устанавливаем расстояние до стартовой точки равным 0.
            distance[data.StartVertex.X, data.StartVertex.Y] = 0;

            // Добавляем стартовую точку в очередь.
            queue.Enqueue(data.StartVertex);

            // Основной цикл BFS.
            while (queue.Count > 0)
            {
                // Извлекаем текущую точку из очереди.
                var current = queue.Dequeue();

                // Если достигли конечной точки, восстанавливаем путь и возвращаем результат.
                if (current == data.FinishVertex)
                    return new OutputData { Way = ReconstructPath(previous, data.StartVertex, data.FinishVertex) };

                // Перебираем все возможные направления движения.
                for (int i = 0; i < 4; i++)
                {
                    int nx = current.X + dx[i]; // Новая координата X
                    int ny = current.Y + dy[i]; // Новая координата Y

                    // Проверяем, находится ли точка в пределах лабиринта, проходима ли она, 
                    // и не была ли она уже посещена.
                    if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && 
                        data.G.Matrix[nx, ny] == 0 && distance[nx, ny] == int.MaxValue)
                    {
                        // Обновляем расстояние и сохраняем предыдущую точку.
                        distance[nx, ny] = distance[current.X, current.Y] + 1;
                        previous[nx, ny] = current;

                        // Добавляем новую точку в очередь.
                        queue.Enqueue((nx, ny));
                    }
                }
            }

            // Если путь не найден, возвращаем пустой результат.
            return new OutputData { Way = null };
        }

        // Метод для восстановления пути на основе массива предыдущих точек.
        private List<(int X, int Y)> ReconstructPath((int X, int Y)?[,] previous, (int X, int Y) start,
            (int X, int Y) finish)
        {
            var path = new List<(int X, int Y)>(); // Список для хранения пути.
            var current = finish; // Начинаем с конечной точки.

            // Проходим по массиву previous, пока не дойдем до стартовой точки.
            while (current != start)
            {
                path.Insert(0, current); // Добавляем текущую точку в начало пути.
                current = previous[current.X, current.Y] ?? start; // Переходим к предыдущей точке.
            }

            path.Insert(0, start); // Добавляем стартовую точку в начало пути.
            return path; // Возвращаем восстановленный путь.
        }
    }
}
