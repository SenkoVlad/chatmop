using System;

namespace senkovlad.chat.data.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string From { get; set; }
    }
}