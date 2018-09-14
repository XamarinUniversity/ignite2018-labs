using System;
using System.Collections.Generic;
using System.Text;

namespace MyCircle.Services
{
    public static class UserInfo
    {
        private const string UserNameKey = nameof(UserName);
        private const string ColorKey = nameof(Color);

        public static string UserName
        {
            get => (App.Current.Properties.ContainsKey(UserNameKey))
                    ? App.Current.Properties[UserNameKey].ToString()
                    : null;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    App.Current.Properties.Remove(UserNameKey);
                }
                else
                {
                    App.Current.Properties[UserNameKey] = value;
                }
            }
        }

        public static string Color
        {
            get => (App.Current.Properties.ContainsKey(ColorKey))
                   ? App.Current.Properties[ColorKey].ToString()
                   : "Black";

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    App.Current.Properties.Remove(ColorKey);
                }
                else
                {
                    App.Current.Properties[ColorKey] = value;
                }
            }
        }
    }
}
