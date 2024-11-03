using Grpc.Core;
using distributed_system;

namespace distributed_system.Services;

public class LocationServiceImpl : LocationService.LocationServiceBase
{
    public override Task<LocationResponse> AddLocation(LocationRequest request, ServerCallContext context)
    {
        // Lógica para adicionar localidade
        return Task.FromResult(new LocationResponse
        {
            Id = request.Id,
            Message = "Localidade adicionada com sucesso.",
            Success = true
        });
    }

    public override Task<LocationResponse> UpdateLocation(LocationRequest request, ServerCallContext context)
    {
        // Lógica para atualizar localidade
        return Task.FromResult(new LocationResponse
        {
            Id = request.Id,
            Message = "Localidade atualizada com sucesso.",
            Success = true
        });
    }

    public override Task<LocationResponse> GetLocation(LocationIdRequest request, ServerCallContext context)
    {
        // Lógica para obter localidade
        return Task.FromResult(new LocationResponse
        {
            Id = request.Id,
            Message = "Informações da localidade obtidas.",
            Success = true
        });
    }
}
