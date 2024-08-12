using SocialNetworkConnectivity.Dto;
using System.Text.Json;

namespace SocialNetworkConnectivity;

public class FileManager
{
    private HashSet<Connection> _connections;
    private string _connectionFilePath;

    public FileManager(HashSet<Connection> connections, string connectionFilePath)
    {
        _connections = connections;
        _connectionFilePath = connectionFilePath;
    }

    public void SaveConnectionsToFile()
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(_connections, new JsonSerializerOptions { WriteIndented = true });
            string directoryPath = Path.GetDirectoryName(_connectionFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(_connectionFilePath, jsonString);
            Console.WriteLine($"File saved to {_connectionFilePath}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Error: No permission to write file. " + ex.Message);
            Helper.PressKeyToExit();
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error writing file: " + ex.Message);
            Helper.PressKeyToExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            Helper.PressKeyToExit();
        }
    }

    public void GetConnectionsFromFile()
    {
        //check if file exists
        if (!File.Exists(_connectionFilePath))
        {
            Console.WriteLine("File does not exist.");
            Helper.PressKeyToExit();
        }

        try
        {
            string jsonString = File.ReadAllText(_connectionFilePath);
            _connections = JsonSerializer.Deserialize<HashSet<Connection>>(jsonString);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Error: No permission to read file. " + ex.Message);
            Helper.PressKeyToExit();
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
            Helper.PressKeyToExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            Helper.PressKeyToExit();
        }
    }

}
