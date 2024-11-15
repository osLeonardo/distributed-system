using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using distributed_system;
using location_client.Services;

class Program
{
    private static IClientService _clientService;

    public Program(IClientService clientService)
    {
        _clientService = clientService;
    }

    static async Task Main(string[] args)
    {
        _clientService = new ClientService();

        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpClientHandler);
        using var channel = GrpcChannel.ForAddress("https://localhost:8080", new GrpcChannelOptions { HttpHandler = grpcWebHandler });
        var locationClient = new LocationService.LocationServiceClient(channel);
        var productClient = new ProductService.ProductServiceClient(channel);

        bool isLoggedIn = false;
        while (!isLoggedIn)
        {
            isLoggedIn = await _clientService.LoginLocation(locationClient);
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
                {
                    await _clientService.AddLocation(locationClient);
                    break;
                }
                case "update":
                {
                    Console.WriteLine("Enter a parameter (login, location, product, cancel):");
                    var newCommand = Console.ReadLine();

                    switch (newCommand?.ToLower())
                    {
                        case "login":
                        {
                            await _clientService.UpdateLocationLogin(locationClient);
                            break;
                        }
                        case "update":
                        {
                            await _clientService.UpdateLocation(locationClient);
                            break;
                        }
                        case "product":
                        {
                            break;
                        }
                        case "cancel":
                        {
                            break;
                        }
                        default:
                        {
                            Console.WriteLine("Unknown command.");
                            break;
                        }
                    }
                    break;
                }
                case "get":
                {
                    await _clientService.GetLocation(locationClient);
                    break;
                }
                case "exit":
                {
                    return;
                }
                default:
                {
                    Console.WriteLine("Unknown command.");
                    break;
                }
            }
        }
    }
}