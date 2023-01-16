namespace Lab5
{
    public class ACO
    {
        private static double p = 0.5;
        private static readonly int M = 50;
        private static int _graphSize;
        private static readonly int c = 8, w = 8, e = 2;
        public int Lmin;
        public double[][] T;
        private int[][] D;
        private int[] _bestPath;
        private int _bestL;
        private List<Ant> _ants;

        public ACO(int[][] D, double[][] T)
        {
            _graphSize = D.Length;
            Lmin = Greedy(D);
            this.D = MatrixFactory.Copy(D);
            this.T = MatrixFactory.Copy(T);
            _bestPath = new int[D.Length+1];
            _bestL = Int32.MaxValue;
            _ants = new List<Ant>();
            for (int i = 0; i < M*c/(c+e+w); i++)
            {
                _ants.Add(new ClassicAnt(D.Length));
            }
            for (int i = 0; i < M*w/(c+e+w); i++)
            {
                _ants.Add(new WildAnt(D.Length));
            }
            for (int i = 0; i < M*e/(c+e+w); i++)
            {
                _ants.Add(new EliteAnt(D.Length));
            }
        }

        public void Go(int numberOfIterations)
        {
            for (int ictr = 0; ictr < numberOfIterations; ictr++)
            {
                Parallel.For(0, _ants.Count, i =>
                {
                    _ants[i].Reset();
                    int p = Random.Shared.Next(D.Length);
                    _ants[i].Run(D, T, p);
                });
                UpdateT(_ants);
                Console.WriteLine("\r"+ictr+" : "+_bestL);
            }
            GetResult();
        }

        private void UseVaporization()
        {
            double coef = 1 - p;
            Parallel.For(0, T.Length, i =>
            {
                Parallel.For(0, T[i].Length, j =>
                {
                    T[i][j] *= coef;
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
            for(int i = 1; i<_bestPath.Length; i++)
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