using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Services;

public class ProductLocationServiceImpl : ProductLocationService.ProductLocationServiceBase
{
    private readonly IProductLocationRepository _productLocationRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILocationRepository _locationRepository;

    public ProductLocationServiceImpl(
        IProductLocationRepository productLocationRepository,
        IProductRepository productRepository,
        ILocationRepository locationRepository
    )
    {
        _productLocationRepository = productLocationRepository;
        _productRepository = productRepository;
        _locationRepository = locationRepository;
    }

    public override async Task<ProductLocationResponse> AddProductLocation(ProductLocationRequest request, ServerCallContext context)
    {
        try
        {
            var product = _productRepository.GetProductById(request.ProductId);
            var location = _locationRepository.GetLocationById(request.LocationId);
            location.CurrentCapacity -= request.Quantity;

            ProductLocation productLocation = new()
            {
                LocationId = request.LocationId,
                Location = location,
                ProductId = request.ProductId,
                Product = product,
                Amount = request.Quantity
            };

            var locationResponse = _locationRepository.UpdateLocation(location);
            var productLocationResponse = _productLocationRepository.AddProductLocation(productLocation);

            var message = "Erro ao associar produto a localização.";
            var success = false;

            if (productLocationResponse is OkResult && locationResponse is OkResult)
            {
                message = "Produto associar a localização com sucesso.";
                success = true;
            }

            return new ProductLocationResponse
            {
                Message = message,
                Success = success
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductLocationResponse> UpdateProductLocation(ProductLocationUpdateRequest request, ServerCallContext context)
    {
        try
        {
            var productLocation = _productLocationRepository.GetProductLocationById(request.Id);
            var product = _productRepository.GetProductById(request.ProductId);
            var location = _locationRepository.GetLocationById(request.LocationId);

            if (productLocation.Amount > request.Quantity)
            {
                location.CurrentCapacity += productLocation.Amount - request.Quantity;
            }
            else
            {
                location.CurrentCapacity -= request.Quantity - productLocation.Amount;
            }

            productLocation = new ProductLocation
            {
                Id = request.Id,
                LocationId = request.LocationId,
                Location = location,
                ProductId = request.ProductId,
                Product = product,
                Amount = request.Quantity
            };

            var productLocationResponse = _productLocationRepository.UpdateProductLocation(productLocation);
            var locationResponse = _locationRepository.UpdateLocation(location);

            var message = "Erro ao atualizar associação de produto a localização.";
            var success = false;

            if (productLocationResponse is OkResult && locationResponse is OkResult)
            {
                success = true;
                message = "Associação de produto a localização atualizada com sucesso.";
            }

            return new ProductLocationResponse
            {
                Message = message,
                Success = success
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductLocationGetResponse> GetProductLocationById(ProductLocationIdRequest request, ServerCallContext context)
    {
        try
        {
            var productLocation = _productLocationRepository.GetProductLocationById(request.Id);
            var location = _locationRepository.GetLocationById(productLocation.LocationId);
            var locationResponse = new LocationData
            {
                Id = location.Id,
                Name = location.Name,
                MaxCapacity = location.MaxCapacity,
                CurrentCapacity = location.CurrentCapacity,
                IsMatriz = location.IsMatriz,
            };
            var product = _productRepository.GetProductById(productLocation.ProductId);
            var productResponse = new ProductData
            {
                Id = product.Id,
                Brand = product.Brand,
                Name = product.Name,
                CostUnit = product.CostUnit,
                SalePrice = product.SalePrice,
            };

            return new ProductLocationGetResponse
            {
                Id = productLocation.Id,
                Location = locationResponse,
                Product = productResponse,
                Quantity = productLocation.Amount
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductLocationGetResponseList> GetAllProductLocations(Empty request, ServerCallContext context)
    {
        try
        {
            var productLocations = _productLocationRepository.GetAllProductLocations();
            var response = new ProductLocationGetResponseList();
            var locationResponse = new LocationData();
            var productResponse = new ProductData();
            var location = new Location();
            var product = new Product();

            foreach (var productLocation in productLocations)
            {
                location = _locationRepository.GetLocationById(productLocation.LocationId);
                locationResponse = new LocationData
                {
                    Id = location.Id,
                    Name = location.Name,
                    MaxCapacity = location.MaxCapacity,
                    CurrentCapacity = location.CurrentCapacity,
                    IsMatriz = location.IsMatriz,
                };
                product = _productRepository.GetProductById(productLocation.ProductId);
                productResponse = new ProductData
                {
                    Id = product.Id,
                    Brand = product.Brand,
                    Name = product.Name,
                    CostUnit = product.CostUnit,
                    SalePrice = product.SalePrice,
                };

                response.ProductLocations.Add(new ProductLocationGetResponse
                {
                    Id = productLocation.Id,
                    Location = locationResponse,
                    Product = productResponse,
                    Quantity = productLocation.Amount
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductLocationResponse> DeleteProductLocation(ProductLocationDeleteRequest request, ServerCallContext context)
    {
        try
        {
            var productLocation = _productLocationRepository.GetProductLocationById(request.Id);
            var location = _locationRepository.GetLocationById(productLocation.LocationId);
            location.CurrentCapacity += productLocation.Amount;

            var locationResponse = _locationRepository.UpdateLocation(location);
            var productLocationResponse = _productLocationRepository.DeleteProductLocation(request.Id);

            var message = "Erro ao deletar associação de produto a localização.";
            var success = false;

            if (productLocationResponse is OkResult && locationResponse is OkResult)
            {
                success = true;
                message = "Associação de produto a localização deletada com sucesso.";
            }

            return new ProductLocationResponse
            {
                Message = message,
                Success = success
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductLocationResponse> MoveProductLocation(ProductLocationMoveRequest request, ServerCallContext context)
    {
        try
        {
            var fromProductLocation = _productLocationRepository.GetProductLocationById(request.FromProductLocationId);
            var fromLocation = _locationRepository.GetLocationById(fromProductLocation.LocationId);

            var toProductLocation = _productLocationRepository.GetProductLocationById(request.ToProductLocationId);
            var toLocation = _locationRepository.GetLocationById(toProductLocation.LocationId);

            if (fromProductLocation.Amount < request.Quantity)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Quantidade a ser movida é maior que a quantidade disponível."));
            }
            if (toLocation.CurrentCapacity < request.Quantity)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Quantidade a ser movida é maior que a capacidade do local de destino."));
            }

            fromProductLocation.Amount -= request.Quantity;
            fromLocation.CurrentCapacity += request.Quantity;

            toProductLocation.Amount += request.Quantity;
            toLocation.CurrentCapacity -= request.Quantity;

            var fromProductLocationResponse = _productLocationRepository.UpdateProductLocation(fromProductLocation);
            var fromLocationResponse = _locationRepository.UpdateLocation(fromLocation);

            var toProductLocationResponse = _productLocationRepository.UpdateProductLocation(toProductLocation);
            var toLocationResponse = _locationRepository.UpdateLocation(toLocation);

            var message = "Erro ao mover produto de localização.";
            var success = false;

            if (fromProductLocationResponse is OkResult &&
                fromLocationResponse is OkResult &&
                toProductLocationResponse is OkResult &&
                toLocationResponse is OkResult)
            {
                success = true;
                message = "Produto movido de localização com sucesso.";
            }

            return new ProductLocationResponse
            {
                Message = message,
                Success = success
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }
}