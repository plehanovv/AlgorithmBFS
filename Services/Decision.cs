using coursework;

namespace coursework
{
    public class Decision
    {
        public InputData InData { get; set; }
        public OutputData OutData { get; set; }
        private Algorithm Algorithm { get; set; }

        public Decision(InputData input)
        {
            InData = input;
            Algorithm = new Algorithm();
        }

        public void RunAlgorithm()
        {
            OutData = Algorithm.BFS(InData);
        }
    }
}
