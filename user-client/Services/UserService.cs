using System.Security.Claims;
using System.Text;

namespace user_client;

public class UserService
{
    private readonly LocationService.LocationServiceClient _locationClient;
    private static readonly HashSet<string> LoggedInUsers = new();

    public UserService(LocationService.LocationServiceClient locationClient)
    {
        _locationClient = locationClient;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var request = new LocationLoginRequest { Username = username, Password = password };
        var response = await _locationClient.LoginLocationAsync(request);

        if (response.Success)
        {
            LoggedInUsers.Add(username);
            return true;
        }

        return false;
    }

    public bool IsLoggedIn(string username)
    {
        return LoggedInUsers.Contains(username);
    }

    public void Logout(string username)
    {
        LoggedInUsers.Remove(username);
    }
}