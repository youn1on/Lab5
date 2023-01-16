namespace Lab5
{
    public class ACO
    {
        private static double p = 0.2;
        private static readonly int M = 50;
        private static int _graphSize;
        private static readonly Random Rand = new();
        public int Lmin;
        public double[][] T;
        private int[][] D;
        private List<int> _bestPath;
        private int _bestL;
        private List<Ant> _ants;

        public ACO(int[][] D, double[][] T)
        {
            _graphSize = D.Length;
            Lmin = Greedy(D);
            this.D = MatrixFactory.Copy(D);
            this.T = MatrixFactory.Copy(T);
            _bestPath = new List<int>();
            _bestL = Int32.MaxValue;
            _ants = new List<Ant>();
            for (int i = 0; i < M; i++)
            {
                _ants.Add(new Ant(D.Length));
            }
        }

        public void Go(int numberOfIterations)
        {
            for (int ictr = 0; ictr < numberOfIterations; ictr++)
            {
                Parallel.For(0, M, i =>
                {
                    _ants[i].Reset();
                    int p = Rand.Next(D.Length);
                    _ants[i].Run(D, T, p);
                });
                // for (int i = 0; i < M; i++)
                // {
                //     _ants[i].Reset();
                //     int p = Rand.Next(D.Length);
                //     _ants[i].Run(D, T, p);
                // }
                UpdateT(_ants);
                Console.WriteLine("\r"+ictr+" : "+_bestL);
            }
            GetResult();
        }

        private void UseVaporization()
        {
            Parallel.ForEach(T, t =>
            {
                Parallel.For(0, t.Length, j =>
                {
                    t[j] *= 1 - p;
                });
            });
        }

        private void UpdateBestL(List<Ant> ants)
        {
            Ant best = ants.MinBy(a => a.Lk)!;
            if (best.Lk < _bestL)
            {
                _bestL = best.Lk;
                _bestPath = best.Path;
            }
        }

        private void UpdateT(List<Ant> ants)
        {
            UseVaporization();
            UpdateBestL(ants);
            Parallel.ForEach(ants, ant =>
            {
                ant.UpdateT(T, _bestL);
            });
        }

        public void GetResult()
        {
            string path = $"[{_bestPath[0]}";
            for(int i = 1; i<_bestPath.Count; i++)
            {
                path += $" ==> {_bestPath[i]}";
            }
            path += $"], {_bestL}";
            Console.WriteLine("\r"+path);
        }
        private static int Greedy(int[][] D)
        {
            int Lmin = 0, currentVertice = 0, nextVertice;
            int[] visited = new int[_graphSize];
            visited[0] = 1;
            for (int ctr = 1; ctr < _graphSize; ctr++)
            {
                nextVertice = GreedyOptimal(D[currentVertice], visited);
                visited[nextVertice] = 1;
                Lmin += D[currentVertice][nextVertice];
                currentVertice = nextVertice;
            }
            Lmin += D[currentVertice][0];
            return Lmin;
        }

        private static int GreedyOptimal(int[] D, int[] visited)
        {
            KeyValuePair<int, int> optimal = new KeyValuePair<int, int>(0, D[0]+1000*visited[0]);
            for (int i = 1; i < D.Length; i++)
            {
                if (D[i]+1000*visited[i]<optimal.Value) optimal = new KeyValuePair<int, int>(i, D[i]+1000*visited[i]);
            }
            return optimal.Key;
        }
    }
}