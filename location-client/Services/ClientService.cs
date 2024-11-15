namespace location_client.Services;

public class ClientService : IClientService
{
    public async Task AddLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter max capacity:");
        var maxCapacityInput = Console.ReadLine();
        Console.WriteLine("Enter current capacity:");
        var currentCapacityInput = Console.ReadLine();
        Console.WriteLine("Is matriz (true/false):");
        var isMatrizInput = Console.ReadLine();
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();

        if (name == null || maxCapacityInput == null || currentCapacityInput == null || isMatrizInput == null || username == null || password == null)
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
            IsMatriz = isMatriz,
            Username = username,
            Password = password
        };

        var response = await client.AddLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    public async Task UpdateLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location ID:");
        var idInput = Console.ReadLine();
        Console.WriteLine("Enter new location name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter new max capacity:");
        var maxCapacityInput = Console.ReadLine();
        Console.WriteLine("Enter new current capacity:");
        var currentCapacityInput = Console.ReadLine();
        Console.WriteLine("Is matriz (true/false):");
        var isMatrizInput = Console.ReadLine();

        if (idInput == null || name == null || maxCapacityInput == null || currentCapacityInput == null || isMatrizInput == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var id = int.Parse(idInput);
        var maxCapacity = int.Parse(maxCapacityInput);
        var currentCapacity = int.Parse(currentCapacityInput);
        var isMatriz = bool.Parse(isMatrizInput);

        var request = new LocationUpdateRequest
        {
            Id = id,
            Name = name,
            MaxCapacity = maxCapacity,
            CurrentCapacity = currentCapacity,
            IsMatriz = isMatriz,
        };

        var response = await client.UpdateLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    public async Task UpdateLocationLogin(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location ID:");
        var idInput = Console.ReadLine();
        Console.WriteLine("Enter new username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter new password:");
        var password = Console.ReadLine();

        if (idInput == null || username == null || password == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var id = int.Parse(idInput);

        var request = new LocationUpdateRequest
        {
            Id = id,
            Username = username,
            Password = password
        };

        var response = await client.UpdateLocationAsync(request);
        Console.WriteLine(response.Message);
    }

    public async Task GetLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter location Name:");
        var name = Console.ReadLine();

        if (name == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return;
        }

        var request = new LocationNameRequest { Name = name };
        var response = await client.GetLocationAsync(request);
        Console.WriteLine(response);
    }

    public async Task<bool> LoginLocation(LocationService.LocationServiceClient client)
    {
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();

        if (username == null || password == null)
        {
            Console.WriteLine("Invalid input. Please try again.");
            return false;
        }

        var request = new LocationLoginRequest { Username = username, Password = password };
        var response = await client.LoginLocationAsync(request);

        if (response.Success)
        {
            Console.WriteLine("Login successful.");
            return true;
        }
        else
        {
            Console.WriteLine("Login failed: " + response.Message);
            return false;
        }
    }
}