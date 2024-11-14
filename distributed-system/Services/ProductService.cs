using distributed_system.Repositories.Intefaces;
using Grpc.Core;

namespace distributed_system.Services;

public class ProductServiceImpl : ProductService.ProductServiceBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<LocationServiceImpl> _logger;

    public ProductServiceImpl(
        IProductRepository productRepository,
        ILogger<LocationServiceImpl> logger
    )
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public override Task<ProductResponse> AddProduct(ProductRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<ProductResponse> GetProduct(ProductIdRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<ProductResponse> UpdateProduct(ProductUpdateRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}