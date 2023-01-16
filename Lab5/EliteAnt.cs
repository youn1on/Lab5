namespace Lab5;

public class EliteAnt : Ant
{
    public EliteAnt(int graphSize) : base(graphSize)
    {
        _possibleNumerators = new double[graphSize];
    }
    
    public override void UpdateT(double[][] T, int bestL)
    {
        Parallel.For(1, Path.Length, i =>
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
    
    protected override int ChooseNext(int[] D, double[] T)
    {
        double denominator = 0;
        int possibleLength = _possibleNumerators.Length - pathLength;
        for (int i = 0; i < possibleLength; i++)
        {
            double numerator = Math.Pow(T[_possibleMoves[i]], a) * Math.Pow(1.0 / D[_possibleMoves[i]], b);
            if (numerator < Math.Pow(10, -15)) numerator = Math.Pow(10, -15);
            _possibleNumerators[i] = numerator;
            denominator += numerator;
        }
        for (int i = 0; i < possibleLength; i++)
        {
            _possibleNumerators[i] /= denominator;
        }
        double choice = Random.Shared.NextDouble();
        int chosenIndex = possibleLength-1;
        for (int i = 0; i < possibleLength; i++)
        {
            if (choice < _possibleNumerators[i])
            {
                chosenIndex = i;
                break;
            }
            choice -= _possibleNumerators[i];
        }

        int chosenVertice = _possibleMoves[chosenIndex];
        RemoveFromPossibles(chosenIndex);
        return chosenVertice;
    }
    
    private double[] _possibleNumerators;

}
