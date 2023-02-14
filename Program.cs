using System;

namespace MonteCarloAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            const int numberOfPlans = 10000; // Number of plans to generate

            Console.WriteLine("How many tasks are in the project?");
            int numberOfTasks = int.Parse(Console.ReadLine());

            int[][] estimates = new int[numberOfTasks][]; // Initialize array for task estimates

            // Prompt user to input task estimates
            for (int i = 0; i < numberOfTasks; i++)
            {
                Console.WriteLine($"Enter estimates for task #{i + 1} (best, worst, average):");
                string input = Console.ReadLine();
                string[] inputs = input.Split(' ');
                estimates[i] = new int[inputs.Length];

                for (int j = 0; j < inputs.Length; j++)
                {
                    estimates[i][j] = int.Parse(inputs[j]);
                }
            }

            // Initialize array to store completion times for each plan
            int[] completionTimes = new int[numberOfPlans];

            // Generate completion times for each plan using the Monte Carlo algorithm
            Random random = new Random();
            for (int i = 0; i < numberOfPlans; i++)
            {
                int completionTime = 0;
                for (int j = 0; j < numberOfTasks; j++)
                {
                    // Generate a random completion time for the task based on the estimates
                    int bestCase = estimates[j][0];
                    int worstCase = estimates[j][1];
                    int averageCase = estimates[j][2];

                    double probability = random.Next(0, 101) / 100.0; // Generate a random probability between 0 and 1
                    int completionTimeForTask = (int)Math.Round((bestCase + 4 * averageCase + worstCase) / 6.0);
                    completionTimeForTask += (int)Math.Round(probability * (worstCase - bestCase) / 6.0);

                    completionTime += completionTimeForTask; // Add the completion time for the task to the total completion time
                }

                completionTimes[i] = completionTime; // Store the completion time for the plan
            }

            // Calculate the minimum, maximum, and average completion times for the project
            int minimum = completionTimes[0];
            int maximum = completionTimes[0];
            double average = 0;
            for (int i = 0; i < numberOfPlans; i++)
            {
                minimum = Math.Min(minimum, completionTimes[i]);
                maximum = Math.Max(maximum, completionTimes[i]);
                average += completionTimes[i];
            }
            average /= numberOfPlans;

            // Group the completion times into buckets
            int numberOfBuckets = 10; // Number of buckets to use
            int bucketSize = (maximum - minimum) / numberOfBuckets;
            Dictionary<int, int> bucketCounts = new Dictionary<int, int>();
            for (int i = 0; i < numberOfPlans; i++)
            {
                int bucket = (completionTimes[i] - minimum) / bucketSize;
                if (bucketCounts.ContainsKey(bucket))
                {
                    bucketCounts[bucket]++;
                }
                else
                {
                    bucketCounts[bucket] = 1;
                }
            }

            // Display the results
            Console.WriteLine("After probing 10000 random plans, the results are:");

            Console.WriteLine($"Minimum = {minimum}");
            Console.WriteLine($"Maximum = {maximum}");
            Console.WriteLine($"Average = {average}");

            double accumulatedProbability = 0;
            foreach (int bucket in bucketCounts.Keys)
            {
                double bucketStart = minimum + bucket * bucketSize;
                double bucketEnd = bucketStart + bucketSize;
                double probability = bucketCounts[bucket] / (double)numberOfPlans;
                accumulatedProbability += probability;

                Console.WriteLine($"Probability of finishing between {bucketStart} and {bucketEnd}: {probability * 100}% Accumulated probability: {accumulatedProbability * 100}%");
            }
        }
    }
}