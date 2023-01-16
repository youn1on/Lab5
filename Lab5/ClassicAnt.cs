namespace Lab5
{
    public class ClassicAnt : Ant
    {
        public ClassicAnt(int graphSize) : base(graphSize)
        {
        }

        protected override int ChooseNext(int[] D, double[] T)
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

        public override void UpdateT(double[][] T, int bestL)
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