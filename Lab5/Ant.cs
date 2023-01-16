namespace Lab5
{
    public class Ant
    {
        protected static Random _rand = new();
        protected static int a = 2, b = 3;
        public List<int> Path { get; private set; }
        protected bool[] _visited;
        public int Lk { get; private set; }

        public Ant(int graphSize)
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
        
        protected virtual int ChooseNext(int[] D, double[] T)
        {
            List<KeyValuePair<int, double>> _possible = new List<KeyValuePair<int, double>>();
            double denominator = 0;
            for (int i = 0; i < D.Length; i++)
            {
                if (!_visited[i])
                {
                    double numerator = Math.Pow(T[i], a) * Math.Pow(1.0 / D[i], b);
                    if (numerator < Math.Pow(10, -15)) numerator = Math.Pow(10, -15);
                    _possible.Add(new KeyValuePair<int, double>(i, numerator));
                    denominator += numerator;
                }
            }
            List<KeyValuePair<int, double>> possible = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < _possible.Count; i++)
            {
                possible.Add(new KeyValuePair<int, double>(_possible[i].Key, _possible[i].Value/denominator));
            }
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
            _visited = new bool[_visited.Length];
        }

        public virtual void UpdateT(double[][] T, int bestL)
        {
            Parallel.For(1, Path.Count, i =>
            {
                double value = (double)bestL / Lk;
                T[Path[i - 1]][Path[i]] += value;
                double newCurrentValue = T[Path[i - 1]][Path[i]];
                while (true)
                {
                    double currentValue = newCurrentValue;
                    double newValue = currentValue + value;
                    newCurrentValue = Interlocked.CompareExchange(ref T[Path[i - 1]][Path[i]], newValue, currentValue);
                    if (newCurrentValue.Equals(currentValue))
                        break;
                }
            });
        }
    }
}