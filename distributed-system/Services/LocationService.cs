using Grpc.Core;
using distributed_system;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Services;

public class LocationServiceImpl : LocationService.LocationServiceBase
{
    private readonly ILocationRepository _locationRepository;

    public LocationServiceImpl(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public override async Task<LocationResponse> AddLocation(LocationRequest request, ServerCallContext context)
    {
        try
        {
            Location location = new()
            {
                Name = request.Name,
                MaxCapacity = request.MaxCapacity,
                CurrentCapacity = request.CurrentCapacity,
                IsMatriz = request.IsMatriz,
                Username = request.Username,
                Password = request.Password
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
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> UpdateLocation(LocationUpdateRequest request, ServerCallContext context)
    {
        try
        {
            Location location = new()
            {
                Id = request.Id,
                Name = request.Name,
                MaxCapacity = request.MaxCapacity,
                CurrentCapacity = request.CurrentCapacity,
                IsMatriz = request.IsMatriz
            };

            var response = _locationRepository.UpdateLocation(location);

            var message = "Erro ao atualizar localização.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Localização atualizada com sucesso.";
            }

            return new LocationResponse
            {
                Message = message,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationGetResponse> GetLocation(LocationNameRequest request, ServerCallContext context)
    {
        try
        {
            var location = _locationRepository.GetLocationByName(request.Name);

            return new LocationGetResponse
            {
                Id = location.Id,
                Name = location.Name,
                MaxCapacity = location.MaxCapacity,
                CurrentCapacity = location.CurrentCapacity,
                IsMatriz = location.IsMatriz
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> DeleteLocation(LocationDeleteRequest request, ServerCallContext context)
    {
        try
        {
            var response = _locationRepository.DeleteLocation(request.Id, request.Name);

            var message = "Erro ao deletar localização.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Localização deletada com sucesso.";
            }

            return new LocationResponse
            {
                Message = message,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<LocationResponse> LoginLocation(LocationLoginRequest request, ServerCallContext context)
    {
        try
        {
            var location = _locationRepository.GetLocationByUsernameAndPassword(request.Username, request.Password);
            if (location == null)
            {
                return new LocationResponse
                {
                    Message = "Usuário ou senha inválidos.",
                    Success = false
                };
            }

            return new LocationResponse
            {
                Message = "Login efetuado com sucesso.",
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }
}
