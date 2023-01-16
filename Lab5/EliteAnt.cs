namespace Lab5;

public class EliteAnt : Ant
{
    public EliteAnt(int graphSize) : base(graphSize) { }
    
    public override void UpdateT(double[][] T, int bestL)
    {
        Parallel.For(1, Path.Count, i =>
        {
            double value = (double)bestL / Lk * 2;
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
