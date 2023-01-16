namespace Lab5
{
    public class Ant
    {
        private static Random _rand = new();
        private static int a = 2, b = 3;
        public List<int> Path { get; private set; }
        private int[] _visited;
        public int Lk { get; private set; }

        public Ant(int graphSize)
        {
            Lk = 0;
            Path = new();
            _visited = new int[graphSize];
        }

        public void Run(int[][] D, double[][] T, int startPoint)
        {
            int current = startPoint;
            Path.Add(current);
            
            while (Path.Count < D.Length)
            {
                _visited[current] = 1;
                int next = ChooseNext(D[current], T[current], _visited);
                Path.Add(next);
                Lk += D[current][next];
                _visited[next] = 1;
                current = next;
            }
            Lk += D[current][startPoint];
            Path.Add(startPoint);
        }
        
        private static int ChooseNext(int[] D, double[] T, int[] visited)
        {
            List<KeyValuePair<int, double>> _possible = new List<KeyValuePair<int, double>>();
            double denominator = 0;
            for (int i = 0; i < D.Length; i++)
            {
                if (visited[i] == 0)
                {
                    double numerator = Math.Pow(T[i], a) * Math.Pow(1.0 / D[i], b);
                    if (numerator < Math.Pow(10, -15)) numerator = Math.Pow(10, -15);
                    _possible.Add(new KeyValuePair<int, double>(i, numerator));
                    denominator += numerator;
                }
            }
            double test = 0;
            List<KeyValuePair<int, double>> possible = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < _possible.Count; i++)
            {
                test += _possible[i].Value / denominator;
                possible.Add(new KeyValuePair<int, double>(_possible[i].Key, _possible[i].Value/denominator));
            }
            if (Math.Round(test, 8)!=1) Console.WriteLine($"{test}!=1");
            double choise = _rand.NextDouble();
            foreach (var vertice in possible)
            {
                if (choise < vertice.Value) return vertice.Key;
                choise -= vertice.Value;
            }
            return -1;
        }

        public void Reset()
        {
            Lk = 0;
            Path.Clear();
            _visited = new int[_visited.Length];
        }
    }
}