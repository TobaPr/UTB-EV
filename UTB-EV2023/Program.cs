using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Začínám: ");

            // seznam funkcí
            List<Func<double[], double>> functions = new List<Func<double[], double>>
        {
            RastriginFunction,
            RosenbrockFunction,
            SphereFunction,
            SchwefelFunction,
            MichalewiczFunction,
            StyblinskiTangFunction,
            Alpine01Function, 
            SumOfDifferentPowersFunction, 
            DixonPriceFunction,
            CosineMixtureFunction,
            Mishra07Function,
            Mishra11Function,
            PlateauFunction,
            QingFunction,
            RanaFunction,
            TridFunction,
            YaoLiu09Function,
            BentCigarFunction,
            DebsFunctionNo1,
            QuarticFunction,
            HyperEllipsoidFunction,
            EggHolderFunction,
            ChungReynoldsFunction,
            MovedAxisParallelHyperEllipsoidFunction,
            GeneralizedSchwefelFunctionNo226,

            // Přidejte další funkce podle potřeby
        };

            // počet opakování. V zadání 30
           int opakovani = 30;

           int[] dimensions = { 2, 10, 30 };

            foreach (var dim in dimensions)
            {
                Console.WriteLine($"\n--------------------------------- Dimenze: {dim} ---------------------------------");
                int counter = 1; 
                foreach (var function in functions)
                {
                    Console.WriteLine($"\nD{dim} - {function.Method.Name}({counter})");

                    double best_DE_rand = DE_rand.GetResult(dim, function); 
                    double best_DE_best = DE_best.GetResult(dim, function);
                    double best_PSO = PSO.GetResult(dim, function);
                    double best_SOMA1 = SOMA_ALL_TO_ONE.GetResult(dim, function);
                    double best_SOMA_ALL = SOMA_ALL_TO_ALL.GetResult(dim, function);

                    // Pouštíme dle počtu opakování a bereme nejlepší výsledek

                    // opakování DE_RAND  
                    for (int i = 1; i < opakovani; i++)
                    {
                        double x = DE_rand.GetResult(dim, function);
                        if (x < best_DE_rand)
                        {
                            best_DE_rand = x;
                        }
                    }
                    Console.WriteLine($"\tDE_R \t{best_DE_rand}");

                    // opakování DE_Best
                    for (int i = 1; i < opakovani; i++)
                    {
                        double x = DE_best.GetResult(dim, function);
                        if (x < best_DE_best)
                        {
                            best_DE_best = x;
                        }
                    }
                    Console.WriteLine($"\tDE_B \t{best_DE_best}");

                    // opakování PSO
                    for (int i = 1; i < opakovani; i++)
                    {
                        double x = PSO.GetResult(dim, function);
                        if (x < best_PSO)
                        {
                            best_PSO = x;
                        }
                    }
                    Console.WriteLine($"\tPSO \t{best_PSO}");

                    // opakování SOMA1
                    for (int i = 1; i < opakovani; i++)
                    {
                        double x = SOMA_ALL_TO_ONE.GetResult(dim, function);
                        if (x < best_SOMA1)
                        {
                            best_SOMA1 = x;
                        }
                    }
                    Console.WriteLine($"\tS_ONE \t{best_SOMA1}");

                    // opakování SOMA_ALL
                    for (int i = 1; i < opakovani; i++)
                    {
                        double x = SOMA_ALL_TO_ALL.GetResult(dim, function);
                        if (x < best_SOMA_ALL)
                        {
                            best_SOMA_ALL = x;
                        }
                    }
                    Console.WriteLine($"\tS_ALL \t{best_SOMA_ALL}");

                    counter++; 
                }
            }
            Console.WriteLine("Dokončeno :)");
            Console.ReadLine();
        }

        public static double SphereFunction(double[] x)
        {
            return x.Sum(xi => xi * xi);
        }
        public static double RastriginFunction(double[] x)
        {
            double A = 10;
            int n = x.Length;
            double sum = 0;

            foreach (var xi in x)
            {
                sum += xi * xi - A * Math.Cos(2 * Math.PI * xi);
            }

            return A * n + sum;
        }
        static double RosenbrockFunction(double[] x)
        {
            double a = 1.0; // Konstanta a
            double b = 100.0; // Konstanta b
            double sum = 0.0;

            for (int i = 0; i < x.Length - 1; i++)
            {
                double term1 = Math.Pow(a - x[i], 2);
                double term2 = b * Math.Pow(x[i + 1] - x[i] * x[i], 2);
                sum += term1 + term2;
            }
            return sum;
        }
        static double SchwefelFunction(double[] x)
        {
            double sum = 0.0;
            int dimension = x.Length;

            for (int i = 0; i < dimension; i++)
            {
                sum += -x[i] * Math.Sin(Math.Sqrt(Math.Abs(x[i])));
            }

            double result = 418.9829 * dimension + sum;
            return result;
        }
        static double MichalewiczFunction(double[] x)
        {
            double sum = 0.0;
            int dimension = x.Length;
            double m = 10.0; // Parametr m pro Michalewiczovu funkci

            for (int i = 0; i < dimension; i++)
            {
                double xi = x[i];
                sum += Math.Sin(xi) * Math.Pow(Math.Sin(((i + 1) * xi * xi) / Math.PI), 2 * m);
            }

            return -sum; // Vracíme zápornou hodnotu, protože hledáme minimum funkce
        }
        static double StyblinskiTangFunction(double[] x)
        {
            double sum = 0.0;
            int dimension = x.Length;

            for (int i = 0; i < dimension; i++)
            {
                double xi = x[i];
                sum += Math.Pow(xi, 4) - 16 * Math.Pow(xi, 2) + 5 * xi;
            }

            return 0.5 * sum; // Vracíme poloviční hodnotu, protože hledáme minimum funkce
        }
        static double Alpine01Function(double[] x)
        {
            double sum = 0.0;
            int dimension = x.Length;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Abs(x[i] * Math.Sin(x[i]) + 0.1 * x[i]);
            }

            return sum;
        }
        static double SumOfDifferentPowersFunction(double[] x)
        {
            double sum = 0.0;
            int dimension = x.Length;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(Math.Abs(x[i]), i + 2);
            }

            return sum;
        }
        static double DixonPriceFunction(double[] x)
        {
            double result = Math.Pow(x[0] - 1, 2);

            for (int i = 1; i < x.Length; i++)
            {
                double term = i * (2 * Math.Pow(x[i], 2) - x[i - 1]);
                result += Math.Pow(term, 2);
            }

            return result;
        }
        static double CosineMixtureFunction(double[] x)
        {
            int dimension = x.Length;

            double sum1 = 0.0;
            for (int i = 0; i < dimension; i++)
            {
                sum1 += Math.Cos(5.0 * Math.PI * x[i]);
            }

            double sum2 = 0.0;
            for (int i = 0; i < dimension; i++)
            {
                sum2 += Math.Pow(x[i], 2);
            }
            return 0.1 * sum1 - sum2;
        }
        static double Mishra07Function(double[] x)
        {
            int dimension = x.Length;
            double product = 1.0;

            for (int i = 0; i < dimension; i++)
            {
                product *= x[i];
            }

            int factorialN = Factorial(dimension);

            return Math.Pow(product - factorialN, 2);
        }
        static int Factorial(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }
        static double Mishra11Function(double[] x)
        {
            int dimension = x.Length;

            double sum1 = 0.0;
            double product = 1.0;

            for (int i = 0; i < dimension; i++)
            {
                sum1 += Math.Abs(x[i]);
                product *= Math.Abs(x[i]);
            }

            double sum2 = Math.Pow(product, 1.0 / dimension);

            return Math.Pow((sum1 / dimension) - sum2, 2);
        }
        static double PlateauFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Abs(x[i]);
            }

            return 30.0 + sum;
        }
        static double QingFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(x[i] * x[i] - (i + 1), 2);
            }

            return sum;
        }
        static double RanaFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension - 1; i++)
            {
                double term1 = x[i] * Math.Sin(Math.Sqrt(Math.Abs(x[i + 1] + 1 + x[i]))) * Math.Cos(Math.Sqrt(Math.Abs(x[i] - x[i + 1] + 1)));
                double term2 = (x[i + 1] + 1) * Math.Cos(Math.Sqrt(Math.Abs(x[i] - x[i + 1] + 1)));
                sum += term1 + term2;
            }

            return sum;
        }
        static double TridFunction(double[] x)
        {
            int dimension = x.Length;
            double sum1 = 0.0;
            double sum2 = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum1 += Math.Pow(x[i] - 1, 2);
            }

            for (int i = 1; i < dimension; i++)
            {
                sum2 += x[i - 1] * x[i];
            }

            return sum1 - sum2;
        }
        static double YaoLiu09Function(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(x[i], 2) - 10 * Math.Cos(2 * Math.PI * x[i]) + 10;
            }

            return sum;
        }
        static double BentCigarFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 2; i < dimension; i++)
            {
                sum += Math.Pow(x[i], 2);
            }

            return Math.Pow(x[0], 2) + 1e6 * sum;
        }
        static double DebsFunctionNo1(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(Math.Sin(5 * Math.PI * x[i]), 2);
            }

            return sum;
        }
        static double QuarticFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += (i + 1) * Math.Pow(x[i], 4);
            }

            return sum;
        }
        static double HyperEllipsoidFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(x[i], 2);
            }

            return sum;
        }
        static double EggHolderFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension - 1; i++)
            {
                double term1 = -x[i] * Math.Sin(Math.Sqrt(Math.Abs(x[i] - x[i + 1] - 47)));
                double term2 = -(x[i + 1] + 47) * Math.Sin(Math.Sqrt(Math.Abs(0.5 * x[i] + x[i + 1] + 47)));

                sum += term1 + term2;
            }

            return sum;
        }
        static double ChungReynoldsFunction(double[] x)
        {
            int dimension = x.Length;
            double sumOfSquares = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sumOfSquares += Math.Pow(x[i], 2);
            }

            return Math.Pow(sumOfSquares, 2);
        }
        static double MovedAxisParallelHyperEllipsoidFunction(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum += Math.Pow(i * (x[i] - 5 * i), 2);
            }

            return sum;
        }
        static double GeneralizedSchwefelFunctionNo226(double[] x)
        {
            int dimension = x.Length;
            double sum = 0.0;

            for (int i = 0; i < dimension; i++)
            {
                sum -= x[i] * Math.Sin(Math.Sqrt(Math.Abs(x[i])));
            }

            return sum;
        }

    }
}
