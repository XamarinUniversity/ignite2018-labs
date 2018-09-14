using System;
using System.Collections.Generic;
using System.Text;
using XamarinUniversity.Infrastructure;

namespace MyCircle.ViewModels
{
    public sealed class ProfileColorViewModel : SimpleViewModel
    {
        public string Color { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set => SetPropertyValue(ref isSelected, value);
        }

        private bool isSelected;
    }
}
