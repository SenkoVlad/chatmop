using Grpc.Core;
using Microsoft.Extensions.Logging;
using senkovlad.chat.backend.Managers;
using senkovlad.chat.data;
using senkovlad.chat.data.Models;
using senkovlad.chat.shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace senkovlad.chat.backend.Services
{
    public class ChatRoomService : ChatRoom.ChatRoomBase
    {
        private readonly ILogger<ChatRoomService> _logger;
        private ChatRoomManager _chatRoomManager;

        public ChatRoomService(ILogger<ChatRoomService> logger, ChatRoomManager chatRoomManager)
        {
            _logger = logger;
            _chatRoomManager = chatRoomManager;
        }
        public override async Task JoinChat(ChatMessageRequest request, IServerStreamWriter<ChatMessageResponse> responseStream, ServerCallContext context)
        {
            _chatRoomManager.listeners.Add(responseStream);

            foreach (var message in await _chatRoomManager.GetMessagesAsync())
            {
                await responseStream.WriteAsync(new ChatMessageResponse
                {
                    Message = message.Text
                });
            }

            while(!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
            _chatRoomManager.listeners.Remove(responseStream);
        }

        public override async Task<ChatMessageResponse> SendMessage(ChatMessageRequest request, ServerCallContext context)
        {
            var messageModel = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Text = request.MessageText
            };
            await _chatRoomManager.AddMessage(messageModel);
            return new ChatMessageResponse
            {
                Message = request.MessageText
            };
        }
    }
}
