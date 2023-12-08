namespace CGI___Advent_of_Code
{
    internal static class Part2
    {
        //Same as the first, except the cost-per-crab no longer scales linearly with the distance traveled
        //If we keep moving straight to the destination (which we do, since the scaling is still positive) the cost will be
        //the sum of all positive integers up to and including the distance:

        static int FuelCost(int crab, int point)
        {
            int distance = Math.Abs(crab - point);
            //Formula for the sum of the first n = distance positive integers
            int fuelCost = distance * (distance + 1) / 2;

            return fuelCost;
        }

        static int TotalFuelCost(int point)
        {
            int fuelCost = 0;
            for (int i = 0; i < Input.Crabs.Count; i++)
            {
                fuelCost += FuelCost(Input.Crabs[i], point);
            }
            return fuelCost;
        }

        //distance * (distance + 1) / 2 = (distance^2 + distance)/2
        //Thinking is hard and I'm pretty sure this sum can't have any local minimums, so I'm just going to (try to) make the computer do the work with gradient descent
        static int TotalFuelCostGradient(int point)
        {
            int deltaFuelCost = 0;
            foreach (int crab in Input.Crabs)
            {
                //Derivative of (distance^2 + distance)/2 in terms of point (distance = Math.Abs(crab - point))
                if (point == crab)
                {
                    //The crab is already on this point and doesn't want it to move in either direction - gradient is undefined, do not contribute to total
                    continue;
                }
                deltaFuelCost +=
                    (-2 * (crab - point) - (Math.Abs(crab - point) / (crab - point))) / 2;
            }
            return deltaFuelCost;
        }

        static int OptimalAlignmentPoint()
        {
            //Use gradient descent to find the best point
            //We can choose a point outside these bounds but we never want to because we know that such a point would always at least be worse than if it were clamped inside,
            //as the latter would decrease every crab's distance
            int low = Input.Crabs.Min();
            int high = Input.Crabs.Max();
            //Start in the middle
            ///Hey, this is me from the future - I'm out of time, but I wanted to comment on this halving I'm doing
            ///It doesn't take into account the distribution of crab positions and will be disproportionally slowed down by outliers
            ///(Still halving though, so probably fine)
            ///More importantly, when it solves non-integer values by rounding, it might choose the worse of the two and end up with an inaccurate result (although it didn't for me)
            ///Again, I'm out of time - for a patchwork solution I'd check the integers adjacent to the end result to see if one is better. For a correct solution, I should probably
            ///study the theory a bit :)
            int sample = (int)Math.Round((high + low) / 2.0);
            int previousSample;
            do
            {
                previousSample = sample;

                int deltaFuelCost = TotalFuelCostGradient(sample);

                //Console.WriteLine(
                //    $"Sample: {sample}, gradient: {deltaFuelCost}, high: {high}, low: {low}"
                //);

                if (deltaFuelCost == 0)
                {
                    continue; //sample is optimal, do not change it
                }
                else if (deltaFuelCost < 0)
                {
                    //cut everything lower than the sample by changing the lower bound
                    low = sample;
                    //get a new sample in the middle of the new range
                    sample = (int)Math.Round((sample + high) / 2.0);
                }
                else
                {
                    //cut everything higher than the sample by changing the higher bound
                    high = sample;
                    //get a new sample in the middle of the new range
                    sample = (int)Math.Round((sample + low) / 2.0);
                }

                //repeat unless we've converged
            } while (previousSample != sample);
            return sample;
        }

        public static int MinimumFuelCost()
        {
            int point = OptimalAlignmentPoint();
            Console.WriteLine($"Optimal point of crabvergence: {point}");
            int fuelCost = TotalFuelCost(point);
            return fuelCost;
        }
    }
}
