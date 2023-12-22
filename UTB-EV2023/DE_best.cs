using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    class DE_best
    {
       
        static Random random = new Random();

        public static double GetResult(int dim, Func<double[], double> function)
        {
            double lowerBound = -100;
            double upperBound = 100;

            // Velikost populace pro různé dimenze
            int[] populationSizes = { 10, 20, 50 };

            // FES = 2000 * počet dimenzí
            int maxFES = 2000 * dim;

            // Inicializace populace
            int populationSize = GetPopulationSize(dim, populationSizes);
            double[][] population = InitializePopulation(populationSize, dim, lowerBound, upperBound);

            


            double[] bestSolution = population[0];
            double bestFitness = function(bestSolution);

            for (int FES = 0; FES < maxFES; FES+=populationSize)
            {
                for (int i = 0; i < populationSize; i++)
                {
                    double[] trialVector = CreateTrialVectorBest(population, i, dim, function);
                    double trialFitness = function(trialVector);
                    double currentFitness = function(population[i]);

                    if (trialFitness < bestFitness)
                    {
                        bestFitness = trialFitness;
                        bestSolution = trialVector;
                    }

                    if (trialFitness < currentFitness)
                    {
                        for (int j = 0; j < dim; j++)
                        {
                            population[i][j] = trialVector[j];
                        }
                    }
                }
            }

            return bestFitness;

        }

        static double[] CreateTrialVectorBest(double[][] population, int currentIndex, int dim, Func<double[], double> function)
        {
            double[] currentIndividual = population[currentIndex];
            double[] bestIndividual = GetBestIndividual(population, function);

            double F = 0.5;
            double CR = 0.9;

            double[] trialVector = new double[dim];

            for (int j = 0; j < dim; j++)
            {
                if (random.NextDouble() < CR || j == random.Next(dim))
                    trialVector[j] = bestIndividual[j] + F * (currentIndividual[j] - bestIndividual[j]);
                else
                    trialVector[j] = currentIndividual[j];
            }

            ClampValues(trialVector, -100, 100);

            return trialVector;
        }

        static double[] GetBestIndividual(double[][] population, Func<double[], double> function)
        {
            int bestIndex = 0;
            double bestFitness = function(population[0]);

            for (int i = 1; i < population.Length; i++)
            {
                double currentFitness = function(population[i]);
                if (currentFitness < bestFitness)
                {
                    bestFitness = currentFitness;
                    bestIndex = i;
                }
            }

            return population[bestIndex];
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




    }
}
