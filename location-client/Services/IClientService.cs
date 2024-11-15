namespace location_client.Services;

public interface IClientService
{
    Task AddLocation(LocationService.LocationServiceClient client);
    Task UpdateLocation(LocationService.LocationServiceClient client);
    Task UpdateLocationLogin(LocationService.LocationServiceClient client);
    Task GetLocation(LocationService.LocationServiceClient client);
    Task<bool> LoginLocation(LocationService.LocationServiceClient client);
}