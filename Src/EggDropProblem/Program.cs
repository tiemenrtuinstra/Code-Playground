namespace EggDropProblem;

class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
        bool redo;
        do
        {
            redo = false;
            Console.WriteLine("Choose the problem to solve:");
            Console.WriteLine("1. Two Egg Drop Problem");
            Console.WriteLine("2. Multi Egg Multi Floor Egg Drop Problem");

            // Read and validate user choice
            if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                redo = true;
                continue;
            }

            // Variables to store the number of floors and eggs
            int numberOfFloors;
            int numberOfEggs;

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the number of floors:");
                    if (!int.TryParse(Console.ReadLine(), out numberOfFloors) || numberOfFloors <= 0)
                    {
                        Console.WriteLine("Invalid number of floors. Please enter a positive integer.");
                        redo = true;
                        continue;
                    }

                    TwoEggDropProblem(numberOfFloors);
                    break;

                case 2:
                    Console.WriteLine("Enter the number of floors:");
                    if (!int.TryParse(Console.ReadLine(), out numberOfFloors) || numberOfFloors <= 0)
                    {
                        Console.WriteLine("Invalid number of floors. Please enter a positive integer.");
                        redo = true;
                        continue;
                    }

                    Console.WriteLine("Enter the number of eggs:");
                    if (!int.TryParse(Console.ReadLine(), out numberOfEggs) || numberOfEggs <= 0)
                    {
                        Console.WriteLine("Invalid number of eggs. Please enter a positive integer.");
                        redo = true;
                        continue;
                    }

                    MultiEggMultiFloorEggDropProblem(numberOfFloors, numberOfEggs);
                    break;
            }

            // Ask if the user wants to redo
            Console.WriteLine("Do you want to solve another problem? (y/n)");
            string redoChoice = Console.ReadLine().Trim().ToLower();
            if (redoChoice == "y" || redoChoice == "yes")
            {
                redo = true;
            }

        } while (redo);

        // Press any key to exit
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    /// <summary>
    /// Solves the Two Egg Drop Problem and displays the results.
    /// </summary>
    /// <param name="numberOfFloors">The number of floors in the building.</param>
    static void TwoEggDropProblem(int numberOfFloors)
    {
        // Call the TwoEggDropWithFloor method and get the path
        var result = EggDrop.TwoEggDropWithFloor(numberOfFloors);

        // Display the results
        Console.WriteLine($"Minimum number of trials in worst case with 2 eggs and {numberOfFloors} floors is {result.MinTrials}");
        Console.WriteLine("Floors to drop the eggs from (in order):");
        foreach (int floor in result.Floors)
        {
            Console.WriteLine(floor);
        }
    }

    /// <summary>
    /// Solves the Multi Egg Multi Floor Egg Drop Problem and displays the results.
    /// </summary>
    /// <param name="numberOfFloors">The number of floors in the building.</param>
    /// <param name="numberOfEggs">The number of eggs available.</param>
    static void MultiEggMultiFloorEggDropProblem(int numberOfFloors, int numberOfEggs)
    {
        // Call the EggDropDP method and get the path
        List<int> path;
        int minTrials = EggDrop.EggDropDP(numberOfFloors, numberOfEggs, out path);

        // Display the results
        Console.WriteLine($"Minimum number of trials in worst case with {numberOfEggs} eggs and {numberOfFloors} floors is {minTrials}");
        Console.WriteLine("Floors to drop the eggs from (in order):");
        foreach (int floor in path)
        {
            Console.WriteLine(floor);
        }
    }
}
