using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using distributed_system;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpClientHandler);
        using var channel = GrpcChannel.ForAddress("https://localhost:8080", new GrpcChannelOptions { HttpHandler = grpcWebHandler });
        var client = new LocationService.LocationServiceClient(channel);

        bool isLoggedIn = false;
        while (!isLoggedIn)
        {
            isLoggedIn = await LoginLocation(client);
            if (!isLoggedIn)
            {
                Console.WriteLine("Login failed. Please try again.");
            }
        }

        while (true)
        {
            Console.WriteLine("Enter a command (add, update, get, exit):");
            var command = Console.ReadLine();

            switch (command?.ToLower())
            {
                case "add":
                    await AddLocation(client);
                    break;
                case "update":
                    await UpdateLocation(client);
                    break;
                case "get":
                    await GetLocation(client);
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    static async Task AddLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter max capacity:");
        var maxCapacityInput = Console.ReadLine();
        Console.WriteLine("Enter current capacity:");
        var currentCapacityInput = Console.ReadLine();
        Console.WriteLine("Is matriz (true/false):");
        var isMatrizInput = Console.ReadLine();

        if (name == null || maxCapacityInput == null || currentCapacityInput == null || isMatrizInput == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var maxCapacity = int.Parse(maxCapacityInput);
        var currentCapacity = int.Parse(currentCapacityInput);
        var isMatriz = bool.Parse(isMatrizInput);

        var request = new LocationRequest
        {
            Name = name,
            MaxCapacity = maxCapacity,
            CurrentCapacity = currentCapacity,
            IsMatriz = isMatriz
        };

        var response = await client.AddLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    static async Task UpdateLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location ID:");
        var id = Console.ReadLine();
        Console.WriteLine("Enter new location name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter new max capacity:");
        var maxCapacityInput = Console.ReadLine();
        Console.WriteLine("Enter new current capacity:");
        var currentCapacityInput = Console.ReadLine();
        Console.WriteLine("Is matriz (true/false):");
        var isMatrizInput = Console.ReadLine();

        if (id == null || name == null || maxCapacityInput == null || currentCapacityInput == null || isMatrizInput == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var maxCapacity = int.Parse(maxCapacityInput);
        var currentCapacity = int.Parse(currentCapacityInput);
        var isMatriz = bool.Parse(isMatrizInput);

        var request = new LocationUpdateRequest
        {
            Id = id,
            Name = name,
            MaxCapacity = maxCapacity,
            CurrentCapacity = currentCapacity,
            IsMatriz = isMatriz.ToString()
        };

        var response = await client.UpdateLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    static async Task GetLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location ID:");
        var id = Console.ReadLine();

        if (id == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var request = new LocationIdRequest { Id = id };
        var response = await client.GetLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    static async Task<bool> LoginLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location name:");
        var name = Console.ReadLine();

        if (name == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return false;
        }

        var request = new LocationLoginRequest { Name = name };
        var response = await client.LoginLocationAsync(request);

        if (response.Success)
        {
            Console.WriteLine($"Login successful. Location ID: {response.Id}");
            return true;
        }
        else
        {
            Console.WriteLine("Login failed: " + response.Message);
            return false;
        }
    }
}