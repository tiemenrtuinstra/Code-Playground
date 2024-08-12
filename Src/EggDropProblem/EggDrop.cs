namespace EggDropProblem;

public class EggDrop
{
    /// <summary>
    /// Determines the minimum number of trials needed in the worst case to find the critical floor
    /// from which an egg can be dropped without breaking, using two eggs and a given number of floors.
    /// </summary>
    /// <param name="floors">The number of floors in the building.</param>
    /// <returns>
    /// A tuple containing:
    /// - MinTrials: The minimum number of trials needed in the worst case.
    /// - Floors: A list of floors from which the eggs should be dropped to achieve the minimum number of trials.
    /// </returns>
    public static (int MinTrials, List<int> Floors) TwoEggDropWithFloor(int floors)
    {
        // dp[i][j] will represent the minimum number of trials needed for i eggs and j floors.
        int[,] dp = new int[3, floors + 1];
        int[,] floorDecision = new int[3, floors + 1]; // To track decisions

        // Base cases
        for (int i = 1; i <= floors; i++)
        {
            dp[1, i] = i; // With one egg, we need i trials for i floors.
            dp[2, i] = int.MaxValue; // Initialize with a large value for two eggs.
        }

        for (int i = 1; i <= 2; i++)
        {
            dp[i, 0] = 0; // Zero trials for zero floors.
            dp[i, 1] = 1; // One trial for one floor.
        }

        // Fill the rest of the entries in the table using the optimal substructure property
        for (int j = 2; j <= floors; j++)
        {
            for (int x = 1; x <= j; x++)
            {
                // Calculate the result of dropping the egg from floor x.
                int res = 1 + Math.Max(dp[1, x - 1], dp[2, j - x]);
                if (res < dp[2, j])
                {
                    dp[2, j] = res;
                    floorDecision[2, j] = x; // Track the floor decision
                }
            }
        }

        // Backtrack to find the path (floors)
        List<int> path = new List<int>();
        int remainingEggs = 2, remainingFloors = floors;
        while (remainingEggs > 1 && remainingFloors > 0)
        {
            int decisionFloor = floorDecision[remainingEggs, remainingFloors];
            path.Add(decisionFloor);
            if (dp[remainingEggs - 1, decisionFloor - 1] < dp[remainingEggs, remainingFloors - decisionFloor])
            {
                // Egg did not break
                remainingFloors -= decisionFloor;
            }
            else
            {
                // Egg broke
                remainingEggs--;
                remainingFloors = decisionFloor - 1;
            }
        }
        if (remainingFloors > 0) path.Add(remainingFloors); // Handle remaining floors for 1 egg

        return (dp[2, floors], path);
    }

    /// <summary>
    /// Determines the minimum number of trials needed in the worst case to find the critical floor
    /// from which an egg can be dropped without breaking, using a given number of eggs and floors.
    /// </summary>
    /// <param name="floors">The number of floors in the building.</param>
    /// <param name="eggs">The number of eggs available.</param>
    /// <param name="path">The list of floors from which the eggs should be dropped to achieve the minimum number of trials.</param>
    /// <returns>The minimum number of trials needed in the worst case.</returns>
    public static int EggDropDP(int floors, int eggs, out List<int> path)
    {
        // Initialize the dp table where dp[i][j] represents the minimum number of trials needed for j eggs and i floors.
        int[,] dp = new int[floors + 1, eggs + 1];
        // Initialize the decision table to track the floor decisions.
        int[,] decision = new int[floors + 1, eggs + 1];
        int res;

        // Base cases initialization
        // If we have 1 egg, we need to try each floor from 1 to i, hence dp[i, 1] = i.
        for (int i = 1; i <= floors; i++)
        {
            dp[i, 1] = i;
            dp[i, 0] = 0; // Zero trials for zero floors.
        }
        // If we have 1 floor, we need 1 trial, hence dp[1, j] = j.
        for (int j = 1; j <= eggs; j++)
        {
            dp[1, j] = j;
        }

        // Fill the rest of the entries in the table using optimal substructure property
        for (int i = 2; i <= floors; i++)
        {
            for (int j = 2; j <= eggs; j++)
            {
                dp[i, j] = int.MaxValue; // Initialize the current cell to a large value.
                                         // Try dropping the egg from each floor x and calculate the minimum number of trials needed.
                for (int x = 1; x <= i; x++)
                {
                    // Calculate the result of dropping the egg from floor x.
                    res = 1 + Math.Max(dp[x - 1, j - 1], dp[i - x, j]);
                    // If the result is less than the current minimum, update the dp table and decision table.
                    if (res < dp[i, j])
                    {
                        dp[i, j] = res;
                        decision[i, j] = x; // Track the floor decision.
                    }
                }
            }
        }

        // Backtrack to find the path (floors)
        path = new List<int>();
        int remainingEggs = eggs, remainingFloors = floors;
        while (remainingEggs > 1 && remainingFloors > 0)
        {
            int decisionFloor = decision[remainingFloors, remainingEggs];
            path.Add(decisionFloor);
            // Check if the egg did not break.
            if (dp[decisionFloor - 1, remainingEggs - 1] < dp[remainingFloors - decisionFloor, remainingEggs])
            {
                // Egg did not break, reduce the number of floors.
                remainingFloors -= decisionFloor;
            }
            else
            {
                // Egg broke, reduce the number of eggs and set the floor to the decision floor minus one.
                remainingEggs--;
                remainingFloors = decisionFloor - 1;
            }
        }
        if (remainingFloors > 0) path.Add(remainingFloors); // Handle remaining floors for 1 egg.

        return dp[floors, eggs];
    }
}
