namespace Lab5
{
    public class MatrixFactory
    {
        private readonly string _filename;
        private int _graphSize;
        public MatrixFactory(string filename)
        {
            _filename = filename;
            if (filename.Length <= 4 || filename[^4] != '.') this._filename += ".csv";
            if (!CheckFileIntegrity(_filename))
            {
                NewFieldGenerator.GenerateNewField(_filename);
                Console.WriteLine($"New file \"{_filename}\" created!");
            }
        }

        public int[][] GetD()
        {
            using (StreamReader sr = new StreamReader(_filename, System.Text.Encoding.Default))
            {
                _graphSize = Int32.Parse(sr.ReadLine());
                int[][] D = new int[_graphSize][];
                for (int i = 0; i < _graphSize; i++)
                {
                    D[i] = new int[_graphSize];
                    String[] strDistances = sr.ReadLine().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < _graphSize; j++) D[i][j] = Int32.Parse(strDistances[j]);
                }
                return D;
            }
        }

        public double[][] GetT()
        {
            double[][] T = new double[_graphSize][];
            Random rand = new();
            for (int i = 0; i < _graphSize; i++)
            {
                T[i] = new double[_graphSize];
                for (int j = 0; j < _graphSize; j++)
                {
                    if (i == j) T[i][j] = 0;
                    else T[i][j] = /*Convert.ToDouble(rand.Next(1, 4)) / 10*/3;
                }
            }

            return T;
        }

        private static bool CheckFileIntegrity(string filename)
        {
            if (!File.Exists(filename)) return false;
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                int n;
                if (!int.TryParse(sr.ReadLine(), out n)) return false;
                int ctr = 0;
                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    ctr++;
                    if (data.Length < n || ctr > n) return false;
                    int test;
                    foreach (String num in data)
                    {
                        if (!int.TryParse(num, out test)) return false;
                    }
                }
            }
            return true;
        }

        public static T[][] Copy<T>(T[][] original)
        {
            T[][] copy = new T[original.Length][];
            for (int i = 0; i<original.Length; i++)
            {
                copy[i] = new T[original[0].Length];
                for (int j = 0; j < original[0].Length; j++) copy[i][j] = original[i][j];
            }
            return copy;
        }
    }
}