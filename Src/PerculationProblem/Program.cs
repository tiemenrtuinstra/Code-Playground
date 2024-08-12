namespace PerculationProblem
{
    class Program
    {
        private const int MaxWaterCells = 0;
        private const int N = 12;

        private static char[,] matrix = GenerateMatrix(N, MaxWaterCells);

        private static UnionFind uf = new UnionFind(N * N + 2);

        private static int virtualTopIndex = N * N; // Virtual top node index
        private static int virtualBottomIndex = N * N + 1; // Virtual bottom node index

        private static int perculationTryCount = 0;
        private static int openGroundCount = MaxWaterCells;
        private static int longestOpenPathLength = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Matrix size: " + N + "x" + N);
            Console.WriteLine("Max water cells: " + MaxWaterCells);
            Console.WriteLine("Perculation tries count: " + perculationTryCount);
            Console.WriteLine("Open ground count: " + openGroundCount);
            Console.WriteLine("Initial matrix state:");
            MakeMatrixPercolateAndTest();
            Console.WriteLine("\n-------------------------\n");


            Console.WriteLine("Perculation tries count: " + perculationTryCount);
            Console.WriteLine("Open ground count: " + openGroundCount);

            // Wait for user input before closing
            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }

        public class Cell
        {
            public bool IsOpen { get; set; }
            public bool IsFull { get; set; }
            public bool CanDrain { get; set; }
            public int Parent { get; set; }
            public int Weight { get; set; }

            public Cell()
            {
                IsOpen = false;
                IsFull = false;
                CanDrain = false;
                Parent = -1; // -1 indicates no parent initially
                Weight = 1; // Starting weight can be 1 for Union-Find purposes
            }
        }


        static void MakeMatrixPercolateAndTest()
        {
            Random rand = new Random();
            bool percolates = false;

            do
            {
                // Reset UnionFind for a fresh start
                uf = new UnionFind(N * N + 2);

                // Initially test percolation with the current matrix state
                percolates = PerformPercolationTest();
                perculationTryCount++;
                Console.WriteLine(percolates ? "The matrix percolates" : "The matrix does not percolate");
                Console.WriteLine("Perculation tries count: " + perculationTryCount);
                Console.WriteLine("Open ground count: " + openGroundCount);
                longestOpenPathLength = CalculateLongestOpenPath();
                Console.WriteLine("Longest open path length: " + longestOpenPathLength);

                PrintMatrix(uf, virtualTopIndex);

                //wait for 1 second
                System.Threading.Thread.Sleep(500);

                if (!percolates)
                {
                    // Change a random cell to 'o'
                    int row = rand.Next(N);
                    int col = rand.Next(N);
                    matrix[row, col] = 'o';
                    openGroundCount++;
                    Console.WriteLine("\n-------------------------\n");
                }

            } while (!percolates);
        }

        static bool PerformPercolationTest()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] == 'o')
                    {
                        // Perform union operations in all four directions
                        for (int direction = 0; direction < 4; direction++)
                        {
                            UnionInDirection(i, j, direction, uf);
                        }

                        // Connect to virtual top and bottom nodes if applicable
                        int currentSite = i * N + j;
                        if (i == 0)
                        {
                            uf.Union(virtualTopIndex, currentSite);
                        }
                        if (i == N - 1)
                        {
                            uf.Union(virtualBottomIndex, currentSite);
                        }
                    }
                }
            }

            // Check if virtual top node is connected to virtual bottom node
            if (uf.Find(virtualTopIndex) == uf.Find(virtualBottomIndex))
            {
                // System percolates, now find the longest open path
                longestOpenPathLength = CalculateLongestOpenPath();
                return true;
            }
            return false;
        }

        static int CalculateLongestOpenPath()
        {
            bool[,] visited = new bool[N, N];
            int maxLength = 0;

            for (int col = 0; col < N; col++)
            {
                if (matrix[0, col] == 'o')
                {
                    int pathLength = DFS(0, col, visited);
                    maxLength = Math.Max(maxLength, pathLength);
                }
            }

            return maxLength;
        }

        static int DFS(int row, int col, bool[,] visited)
        {
            if (row < 0 || row >= N || col < 0 || col >= N || visited[row, col] || matrix[row, col] != 'o')
            {
                return 0;
            }

            visited[row, col] = true;

            // Explore all four directions
            int up = DFS(row - 1, col, visited);
            int down = DFS(row + 1, col, visited);
            int left = DFS(row, col - 1, visited);
            int right = DFS(row, col + 1, visited);

            // Return the maximum path length from the current cell
            return 1 + Math.Max(Math.Max(up, down), Math.Max(left, right));
        }



        // Method to perform union operation in a specified direction
        // direction: 0 = right, 1 = left, 2 = down, 3 = up
        static void UnionInDirection(int currentRow, int currentCol, int direction, UnionFind uf)
        {
            int newRow = currentRow, newCol = currentCol;

            switch (direction)
            {
                case 0: // Right
                    newCol += 1;
                    break;
                case 1: // Left
                    newCol -= 1;
                    break;
                case 2: // Down
                    newRow += 1;
                    break;
                case 3: // Up
                    newRow -= 1;
                    break;
            }

            // Check bounds and if the new cell is open ('o')
            if (newRow >= 0 && newRow < N && newCol >= 0 && newCol < N && matrix[newRow, newCol] == 'o')
            {
                int currentSite = currentRow * N + currentCol;
                int newSite = newRow * N + newCol;
                uf.Union(currentSite, newSite);
            }
        }

        static bool IsConnectedToTop(int cellIndex, UnionFind uf, int virtualTopIndex)
        {
            return uf.Find(cellIndex) == uf.Find(virtualTopIndex);
        }

        static void PrintMatrix(UnionFind uf, int virtualTopIndex)
        {
            Console.WriteLine();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    SetConsoleColorForCell(i, j, uf, virtualTopIndex);
                    Console.Write(matrix[i, j] == 'o' ? "██" : "  ");
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static void SetConsoleColorForCell(int i, int j, UnionFind uf, int virtualTopIndex)
        {
            if (matrix[i, j] == 'o')
            {
                int currentSite = i * N + j;
                bool isConnectedToTop = IsConnectedToTop(currentSite, uf, virtualTopIndex);
                Console.ForegroundColor = isConnectedToTop ? ConsoleColor.DarkBlue : ConsoleColor.DarkGray;
                Console.BackgroundColor = isConnectedToTop ? ConsoleColor.DarkBlue : ConsoleColor.DarkGray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Gray;
            }
        }

        static char[,] GenerateMatrix(int size, int maxWaterCells)
        {
            char[,] generatedMatrix = new char[size, size];
            Random rand = new Random();
            List<Tuple<int, int>> allCells = new List<Tuple<int, int>>();

            // Initialize all cells as blocked
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    generatedMatrix[i, j] = 'x';
                    allCells.Add(new Tuple<int, int>(i, j));
                }
            }

            // Randomly make cells open until maxWaterCells is reached
            for (int i = 0; i < maxWaterCells && allCells.Count > 0; i++)
            {
                int index = rand.Next(allCells.Count);
                Tuple<int, int> cell = allCells[index];
                generatedMatrix[cell.Item1, cell.Item2] = 'o';
                allCells.RemoveAt(index); // Remove the selected cell to avoid repetition
            }

            return generatedMatrix;
        }
    }
}
