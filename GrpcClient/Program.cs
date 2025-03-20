
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcAuthenticaion;
using GrpcCrudService;


var channel = GrpcChannel.ForAddress("https://localhost:7213");
var client = new productService.productServiceClient(channel);
var authClient = new AuthService.AuthServiceClient(channel);
var token = (await authClient.GenerateTokenAsync(new Empty())).Token;
var headers = new Metadata {
    {"Authorization","Bearer "+token }
};
//var product1 = await client.CreateProductAsync(new CreateProductRequest()
//{
//    Name = "shoe",
//    Description = "sport shoes",
//    Price = 20000,
//    ColorSet = { "red", "black" },
//    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
//}, headers);
//var product2 = await client.CreateProductAsync(new CreateProductRequest()
//{
//    Name = "bag",
//    Description = "sport backpack",
//    Price = 300000,
//    ColorSet = { "yellow", "green" }
//});
var pagination = new GetAllProductsRequest();
pagination.PageSize = 10;
pagination.PageNumber = 1;
foreach (var product in (await client.GetAllProductsAsync(pagination, headers)).Products)
{
    Console.WriteLine(product);
}
Console.ReadKey();