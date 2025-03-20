using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcCrudService;
using GrpcExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GrpcExample.GrpcServices
{
    public class ProductService : productService.productServiceBase
    {
        public AppDbContext _context { get; set; }

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ColorSet = string.Join(",", request.ColorSet),
                CreatedAt = request.CreatedAt.ToDateTime(),
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new CreateProductResponse { Id = product.Id };
        }
        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }
            var productResponse = new GetProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ColorSet = { product.ColorSet.Split(',') }
            };
            //productResponse.ColorSet.Add(product.ColorSet.Split(','));
            return productResponse;
        }
        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ColorSet = string.Join(",", request.ColorSet);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new UpdateProductResponse { Success = true };
        }
        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new DeleteProductResponse { Success = true };
        }
        [Authorize]
        public override async Task<GetAllProductsResponse> GetAllProducts(GetAllProductsRequest request, ServerCallContext context)
        {
            var productsQuery = _context.Products.AsQueryable();
            var products = await productsQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            var response = new GetAllProductsResponse();
            response.Products.AddRange(products.Select(p => new GetProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ColorSet = { p.ColorSet.Split(",") },
                CreatedAt = Timestamp.FromDateTime(p.CreatedAt.ToUniversalTime())//datetime should be in utc mode when converting to timestamp like: p.CreatedAt.ToUniversalTime()
            }));
            return response;
        }
    }
}
