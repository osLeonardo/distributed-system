using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Services;

public class ProductServiceImpl : ProductService.ProductServiceBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductServiceImpl> _logger;

    public ProductServiceImpl(
        IProductRepository productRepository,
        ILogger<ProductServiceImpl> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public override async Task<ProductResponse> AddProduct(ProductRequest request, ServerCallContext context)
    {
        try
        {
            Product product = new()
            {
                Brand = request.Brand,
                Name = request.Name,
                CostUnit = request.CostUnit,
                SalePrice = request.SalePrice
            };

            var response = _productRepository.AddProduct(product);

            var message = "Erro ao adicionar produto.";
            var success = false;

            if (response is OkResult)
            {
                message = "Produto adicionado com sucesso.";
                success = true;
            }

            return new ProductResponse
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

    public override async Task<ProductResponse> UpdateProduct(ProductUpdateRequest request, ServerCallContext context)
    {
        try
        {
            Product product = new()
            {
                Id = request.Id,
                Brand = request.Brand,
                Name = request.Name,
                CostUnit = request.CostUnit,
                SalePrice = request.SalePrice
            };

            var response = _productRepository.UpdateProduct(product);

            var message = "Erro ao atualizar produto.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Produto atualizado com sucesso.";
            }

            return new ProductResponse
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

    public override async Task<ProductGetResponse> GetProductByName(ProductNameRequest request, ServerCallContext context)
    {
        try
        {
            var product = _productRepository.GetProductByName(request.Name);

            return new ProductGetResponse
            {
                Id = product.Id,
                Brand = product.Brand,
                Name = product.Name,
                CostUnit = product.CostUnit,
                SalePrice = product.SalePrice
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductGetResponse> GetProductById(ProductIdRequest request, ServerCallContext context)
    {
        try
        {
            var product = _productRepository.GetProductById(request.Id);

            return new ProductGetResponse
            {
                Id = product.Id,
                Brand = product.Brand,
                Name = product.Name,
                CostUnit = product.CostUnit,
                SalePrice = product.SalePrice
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductGetResponseList> GetAllProducts(Empty request, ServerCallContext context)
    {
        try
        {
            var products = _productRepository.GetAllProducts();

            var response = new ProductGetResponseList();

            foreach (var product in products)
            {
                response.Products.Add(new ProductGetResponse
                {
                    Id = product.Id,
                    Brand = product.Brand,
                    Name = product.Name,
                    CostUnit = product.CostUnit,
                    SalePrice = product.SalePrice
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."), ex.Message);
        }
    }

    public override async Task<ProductResponse> DeleteProduct(ProductDeleteRequest request, ServerCallContext context)
    {
        try
        {
            var response = _productRepository.DeleteProduct(request.Id, request.Name);

            var message = "Erro ao deletar produto.";
            var success = false;

            if (response is OkResult)
            {
                success = true;
                message = "Produto deletado com sucesso.";
            }

            return new ProductResponse
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
}