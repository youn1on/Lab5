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
}