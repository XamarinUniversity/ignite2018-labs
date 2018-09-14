using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCircle.Services
{
    public class CircleMessage : IEquatable<CircleMessage>
    {
        private string color;

        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool IsRoot { get; set; }
        public string ThreadId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Version { get; set; }
        public string Color
        {
            get { return color ?? "Black"; }
            set { color = value; }
        }

        public CircleMessage() : this(null)
        {
        }

        public CircleMessage(string parentId)
        {
            //Id = Guid.NewGuid().ToString();
            ThreadId = parentId ?? Guid.NewGuid().ToString();
            //CreatedAt = DateTime.Now;
        }

        public bool Equals(CircleMessage other)
        {
            return other.Id == this.Id
                && other.Text == this.Text
                && other.Color == this.Color
                && other.ThreadId == this.ThreadId;
        }
    }
}