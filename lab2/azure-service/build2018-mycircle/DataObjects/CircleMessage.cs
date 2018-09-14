using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace build2018_mycircle.DataObjects
{
    public class CircleMessage : EntityData
    {
        public bool IsRoot { get; set; }
        public string ThreadId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
    }
}