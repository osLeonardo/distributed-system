using Grpc.Core;
using distributed_system;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Microsoft.AspNetCore.Mvc;

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
        var location = new Location()
        {
            Name = request.Name,
            MaxCapacity = request.MaxCapacity,
            CurrentCapacity = request.CurrentCapacity,
            IsMatriz = request.IsMatriz
        };

        var response = _locationRepository.AddLocation(location);

        var message = "Erro ao adicionar localização.";
        var success = false;

        if (response is OkResult)
        {
            success = true;
            message = location.IsMatriz ?
                "Matriz adicionada com sucesso." :
                "Filial adicionada com sucesso.";
        }

        return new LocationResponse
        {
            Message = message,
            Success = true
        };
    }

    public override async Task<LocationResponse> UpdateLocation(LocationUpdateRequest request, ServerCallContext context)
    {
        try
        {
            return new LocationResponse
            {
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

    public override async Task<LocationResponse> GetLocation(LocationNameRequest request, ServerCallContext context)
    {
        try
        {
            return new LocationResponse
            {
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
