using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    class Individual
    {
        public double[] Position { get; set; }
        public double Fitness { get; set; }
    }

    class SOMA_ALL_TO_ONE
    {
        static Random random = new Random();
        static double lowerBound = -100;
        static double upperBound = 100;


        public static double GetResult(int dim, Func<double[], double> function)
        {

        
            int maxEvaluations = 2000 * dim;

            Individual[] population = InitializePopulation(dim, function);
            int iterations = 0;
            while (true)
            {
                Individual[] migrants = SelectMigrants(population, 1); 

               
                foreach (var migrant in migrants)
                {
                    // Vybrat náhodný index destinace (vyjma indexu migrant)
                    int destinationIndex = random.Next(0, population.Length - 1);
                    if (destinationIndex >= Array.IndexOf(population, migrant))
                        destinationIndex++; // Přeskočit vlastní index migrant

                    // Aktualizovat pozici migranta na základě destinace s uvažováním parametrů
                    UpdatePositionBasedOnDestination(migrant, population[destinationIndex]);

                    // kontrola hranic odrazem
                    ApplyReflectionToBounds(population[destinationIndex].Position);
                }

                // Evaluate fitness of the updated population
                EvaluatePopulationFitness(population, function);
                iterations += population.Length;


                if (iterations >= maxEvaluations) 
                { 
                    break; 
                }
               
            }

            // Get the best individual from the final population
            Individual bestIndividual = GetBestIndividual(population);

            return bestIndividual.Fitness;
        }


        static void UpdatePositionBasedOnDestination(Individual migrant, Individual destination)
        {
            double stepSize = 0.11;
            double prt = 0.7;

            // Aktualizovat pozici migranta na základě destinace s uvažováním parametrů
            for (int i = 0; i < migrant.Position.Length; i++)
            {
                // Použít parametry PathLength, StepSize a PRT
                double pathLength = 3.0;
                double randomValue = random.NextDouble();
                double distance = stepSize * pathLength * (2 * randomValue - 1); // Implementace pro zahrnutí kroku a délky cesty

                // Aktualizovat souřadnici migranta na základě destinace a náhodného kroku s uvažováním parametru PRT
                migrant.Position[i] = destination.Position[i] + prt * distance;
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

   

        static Individual[] SelectMigrants(Individual[] population, int numMigrants)
        {
            // Select individuals randomly for migration
            return population.OrderBy(_ => random.Next()).Take(numMigrants).ToArray();
        }


        static Individual GetBestIndividual(Individual[] population)
        {
            // Find and return the individual with the best fitness in the population
            return population.OrderBy(individual => individual.Fitness).First();
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

        static int GetPopSize(int dimension, int[] sizes)
        {
            if (dimension <= sizes.Length)
                return sizes[dimension - 1];
            else
                return sizes[sizes.Length - 1];
        }
    }
}
