namespace Lab5;

public class WildAnt : Ant
{
    public WildAnt(int graphSize) : base(graphSize) { }
    protected override int ChooseNext(int[] D, double[] T)
    {
        List<int> _possible = new List<int>();
        int ctr = 0;
        for (int i = 0; i < D.Length; i++)
        {
            if (_visited[i] == false)
            {
                _possible.Add(i);
                ctr++;
            }
        }
        
        int choice = _rand.Next(ctr);
        return _possible[choice];
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