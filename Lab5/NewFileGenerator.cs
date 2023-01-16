namespace Lab5
{
    public class NewFieldGenerator
    {
        private const int StandardFieldSize = 150;
        private const int MinDist = 5;
        private const int MaxDist = 150;
        
        public static void GenerateNewField(string filename, int n = StandardFieldSize, bool symmetry = false)
        {
            Random rand = new Random();
            if (filename.Length > 4 && filename[^4] != '.') filename += ".csv";
            if (!File.Exists(filename))
            {
                using (File.Create(filename)) { }
            }

            if (!symmetry)
            {
                using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(n);
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (i == j) sw.Write(0);
                            else sw.Write(rand.Next(MinDist, MaxDist + 1));
                            if (j < n - 1) sw.Write(",");
                        }

                        if (i < n - 1) sw.WriteLine();
                    }
                }
            }
            else
            {
                int[,] matr = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        matr[i, j] = matr[j, i] = rand.Next(MinDist, MaxDist + 1);
                    }
                }
                
                using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(n);
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n-1; j++)
                        {
                            sw.Write(matr[i, j]+",");
                        }
                        sw.WriteLine(matr[i, n-1]);
                    }
                }
            }
        }
    }
}