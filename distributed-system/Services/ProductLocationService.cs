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
            var location = _locationRepository.GetLocationById(request.LocationId);
            var product = _productRepository.GetProductById(request.ProductId);

            ProductLocation productLocation = new()
            {
                LocationId = request.LocationId,
                Location = location,
                ProductId = request.ProductId,
                Product = product,
                Amount = request.Quantity
            };

            var response = _productLocationRepository.AddProductLocation(productLocation);

            var message = "Erro ao associar produto a localização.";
            var success = false;

            if (response is OkResult)
            {
                message = "Produto associar a localização com sucesso.";
                success = true;
            }

            return new ProductLocationResponse
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

    public override async Task<ProductLocationResponse> UpdateProductLocation(ProductLocationUpdateRequest request, ServerCallContext context)
    {
        try
        {
            var product = _productRepository.GetProductById(request.ProductId);
            var location = _locationRepository.GetLocationById(request.LocationId);

            ProductLocation productLocation = new()
            {
                Id = request.Id,
                LocationId = request.LocationId,
                Location = location,
                ProductId = request.ProductId,
                Product = product,
                Amount = request.Quantity
            };

            var response = _productLocationRepository.UpdateProductLocation(productLocation);

            var message = "Erro ao atualizar associação de produto a localização.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Associação de produto a localização atualizada com sucesso.";
            }

            return new ProductLocationResponse
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

    public override async Task<ProductLocationGetResponse> GetProductLocationById(ProductLocationIdRequest request, ServerCallContext context)
    {
        try
        {
            var productLocation = _productLocationRepository.GetProductLocationById(request.Id);
            var location = _locationRepository.GetLocationById(productLocation.LocationId);
            var product = _productRepository.GetProductById(productLocation.ProductId);

            var locationResponse = new LocationData
            {
                Id = location.Id,
                Name = location.Name,
                MaxCapacity = location.MaxCapacity,
                CurrentCapacity = location.CurrentCapacity,
                IsMatriz = location.IsMatriz,
            };

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
                Id = product.Id,
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
                product = _productRepository.GetProductById(productLocation.ProductId);
                location = _locationRepository.GetLocationById(productLocation.LocationId);

                locationResponse = new LocationData
                {
                    Id = location.Id,
                    Name = location.Name,
                    MaxCapacity = location.MaxCapacity,
                    CurrentCapacity = location.CurrentCapacity,
                    IsMatriz = location.IsMatriz,
                };

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
            var response = _productLocationRepository.DeleteProductLocation(request.Id);

            var message = "Erro ao deletar associação de produto a localização.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Associação de produto a localização deletada com sucesso.";
            }

            return new ProductLocationResponse
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

    public override async Task<ProductLocationResponse> MoveProductLocation(ProductLocationMoveRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}