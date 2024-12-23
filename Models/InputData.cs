using coursework;

namespace coursework
{
    public class InputData
    {
        public Graph G { get; set; }
        public (int X, int Y) StartVertex { get; set; }
        public (int X, int Y) FinishVertex { get; set; }

        public InputData(Graph graph, (int, int) start, (int, int) finish)
        {
            G = graph;
            StartVertex = start;
            FinishVertex = finish;
        }
    }
}
