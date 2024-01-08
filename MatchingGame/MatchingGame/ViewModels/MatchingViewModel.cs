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
        private HashSet<Char> CharPair { get; set; } = new HashSet<char>();

        private Card PreviousCard = null;

        [ObservableProperty]
        private bool _isEnabled = true;

        [ObservableProperty]
        private ObservableCollection<Card> _cards = new ObservableCollection<Card>();

        [ObservableProperty]
        private Card _selectedCard = new Card();
        partial void OnSelectedCardChanged(Card? oldValue, Card newValue)
        {
            if (newValue?.Visible == Visibility.Visible)
            {
                return;
            }

            if (newValue != null)
            {
                newValue.Visible = System.Windows.Visibility.Visible;
            }

            if (PreviousCard != null && newValue != null)
            {
                if (PreviousCard.MatchingId == newValue.MatchingId && newValue.MatchingId != 'x')
                {
                    char c = PreviousCard.MatchingId;
                    PreviousCard.IsMatched = true;
                    newValue.IsMatched = true;
                    PreviousCard = null;
                    SelectedCard = null;
                    if (IsFinish(c))
                    {
                        Task.Run(() =>
                        {
                            Debug.WriteLine($"FINISH");
                        });
                    }
                }
                else
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
            }

            PreviousCard = newValue;
            if (PreviousCard?.IsMatched == true)
            {
                PreviousCard = null;
            }
        }

        public MatchingViewModel()
        {
            HashSet<Char> charPair = new HashSet<char>();
            string[] imageSrcList = Directory.GetFiles("Images");
            foreach (string path in imageSrcList)
            {
                var card = new Card();
                card.FileName = Path.GetFileName(path);
                card.MatchingId = card.FileName == null ? 'x' : card.FileName.ToLower()[0];
                card.Image = Path.GetFullPath(path);
                Cards.Add(card);

                if (charPair.Contains(card.MatchingId))
                {
                    CharPair.Add(card.MatchingId);
                }
                charPair.Add(card.MatchingId);
            }
        }

        private bool IsFinish(char id)
        {
            CharPair.Remove(id);

            if (CharPair.Count() == 0)
            {
                return true;
            }

            return false;
        }
    }
}
