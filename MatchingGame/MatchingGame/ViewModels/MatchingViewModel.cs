﻿using CommunityToolkit.Mvvm.ComponentModel;
using MatchingGame.Models;
using System;
using System.Collections.Concurrent;
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

        [ObservableProperty]
        private int _leftCount = 0;

        private Card PreviousCard = null;
        public ConcurrentDictionary<string, Card> PathCardDict { get; set; } = new ConcurrentDictionary<string, Card>();

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
                        foreach (var card in Cards)
                        {
                            card.Visible = Visibility.Visible;
                        }
                        MainWindow.TextViewModel.Print();
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

        [ObservableProperty]
        private Visibility _photoVisibility = Visibility.Collapsed;
        [ObservableProperty]
        private string _photoPath = "";

        public MatchingViewModel()
        {
            HashSet<Char> charPair = new HashSet<char>();
            string[] imageSrcList = Directory.GetFiles("Images");
            foreach (string path in imageSrcList)
            {
                var card = new Card();
                card.FileName = Path.GetFileNameWithoutExtension(path);
                card.MatchingId = card.FileName == null ? 'x' : card.FileName.ToLower()[0];
                card.Image = Path.GetFullPath(path);
                Cards.Add(card);
                PathCardDict.TryAdd(card.FileName, card);

                if (charPair.Contains(card.MatchingId))
                {
                    CharPair.Add(card.MatchingId);
                    LeftCount = CharPair.Count();
                }
                charPair.Add(card.MatchingId);
            }
            SelectedCard = null;
            ShuffleExtension.Shuffle(Cards);
        }

        private bool IsFinish(char id)
        {
            CharPair.Remove(id);
            LeftCount = CharPair.Count();

            if (CharPair.Count() == 0)
            {
                return true;
            }

            return false;
        }
    }
}
