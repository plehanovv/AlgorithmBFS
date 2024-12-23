using coursework;

namespace coursework
{
    public class Algorithm
    {
        private static readonly int[] dx = { -1, 0, 1, 0 };
        private static readonly int[] dy = { 0, 1, 0, -1 };

        public OutputData BFS(InputData data)
        {
            int rows = data.G.Matrix.GetLength(0);
            int cols = data.G.Matrix.GetLength(1);
            var distance = new int[rows, cols];
            var previous = new (int X, int Y)?[rows, cols];
            var queue = new Queue<(int X, int Y)>();

            for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                distance[i, j] = int.MaxValue;

            distance[data.StartVertex.X, data.StartVertex.Y] = 0;
            queue.Enqueue(data.StartVertex);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == data.FinishVertex)
                    return new OutputData { Way = ReconstructPath(previous, data.StartVertex, data.FinishVertex) };

                for (int i = 0; i < 4; i++)
                {
                    int nx = current.X + dx[i];
                    int ny = current.Y + dy[i];

                    if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && data.G.Matrix[nx, ny] == 0 &&
                        distance[nx, ny] == int.MaxValue)
                    {
                        distance[nx, ny] = distance[current.X, current.Y] + 1;
                        previous[nx, ny] = current;
                        queue.Enqueue((nx, ny));
                    }
                }
            }

            return new OutputData { Way = null };
        }


        private List<(int X, int Y)> ReconstructPath((int X, int Y)?[,] previous, (int X, int Y) start,
            (int X, int Y) finish)
        {
            var path = new List<(int X, int Y)>();
            var current = finish;

            while (current != start)
            {
                path.Insert(0, current);
                current = previous[current.X, current.Y] ?? start;
            }

            path.Insert(0, start);
            return path;
        }
    }
}