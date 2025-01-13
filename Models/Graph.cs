using coursework;

namespace coursework
{
    public class Graph
    {
        public int[,] Matrix { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }

        
        public Graph(int size)
        {
            Rows = size;
            Cols = size;
            Matrix = GenerateRandomLabyrinth(size);
        }

        
        public Graph(int[,] labyrinth)
        {
            Matrix = labyrinth;
            Rows = labyrinth.GetLength(0);
            Cols = labyrinth.GetLength(1);
        }

        
        private int[,] GenerateRandomLabyrinth(int size)
        {
            var random = new Random();
            var labyrinth = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    labyrinth[i, j] = (i == 0 && j == 0) ? 0 : random.Next(2); 
                }
            }

            return labyrinth;
        }
    }
}