using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    class DE_rand
    {
        static Random random = new Random();

        public static double GetResult(int dim, Func<double[], double> function)
        {
            // Parametry DE
            double F = 0.8;
            double CR = 0.9;

            // Rozsah hodnot
            double lowerBound = -100;
            double upperBound = 100;

            // Velikost populace pro různé dimenze
            int[] populationSizes = { 10, 20, 50 };
           

            // Inicializace populace
            int populationSize = GetPopulationSize(dim, populationSizes);
            double[][] population = InitializePopulation(populationSize, dim, lowerBound, upperBound);

            // FES = 2000 * počet dimenzí
            int maxFES = 2000 * dim;

            // Nejlepší řešení
            double best = 0;


            // Hlavní cyklus evoluce
            for (int FES = 0; FES < maxFES; FES+=populationSize)
            {
                for (int i = 0; i < populationSize; i++)
                {
                    // Generování náhodných indexů pro výběr jednotlivců
                    int r1, r2, r3;
                    do
                    {
                        r1 = random.Next(populationSize);
                    } while (r1 == i);

                    do
                    {
                        r2 = random.Next(populationSize);
                    } while (r2 == i || r2 == r1);

                    do
                    {
                        r3 = random.Next(populationSize);
                    } while (r3 == i || r3 == r1 || r3 == r2);

                    // DE/rand/1/bin
                    for (int j = 0; j < dim; j++)
                    {
                        if (random.NextDouble() < CR || j == random.Next(dim))
                            population[i][j] = population[r1][j] + F * (population[r2][j] - population[r3][j]);
                    }

                    // Omezení hodnot na rozsah [-100, 100]
                    ClampValues(population[i], lowerBound, upperBound);
                }

                // Evaluace fitness hodnot
                double[] fitnessValues = EvaluatePopulation(population,function);

                // Najdi nejlepšího jedince
                int bestIndex = Array.IndexOf(fitnessValues, fitnessValues.Min());
                

                if (FES == 0) {
                    best = fitnessValues[bestIndex]; //trošku debilní, ale co už....
                }
                

                if (fitnessValues[bestIndex] < best) {
                    best = fitnessValues[bestIndex];
                }
                
            }
       
           
            return best;
        }

        static int GetPopulationSize(int dimension, int[] sizes)
        {
            if (dimension <= sizes.Length)
                return sizes[dimension - 1];
            else
                return sizes[sizes.Length - 1];
        }

        static double[][] InitializePopulation(int size, int dimension, double lowerBound, double upperBound)
        {
            double[][] population = new double[size][];
            for (int i = 0; i < size; i++)
            {
                population[i] = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    population[i][j] = random.NextDouble() * (upperBound - lowerBound) + lowerBound;
                }
            }
            return population;
        }

        static void ClampValues(double[] vector, double min, double max)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] < min || vector[i] > max)
                {
                    // Odrážení od hranic
                    double range = max - min;
                    double overflow = Math.Abs(vector[i] - min) % (2 * range);
                    vector[i] = (overflow <= range) ? min + overflow : max - overflow;
                }
            }
        }

        static double[] EvaluatePopulation(double[][] population, Func<double[], double> function)
        {
            double[] fitnessValues = new double[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                //fitnessValues[i] = RastriginFunction(population[i]);
                fitnessValues[i] = EvaluateFunction(population[i], function);
            }
            return fitnessValues;
        }


        // Metoda pro vyhodnocení funkce pro zadané hodnoty
        static double EvaluateFunction(double[] values, Func<double[], double> function)
        {
            return function(values);
        }


    }
}
