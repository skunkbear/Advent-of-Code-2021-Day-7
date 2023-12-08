using MathNet.Numerics.Statistics;

namespace CGI___Advent_of_Code
{
    internal static class Part1
    {
        //Let's get our crabs in here

        //Woah momma that's a lot of crabs
        //Alright, let's analyse the problem

        //We need to choose a point to align all crabs on - whichever one we choose, it will be optimal to move each crab straight to that point
        //As such, we just need to find the optimal point
        //The minimum fuel cost of each crab's alignment scales linearly with its distance from the point we choose to align on
        //Thus, the optimal alignment point is whichever point minimises the sum of each crab's distance to said point
        //Optimising this is a property of the *median*, so:

        public static int MinimumFuelCost()
        {
            //Get the median of all crabs' positions = the optimal point to converge on
            int point = (int)Input.Crabs.ConvertAll(x => (double)x).Median();
            Console.WriteLine($"Optimal point of crabvergence: {point}");

            //Calculate the fuel cost for this point
            int fuelCost = 0;
            for (int i = 0; i < Input.Crabs.Count; i++)
            {
                fuelCost += Math.Abs(Input.Crabs[i] - point);
            }
            return fuelCost;
        }
    }
}
