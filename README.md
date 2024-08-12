# Solution Overview

Welcome to the Code Playground! This solution contains multiple projects, each addressing a different problem or functionality. The projects included are:

1. **Percolation Problem**
2. **Social Network Connectivity**
3. **Egg Drop Problem**
4. **Color Converter**

This Code Playground allows you to explore and experiment with various algorithms and data structures implemented in these projects. Each project is designed to solve a specific problem, and you can run and modify the code to see how it works.

## Projects

### 1. Percolation Problem

The Percolation Problem project simulates a percolation system using a grid. The goal is to determine if the system percolates, meaning there is a path from the top to the bottom of the grid through open cells.

#### Key Files

- **Program.cs**: Contains the main logic for simulating the percolation system.
- **UnionFind.cs**: Implements the union-find data structure with path compression and union by rank.

#### How to Run

1. Open the solution in Visual Studio.
2. Set the `PercolationProblem` project as the startup project.
3. Run the project.

#### Main Methods

- `Main(string[] args)`: Entry point of the application.
- `MakeMatrixPercolateAndTest()`: Simulates the percolation process and tests if the system percolates.
- `PrintMatrix(UnionFind uf, int virtualTopIndex)`: Prints the current state of the matrix.
- `GenerateMatrix(int size, int maxWaterCells)`: Generates a random matrix with open and blocked cells.
- `PerformPercolationTest()`: Tests if the system percolates.

### 2. Social Network Connectivity

The Social Network Connectivity project determines the earliest time at which all users in a social network are connected based on a series of connection events.

#### Key Files

- **Program.cs**: Contains the main logic for reading connection data and determining connectivity.
- **UnionFind.cs**: Implements the union-find data structure with path compression and union by rank.
- **FileManager.cs**: Manages reading and writing connection data to and from a file.
- **ConnectionChecker.cs**: Contains the logic to determine the earliest time all users are connected.

#### How to Run

1. Open the solution in Visual Studio.
2. Set the `SocialNetworkConnectivity` project as the startup project.
3. Run the project.

#### Main Methods

- `Main(string[] args)`: Entry point of the application.
- `SetAmount()`: Sets the number of users and connections.
- `GenerateConnections()`: Generates random connections between users.
- `EarliestTime(int numberOfUsers, HashSet<Connection> connections)`: Determines the earliest time all users are connected.

### 3. Egg Drop Problem

The Egg Drop Problem project solves the classic egg drop problem, which involves determining the minimum number of trials needed to find the highest floor from which an egg can be dropped without breaking.

#### Key Files

- **Program.cs**: Contains the main logic for solving the egg drop problem.
- **EggDrop.cs**: Implements the algorithms for solving the egg drop problem.

#### How to Run

1. Open the solution in Visual Studio.
2. Set the `EggDropProblem` project as the startup project.
3. Run the project.

#### Main Methods

- `TwoEggDropProblem(int numberOfFloors)`: Solves the two-egg drop problem.
- `MultiEggMultiFloorEggDropProblem(int numberOfFloors, int numberOfEggs)`: Solves the multi-egg, multi-floor egg drop problem.

### 4. Color Converter

The Color Converter project provides utilities for printing text in random colors in the console.

#### Key Files

- **ColorConsoleExtensions.cs**: Contains extension methods for printing text in random colors.

#### How to Run

1. Open the solution in Visual Studio.
2. Set the `ColorConverter` project as the startup project.
3. Run the project.

#### Main Methods

- `PrintRandomColor(string text, bool colorForeground = true, bool colorBackground = false)`: Prints text in random colors.

## Common Library

### UnionFind

The `UnionFind` class is used by multiple projects to manage disjoint sets efficiently. It supports path compression and union by rank to optimize the union-find operations.

#### Methods

- `UnionFind(int size)`: Initializes the union-find data structure.
- `int Find(int x)`: Finds the root of the element `x` with path compression.
- `bool Union(int x, int y)`: Unites the sets containing `x` and `y`.
- `int Count()`: Returns the number of disjoint sets.

## How to Build and Run

1. Open the solution in Visual Studio.
2. Build the solution to restore dependencies and compile the projects.
3. Set the desired project as the startup project.
4. Run the project.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.

## Contact

For any questions or issues, please open an issue in the repository or contact the project maintainers.
