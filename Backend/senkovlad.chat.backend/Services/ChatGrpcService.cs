using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using senkovlad.chat.backend.Managers;
using senkovlad.chat.data.Models;
using senkovlad.chat.shared;

namespace senkovlad.chat.backend.Services
{
    public class ChatGrpcService : ChatService.ChatServiceBase
    {
        private readonly ChatGrpcManager _chatGrpcManager;

        public ChatGrpcService(ChatGrpcManager chatGrpcManager)
        {
            _chatGrpcManager = chatGrpcManager;
        }

        public async override Task<UserList> getAllUsers(Empty request, ServerCallContext context)
        {
            var usersModel = await _chatGrpcManager.GetAllUserAsync();
            var users = usersModel.Select(user => new shared.User
            {
                Id = user.Id,
                Name = user.Name
            });

            var response = new UserList();
            response.Users.Add(users);
            return response;
        }

        public async override Task<JoinResponse> join(shared.User request, ServerCallContext context)
        {
            var user = await _chatGrpcManager.GetUserByNameAsync(request.Name);
            if(user != null)
            {
                return new JoinResponse
                {
                    Error = 1,
                    Msg = "User already exist"
                };
            }
            var newUser = new data.Models.User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };
            await _chatGrpcManager.AddUserAsync(newUser);

            return new JoinResponse
            {
                Error = 0,
                Msg = "Success"
            };
        }

        public async override Task reciveMsg(Empty request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            _chatGrpcManager.listeners.Add(responseStream);

            //foreach (var message in await _chatGrpcManager.GetMessagesAsync())
            //{
            //    await responseStream.WriteAsync(new ChatMessage
            //    {
            //        From = message.From,
            //        Time = message.Time.ToLongDateString(),
            //        Msg = message.Text
            //    });
            //}

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
            _chatGrpcManager.listeners.Remove(responseStream);
        }

        public async override Task<Empty> sendMessage(ChatMessage request, ServerCallContext context)
        {
            var messageModel = new Message
            {
                Id = Guid.NewGuid().ToString(),
                From = request.From,
                Text = request.Msg,
                Time = Convert.ToDateTime(request.Time)
            };
            await _chatGrpcManager.AddMessage(messageModel);
            return new Empty { };
        }
    }
}
