using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCircle.Services
{
    public sealed class CircleMessage
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRoot { get; set; }
        public string ThreadId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        public CircleMessage(string parentId = null)
        {
            Id = Guid.NewGuid().ToString();
            ThreadId = parentId ?? Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }
}