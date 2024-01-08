using CommunityToolkit.Mvvm.ComponentModel;
using MatchingGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MatchingGame.ViewModels
{
    public partial class MatchingViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Card> _cards = new ObservableCollection<Card>();

        [ObservableProperty]
        private string _hint = "";

        public MatchingViewModel()
        {
            string[] imageSrcList = Directory.GetFiles("Images");
            foreach (string path in imageSrcList)
            {
                var card = new Card();
                card.MatchingId = Path.GetFileName(path);
                card.Image = Path.GetFullPath(path);
                Cards.Add(card);
            }

            Hint = $"AFAFA {Cards.Count()}";
        }
    }
}
