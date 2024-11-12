using Grpc.Core;
using distributed_system;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;

namespace distributed_system.Services;

public class LocationServiceImpl : LocationService.LocationServiceBase
{
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<LocationServiceImpl> _logger;

    public LocationServiceImpl(
        ILocationRepository locationRepository,
        ILogger<LocationServiceImpl> logger
    )
    {
        _locationRepository = locationRepository;
        _logger = logger;
    }

    public override async Task<LocationResponse> AddLocation(LocationRequest request, ServerCallContext context)
    {
        try
        {
            var location = new Location()
            {
                Id = Guid.NewGuid().GetHashCode(),
                Name = request.Name,
                MaxCapacity = request.MaxCapacity,
                CurrentCapacity = request.CurrentCapacity,
                IsMatriz = request.IsMatriz
            };

            var response = _locationRepository.AddLocation(location);

            var message = location.IsMatriz ?
                "Matriz adicionada com sucesso." :
                "Filial adicionada com sucesso.";

            return new LocationResponse
            {
                Id = location.Id.ToString(),
                Message = message,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddLocation");
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> UpdateLocation(LocationUpdateRequest request, ServerCallContext context)
    {
        try
        {
            return new LocationResponse
            {
                Id = request.Id,
                Message = "Localidade atualizada com sucesso.",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateLocation");
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> GetLocation(LocationIdRequest request, ServerCallContext context)
    {
        try
        {
            return new LocationResponse
            {
                Id = request.Id,
                Message = "Informações da localidade obtidas.",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLocation");
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> LoginLocation(LocationLoginRequest request, ServerCallContext context)
    {
        try
        {
            var location = _locationRepository.GetLocationByName(request.Name);
            if (location == null)
            {
                return new LocationResponse
                {
                    Message = "Location not found.",
                    Success = false
                };
            }

            return new LocationResponse
            {
                Id = location.Id.ToString(),
                Message = "Login successful.",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LoginLocation");
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }
}
