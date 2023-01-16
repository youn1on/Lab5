namespace Lab5;

public abstract class Ant
{
    protected static int a = 2, b = 3;
    public int[] Path { get; private set; }
    protected int[] _possibleMoves;
    protected int pathLength { get; set; }
    public int Lk { get; private set; }

    protected Ant(int graphSize)
    {
        Lk = 0;
        Path = new int[graphSize+1];
        pathLength = 0;
        _possibleMoves = new int[graphSize];
        for (int i = 0; i < _possibleMoves.Length; i++)
        {
            _possibleMoves[i] = i;
        }
    }

    public void Run(int[][] D, double[][] T, int startPoint)
    {
        int current = startPoint;
        Path[0] = current;
        pathLength++;
        RemoveFromPossibles(current);
        for (int i = 0; i < _possibleMoves.Length-1; i++)
        {
            int next = ChooseNext(D[current], T[current]);
            Path[i+1] = next;
            pathLength++;
            Lk += D[current][next];
            current = next;
        }
        Lk += D[current][startPoint];
        Path[^1] = startPoint;
        pathLength++;
    }

    protected abstract int ChooseNext(int[] D, double[] T);

    public void Reset()
    {
        Lk = 0;
        pathLength = 0;
        for (int i = 0; i < _possibleMoves.Length; i++)
        {
            _possibleMoves[i] = i;
        }
    }

    public abstract void UpdateT(double[][] T, int bestL);

    protected void RemoveFromPossibles(int index)
    {
        if (index < _possibleMoves.Length - 1)
        {
            Array.Copy(_possibleMoves, index + 1, _possibleMoves, index, _possibleMoves.Length - index - 1);
        }
    }
}