using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MatchingGame.Models
{
    public partial class Card : ObservableObject
    {
        [ObservableProperty]
        private string _matchingId;

        [ObservableProperty]
        private string _image;

        [ObservableProperty]
        private Visibility _visible = Visibility.Hidden;
    }
}
