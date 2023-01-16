namespace Lab5;

public abstract class Ant
{
    protected static Random _rand = new();
        protected static int a = 2, b = 3;
        public List<int> Path { get; private set; }
        protected bool[] _visited;
        public int Lk { get; private set; }

        protected Ant(int graphSize)
        {
            Lk = 0;
            Path = new();
            _visited = new bool[graphSize];
        }

        public void Run(int[][] D, double[][] T, int startPoint)
        {
            int current = startPoint;
            Path.Add(current);
            
            while (Path.Count < D.Length)
            {
                _visited[current] = true;
                int next = ChooseNext(D[current], T[current]);
                Path.Add(next);
                Lk += D[current][next];
                current = next;
            }
            Lk += D[current][startPoint];
            Path.Add(startPoint);
        }

        protected abstract int ChooseNext(int[] D, double[] T);

        public void Reset()
        {
            Lk = 0;
            Path.Clear();
            _visited = new bool[_visited.Length];
        }

        public abstract void UpdateT(double[][] T, int bestL);
}