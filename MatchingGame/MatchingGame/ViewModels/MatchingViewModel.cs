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
using System.Windows;
using System.Windows.Media.Imaging;

namespace MatchingGame.ViewModels
{
    public partial class MatchingViewModel : ObservableObject
    {
        private Card PreviousCard = null;

        [ObservableProperty]
        private bool _isEnabled = true;

        [ObservableProperty]
        private ObservableCollection<Card> _cards = new ObservableCollection<Card>();

        [ObservableProperty]
        private Card _selectedCard = new Card();
        partial void OnSelectedCardChanged(Card? oldValue, Card newValue)
        {
            Debug.WriteLine($"{oldValue?.MatchingId} {newValue?.MatchingId}");
            if (newValue != null)
            {
                newValue.Visible = System.Windows.Visibility.Visible;
            }

            if (PreviousCard != null && newValue != null)
            {
                Card preCard = PreviousCard;
                IsEnabled = false;
                Task.Run(() =>
                {
                    SpinWait.SpinUntil(()=>false, 1000);

                    Application.Current.Dispatcher.Invoke(() => { 
                        if (preCard != null)
                        {
                            preCard.Visible = System.Windows.Visibility.Hidden;
                            preCard = null;
                        }
                        if (SelectedCard != null)
                        {
                            SelectedCard.Visible = System.Windows.Visibility.Hidden;
                            SelectedCard = null;
                        }
                        IsEnabled = true;
                    });
                });
            }
            PreviousCard = newValue;
        }

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
        }
    }
}
