using Grpc.Core;
using Microsoft.Extensions.Logging;
using senkovlad.chat.shared;
using System.Threading.Tasks;

namespace senkovlad.chat.backend
{
    public class GreeterService : Greeter.GreeterBase
    {
        public GreeterService()
        {
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Grpc reply: " + request.Name
            });
        }

        public override async Task JoinChat(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var messages = new string[]
            {
                "Message1",
                "Message2",
                "Message3",
                "Message4"
            };
            int index = messages.Length + 1;

            foreach (var message in messages)
            {
                await responseStream.WriteAsync(new HelloReply
                {   
                    Message = message
                });
            }

            while(!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = "Message" + index++
                });
                await Task.Delay(2000, context.CancellationToken);
            }
        }
    }
}
