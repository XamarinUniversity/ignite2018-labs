using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCircle.Services
{
    public class CircleMessage
    {
        private string color;

        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool IsRoot { get; set; }
        public string ThreadId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Color
        {
            get { return color ?? "Black"; }
            set { color = value; }
        }

        public CircleMessage(string parentId = null)
        {
            //Id = Guid.NewGuid().ToString();
            ThreadId = parentId ?? Guid.NewGuid().ToString();
            //CreatedAt = DateTime.Now;
        }
    }
}