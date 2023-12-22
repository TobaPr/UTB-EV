using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB_EV2023
{
    class Particle
    {
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }
        public double Fitness { get; set; }
        public double[] PersonalBestPosition { get; set; }
        public double PersonalBestFitness { get; set; }
    }
    class PSO
    {
        static Random random = new Random();
        static double lowerBound = -100;
        static double upperBound = 100;

      
        public static double GetResult(int dim, Func<double[], double> function)
        {
            // Velikost populace pro různé dimenze
            int[] populationSizes = { 10, 20, 50 };
            int swarmSize = GetSwarmSize(dim, populationSizes);
            // FES = 2000 * počet dimenzí
            int maxFES = 2000 * dim;

            Particle[] swarm = InitializeSwarm(dim, function);

            for (int FES = 0; FES < maxFES; FES+=swarmSize)
            {
                UpdateSwarm(swarm, function);
            }

            Particle globalBestParticle = GetGlobalBestParticle(swarm);

            return globalBestParticle.Fitness;
        }

        static Particle[] InitializeSwarm(int dim, Func<double[], double> function)
        {
            int[] swarmSizes = { 10, 20, 50 };
            int swarmSize = GetSwarmSize(dim, swarmSizes);

            Particle[] swarm = new Particle[swarmSize];

            for (int i = 0; i < swarmSize; i++)
            {
                double[] initialPosition = GenerateRandomPosition(dim);
                double[] initialVelocity = GenerateRandomVelocity(dim);

                double fitness = function(initialPosition);

                swarm[i] = new Particle
                {
                    Position = initialPosition,
                    Velocity = initialVelocity,
                    Fitness = fitness,
                    PersonalBestPosition = initialPosition,
                    PersonalBestFitness = fitness
                };
            }

            return swarm;
        }

        static void UpdateSwarm(Particle[] swarm, Func<double[], double> function)
        {
            foreach (var particle in swarm)
            {
                UpdateVelocity(particle, swarm);
                UpdatePosition(particle);

                double currentFitness = function(particle.Position);

                if (currentFitness < particle.PersonalBestFitness)
                {
                    particle.PersonalBestFitness = currentFitness;
                    particle.PersonalBestPosition = (double[])particle.Position.Clone();
                }
            }
        }

        static void UpdateVelocity(Particle particle, Particle[] swarm)
        {
            
            // dle zadání
            double c1 = 1.49618;
            double c2 = 1.49618;
            double w = 0.7298;

            Particle globalBestParticle = GetGlobalBestParticle(swarm);

            for (int i = 0; i < particle.Velocity.Length; i++)
            {
                double r1 = random.NextDouble();
                double r2 = random.NextDouble();

                particle.Velocity[i] = w * particle.Velocity[i] +
                                       c1 * r1 * (particle.PersonalBestPosition[i] - particle.Position[i]) +
                                       c2 * r2 * (globalBestParticle.Position[i] - particle.Position[i]);
            }
        }

        static void UpdatePosition(Particle particle)
        {
            for (int i = 0; i < particle.Position.Length; i++)
            {
                particle.Position[i] += particle.Velocity[i];

                // Omezení hodnot na rozsah [-100, 100] odrazem
                if (particle.Position[i] < lowerBound || particle.Position[i] > upperBound)
                {
                    particle.Velocity[i] = -particle.Velocity[i]; // Obrat 
                    particle.Position[i] = ClampValue(particle.Position[i], lowerBound, upperBound);
                }
            }
        }

        static double ClampValue(double value, double min, double max)
        {
            if (value < min)
            {
                return min + Math.Abs(min - value);
            }
            else if (value > max)
            {
                return max - Math.Abs(max - value);
            }
            return value;
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

        static double[] GenerateRandomVelocity(int dim)
        {
            double[] velocity = new double[dim];

            for (int i = 0; i < dim; i++)
            {
                velocity[i] = random.NextDouble() * (upperBound - lowerBound) + lowerBound;
            }

            return velocity;
        }

        static Particle GetGlobalBestParticle(Particle[] swarm)
        {
            Particle globalBest = swarm[0];

            foreach (var particle in swarm)
            {
                if (particle.PersonalBestFitness < globalBest.PersonalBestFitness)
                {
                    globalBest = particle;
                }
            }

            return globalBest;
        }

        static int GetSwarmSize(int dimension, int[] sizes)
        {
            if (dimension <= sizes.Length)
                return sizes[dimension - 1];
            else
                return sizes[sizes.Length - 1];
        }
    }
}
