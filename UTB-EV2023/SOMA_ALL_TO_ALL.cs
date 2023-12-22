using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    
    class SOMA_ALL_TO_ALL
    {
        static Random random = new Random();
        static double lowerBound = -100;
        static double upperBound = 100;
  

        public static double GetResult(int dim, Func<double[], double> function)
        {

            int maxEvaluations = 2000 * dim;

            Individual[] population = InitializePopulation(dim,function);

            int iterations = 0;

            while (true)
            {
                
                EvaluatePopulationFitness(population,function);

                MigrateAllToAll(population);

                iterations += population.Length; // Přidáme počet ohodnocení v této iteraci

                if (iterations >= maxEvaluations)
                {
                    break;
                }

            }

            // Get the best individual from the final population
            Individual bestIndividual = GetBestIndividual(population);

            return bestIndividual.Fitness;
        }


        static void MigrateAllToAll(Individual[] population)
        {
            foreach (var migrant in population)
            {
                
                int destinationIndex = random.Next(0, population.Length - 1);
                if (destinationIndex >= Array.IndexOf(population, migrant))
                    destinationIndex++; // Skip migrant's own index

                // změna pozice
                UpdatePositionBasedOnDestination(migrant, population[destinationIndex]);

                // kontrola hranic
                ApplyReflectionToBounds(migrant.Position);
            }
        }

        static void UpdatePositionBasedOnDestination(Individual migrant, Individual destination)
        {
            // Aktualizovat pozici migranta na základě destinace s uvažováním parametrů
            for (int i = 0; i < migrant.Position.Length; i++)
            {
                // Použít parametry PathLength, StepSize a PRT
                double pathLength = 3.0;
                double stepSize = 0.11;
                double prt = 0.7;
                double randomValue = random.NextDouble();
                double distance = stepSize * pathLength * (2 * randomValue - 1); // Implementace pro zahrnutí kroku a délky cesty

                // Aktualizovat souřadnici migranta na základě destinace a náhodného kroku
                migrant.Position[i] = destination.Position[i] + prt * distance;
            }
        }

        static Individual GetBestIndividual(Individual[] population)
        {
            // Find and return the individual with the best fitness in the population
            return population.OrderBy(individual => individual.Fitness).First();
        }

        static void ApplyReflectionToBounds(double[] position)
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i] < lowerBound)
                {
                    position[i] = 2 * lowerBound - position[i];
                }
                else if (position[i] > upperBound)
                {
                    position[i] = 2 * upperBound - position[i];
                }
            }
        }

        static Individual[] InitializePopulation(int dim, Func<double[], double> function)
        {
            // Velikost populace pro různé dimenze
            int[] populationSizes = { 10, 20, 50 };
            int populationSize = GetPopSize(dim, populationSizes);
            Individual[] population = new Individual[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                double[] initialPosition = GenerateRandomPosition(dim);
                population[i] = new Individual { Position = initialPosition };
            }

            EvaluatePopulationFitness(population, function);

            return population;
        }

        static void EvaluatePopulationFitness(Individual[] population, Func<double[], double> function)
        {
            foreach (var individual in population)
            {
                individual.Fitness = function(individual.Position);
            }
        }


        static int GetPopSize(int dimension, int[] sizes)
        {
            if (dimension <= sizes.Length)
                return sizes[dimension - 1];
            else
                return sizes[sizes.Length - 1];
        }

        static double[] GenerateRandomPosition(int dim)
        {
            double[] position = new double[dim];

            for (int i = 0; i < dim; i++)
            {
                position[i] = random.NextDouble() * (upperBound - lowerBound) + lowerBound;
            }

            return position;
        }


    }
}

