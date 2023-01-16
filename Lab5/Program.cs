namespace Lab5
{
    internal static class Program
    {
        public static void Main()
        {
            //NewFieldGenerator.GenerateNewField(@"test.csv", 300);
            MatrixFactory mf = new MatrixFactory(@"test.csv");
            int[][] D = mf.GetD();
            double[][] T = mf.GetT();
            ACO aco = new(D, T);
            Console.WriteLine($"Lmin = {aco.Lmin}");
            aco.Go(1000);
        }
    }
}