using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using senkovlad.chat.data;
using senkovlad.chat.data.Models;
using senkovlad.chat.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senkovlad.chat.backend.Managers
{
    public class ChatRoomManager
    {
        private readonly AppDbContext _dbContext;
        public event Func<string, Task> MessageSent;
        public List<IServerStreamWriter<ChatMessageResponse>> listeners;

        public ChatRoomManager(AppDbContext dbContext)
        {
            listeners = new List<IServerStreamWriter<ChatMessageResponse>>();
            _dbContext = dbContext;
            MessageSent += ChatRoomService__messageSent;
        }

        private async Task ChatRoomService__messageSent(string message)
        {
            foreach (var listener in listeners)
            {
                await listener.WriteAsync(new ChatMessageResponse
                {
                    Message = message
                });
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        public async Task AddMessage(Message messageModel)
        {
            await _dbContext.AddAsync(messageModel);
            await _dbContext.SaveChangesAsync();
            await MessageSent.Invoke(messageModel.Text);
        }
    }
}
