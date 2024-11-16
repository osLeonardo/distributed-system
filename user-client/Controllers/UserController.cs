using distributed_system;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using user_client.Entities;

namespace user_client.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ProductService.ProductServiceClient _productClient;
    private readonly LocationService.LocationServiceClient _locationClient;
    private readonly ProductLocationService.ProductLocationServiceClient _productLocationClient;

    public UserController(UserService userService,
                          ProductService.ProductServiceClient productClient,
                          LocationService.LocationServiceClient locationClient,
                          ProductLocationService.ProductLocationServiceClient productLocationClient)
    {
        _userService = userService;
        _productClient = productClient;
        _locationClient = locationClient;
        _productLocationClient = productLocationClient;
    }

    #region User Endpoints
    [HttpPost("login")]
    [Tags("User")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var success = await _userService.LoginAsync(request.Username, request.Password);
        if (success)
        {
            HttpContext.Session.SetString("username", request.Username);
            return Ok("Login successful.");
        }
        return Unauthorized("Invalid username or password.");
    }

    [HttpPost("logout")]
    [Tags("User")]
    public IActionResult Logout()
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null)
        {
            _userService.Logout(username);
            HttpContext.Session.Remove("username");
            return Ok("Logout successful.");
        }
        return BadRequest("User not logged in.");
    }
    #endregion

    #region Location Endpoints
    [HttpPost("location")]
    [Tags("Location")]
    public async Task<IActionResult> AddLocation([FromBody] LocationRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.AddLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpPut("location")]
    [Tags("Location")]
    public async Task<IActionResult> UpdateLocation([FromBody] LocationUpdateRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.UpdateLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("location/{name}")]
    [Tags("Location")]
    public async Task<IActionResult> GetLocationByName(string name)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.GetLocationByNameAsync(new LocationNameRequest { Name = name });
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("location/id/{id}")]
    [Tags("Location")]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.GetLocationByIdAsync(new LocationIdRequest { Id = id });
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("locations")]
    [Tags("Location")]
    public async Task<IActionResult> GetAllLocations()
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.GetAllLocationsAsync(new Empty());
            return Ok(response.Locations);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpDelete("location")]
    [Tags("Location")]
    public async Task<IActionResult> DeleteLocation([FromBody] LocationDeleteRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _locationClient.DeleteLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }
    #endregion

    #region Product Endpoints
    [HttpPost("product")]
    [Tags("Product")]
    public async Task<IActionResult> AddProduct([FromBody] ProductRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productClient.AddProductAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpPut("product")]
    [Tags("Product")]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productClient.UpdateProductAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("product/{id}")]
    [Tags("Product")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productClient.GetProductByIdAsync(new ProductIdRequest { Id = id });
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("products")]
    [Tags("Product")]
    public async Task<IActionResult> GetAllProducts()
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productClient.GetAllProductsAsync(new Empty());
            return Ok(response.Products);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpDelete("product")]
    [Tags("Product")]
    public async Task<IActionResult> DeleteProduct([FromBody] ProductDeleteRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productClient.DeleteProductAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }
    #endregion

    #region ProductLocation Endpoints
    [HttpPost("productlocation")]
    [Tags("Product Location")]
    public async Task<IActionResult> AddProductLocation([FromBody] ProductLocationRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productLocationClient.AddProductLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpPut("productlocation")]
    [Tags("Product Location")]
    public async Task<IActionResult> UpdateProductLocation([FromBody] ProductLocationUpdateRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productLocationClient.UpdateProductLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("productlocation/{id}")]
    [Tags("Product Location")]
    public async Task<IActionResult> GetProductLocationById(int id)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productLocationClient.GetProductLocationByIdAsync(new ProductLocationIdRequest { Id = id });
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpGet("productlocations")]
    [Tags("Product Location")]
    public async Task<IActionResult> GetAllProductLocations()
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productLocationClient.GetAllProductLocationsAsync(new Empty());
            return Ok(response.ProductLocations);
        }
        return Unauthorized("You are not logged in.");
    }

    [HttpDelete("productlocation")]
    [Tags("Product Location")]
    public async Task<IActionResult> DeleteProductLocation([FromBody] ProductLocationDeleteRequest request)
    {
        var username = HttpContext.Session.GetString("username");
        if (username != null && _userService.IsLoggedIn(username))
        {
            var response = await _productLocationClient.DeleteProductLocationAsync(request);
            return Ok(response);
        }
        return Unauthorized("You are not logged in.");
    }
    #endregion
}