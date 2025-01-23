using Grpc.Core;
using GrpcExample.Protos;

namespace GrpcExample.GrpcServices
{
    public class CalculatorGrpcService : CalculatorService.CalculatorServiceBase
    {
        public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
        {
            long sum = request.FirstNumber + request.SecondNumber;
            SumResponse response = new()
            {
                Result = sum
            };
            return Task.FromResult(response);
        }
    }
}
