using Grpc.Core;
using distributed_system;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;

namespace distributed_system.Services;

public class LocationServiceImpl : LocationService.LocationServiceBase
{
    private readonly ILocationRepository _locationRepository;

    public LocationServiceImpl(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public override Task<LocationResponse> AddLocation(LocationRequest request, ServerCallContext context)
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

        return Task.FromResult(new LocationResponse
        {
            Id = location.Id.ToString(),
            Message = message,
            Success = true
        });

    }

    public override Task<LocationResponse> UpdateLocation(LocationUpdateRequest request, ServerCallContext context)
    {
        return Task.FromResult(new LocationResponse
        {
            Id = request.Id,
            Message = "Localidade atualizada com sucesso.",
            Success = true
        });
    }

    public override Task<LocationResponse> GetLocation(LocationIdRequest request, ServerCallContext context)
    {
        return Task.FromResult(new LocationResponse
        {
            Id = request.Id,
            Message = "Informações da localidade obtidas.",
            Success = true
        });
    }
}
