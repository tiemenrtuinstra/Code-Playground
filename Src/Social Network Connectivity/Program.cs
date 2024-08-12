using Common.Enums;
using Common.Extensions;
using SocialNetworkConnectivity.Dto;

namespace SocialNetworkConnectivity;

public class Program
{
    private static int _numberOfUsers;
    private static long _numberOfConnections;
    private static readonly int MAX_USERS = 10000;
    private static readonly long MAX_CONNECTIONS = 499995000;

    private static HashSet<Connection> _connections = new HashSet<Connection>();

    private static readonly string _connectionFilePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? AppContext.BaseDirectory, "data", "connections.json");
    private static FileManager _fileManager = new FileManager(_connections, _connectionFilePath);

    private static ConnectionChecker _connectionChecker = new ConnectionChecker();

    private static void Main(string[] args)
    {
        Console.Title = "Social Network Connectivity";
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Hello, welcome to the matrix, lolz");
        Thread.Sleep(1000);

        Console.WriteLine();
        Console.WriteLine("Get ready...");
        Thread.Sleep(1000);

        for (int i = 5; i > 0; i--)
        {
            Console.WriteLine(i.ToString());
            Thread.Sleep(1000);
        }

        Console.WriteLine("Go!");
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        if (File.Exists(_connectionFilePath))
        {
            Console.WriteLine("File already exists, do you want to overwrite it with new connections? (y/n) (default: no)");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Warning: This will overwrite the existing file.");
            Console.BackgroundColor = ConsoleColor.Black;
            string overWriteResponse = Console.ReadLine() ?? "n";
            if (overWriteResponse.ToLower() == "y")
            {
                SetAmount();
                Console.WriteLine();
                GenerateConnections();
                Console.WriteLine();
                _fileManager.SaveConnectionsToFile();
            }
            else
            {
                Console.WriteLine("Using established connections.");
                Console.WriteLine();
                ConsoleExtensions.CustomLine(lineType: LineType.Dotted);
                _fileManager.GetConnectionsFromFile();
            }

            Console.WriteLine();
            Console.WriteLine($"Using {_connections.Count()} Connections");
            Console.WriteLine();
        }

        Console.BackgroundColor = ConsoleColor.Black;


        int earliestTime = _connectionChecker.EarliestTime(_numberOfUsers, _connections);
        Console.WriteLine("The earliest time at which all users are connected is: " + earliestTime);

        Console.WriteLine();
        ConsoleExtensions.CustomLine(lineType:LineType.Dotted);

        ConsoleExtensions.EndProgram();
    }

    private static void SetAmount()
    {
        Console.WriteLine();
        Console.WriteLine("Let's generate some connections.");
        SetUserAmount();
        SetConnectionAmount();

        Console.WriteLine("Your input");
        Console.WriteLine($"user: {_numberOfUsers}");
        Console.WriteLine($"connections: {_numberOfConnections}");
    }

    private static void SetUserAmount()
    {
        Console.WriteLine($"Enter the number of users: (up to {MAX_USERS})");

        _numberOfUsers = int.TryParse(Console.ReadLine(), out var result) ? result : MAX_USERS;

        if (_numberOfUsers > 10000)
        {
            Console.WriteLine($"Can you read? Probably not, the amount of users shouldn't exceed {MAX_USERS}");
            SetUserAmount();
        }
    }

    private static void SetConnectionAmount()
    {
        Console.WriteLine($"Enter the number of connections you wish to generate (up to {MAX_CONNECTIONS}):");

        _numberOfConnections = long.TryParse(Console.ReadLine(), out var result) ? result : MAX_CONNECTIONS;

        if (_numberOfConnections > 499995000)
        {
            Console.WriteLine($"Can you read? Probably not, the amount of connections shouldn't exceed {MAX_CONNECTIONS}");
            SetConnectionAmount();
        }
    }

    private static void GenerateConnections()
    {
        Random random = new Random();

        while (_connections.Count < _numberOfConnections)
        {
            int user1 = random.Next(_numberOfUsers);
            int user2 = random.Next(_numberOfUsers);

            if (user1 != user2)
            {
                Connection pair = new Connection { User1Id = user1, User2Id = user2, DateTime = DateTime.UtcNow };

                // Check if the connection already exists
                if (!_connections.Any(c => (c.User1Id == user1 && c.User2Id == user2) || (c.User1Id == user2 && c.User2Id == user1)))
                {
                    _connections.Add(pair);
                }
            }

            if (_connections.Count % _numberOfUsers == 0)
            {
                Console.WriteLine($"Total connections generated: {_connections.Count}");
            }
        }
        Console.WriteLine($"Total unique connections generated: {_connections.Count}");
    }





}