using Grpc.Core;
using Microsoft.Extensions.Logging;
using senkovlad.chat.shared;
using System.Threading.Tasks;

namespace senkovlad.chat.backend
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Grpc reply: " + request.Name
            });
        }
    }
}
