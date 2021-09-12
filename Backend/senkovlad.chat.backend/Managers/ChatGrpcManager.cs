using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using senkovlad.chat.data;
using senkovlad.chat.data.Models;
using senkovlad.chat.shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace senkovlad.chat.backend.Managers
{
    public class ChatGrpcManager
    {
        private readonly AppDbContext _dbContext;
        public event Func<Message, Task> MessageSent;
        public List<IServerStreamWriter<ChatMessage>> listeners;

        public ChatGrpcManager(AppDbContext dbContext)
        {
            listeners = new List<IServerStreamWriter<ChatMessage>>();
            _dbContext = dbContext;
            MessageSent += ChatRoomService__messageSent;
        }

        public async Task<IEnumerable<data.Models.User>> GetAllUserAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        private async Task ChatRoomService__messageSent(Message chatMessage)
        {
            foreach (var listener in listeners)
            {
                await listener.WriteAsync(new ChatMessage
                {
                    Msg = chatMessage.Text,
                    From = chatMessage.From,
                    Time = chatMessage.Time.ToLongDateString()
                });
            }
        }

        public async Task<data.Models.User> GetUserByNameAsync(string name)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Name == name);
            return user;
        }

        public async Task AddUserAsync(data.Models.User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        public async Task AddMessage(Message messageModel)
        {
            await _dbContext.AddAsync(messageModel);
            await _dbContext.SaveChangesAsync();
            await MessageSent.Invoke(messageModel);
        }
    }
}
