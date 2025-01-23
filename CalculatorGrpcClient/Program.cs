using Grpc.Net.Client;
using GrpcExample.Protos;

using var grpcChannel = GrpcChannel.ForAddress("https://localhost:7213/");
var client = new CalculatorService.CalculatorServiceClient(grpcChannel);
SumRequest sumRequest = new()
{
    FirstNumber = 105600,
    SecondNumber = 89700
};
SumResponse sumResponse = client.Sum(sumRequest);
Console.WriteLine(sumResponse.Result);
Console.ReadKey();
