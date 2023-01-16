namespace Lab5;

public class WildAnt : Ant
{
    public WildAnt(int graphSize) : base(graphSize) { }
    protected override int ChooseNext(int[] D, double[] T)
    {
        int choiceIndex = Random.Shared.Next(_possibleMoves.Length-pathLength);
        int choice = _possibleMoves[choiceIndex];
        RemoveFromPossibles(choiceIndex);
        return choice;
    }
    
    public override void UpdateT(double[][] T, int bestL)
    {
        Parallel.For(1, Path.Length, i =>
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