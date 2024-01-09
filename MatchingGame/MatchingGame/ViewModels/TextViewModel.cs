using CommunityToolkit.Mvvm.ComponentModel;
using MatchingGame.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatchingGame.ViewModels
{
    public partial class TextViewModel : ObservableObject
    {
        List<(string, string)> ImageTextDict = new List<(string, string)>();

        [ObservableProperty]
        private string _text = "";

        public TextViewModel()
        {
            HashSet<Char> charPair = new HashSet<char>();
            string[] filePaths = Directory.GetFiles("Texts");
            foreach (string path in filePaths)
            {
                ImageTextDict.Add((Path.GetFileNameWithoutExtension(path), File.ReadAllText(path)));
            }
        }

        public void Print()
        {
            Task.Run(() =>
            {
                SpinWait.SpinUntil(() => false, 2000);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow.MatchingViewModel.PhotoVisibility = Visibility.Visible;
                });
                
                foreach (var pair in ImageTextDict)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (MainWindow.MatchingViewModel.PathCardDict.TryGetValue(pair.Item1, out Card card))
                        {
                            MainWindow.MatchingViewModel.PhotoPath = card.Image;
                        }
                    });

                    foreach (var s in pair.Item2)
                    {
                        SpinWait.SpinUntil(() => false, 150);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Text += s;
                        });
                    }

                    SpinWait.SpinUntil(() => false, 2000);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Text = "";
                    });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow.MatchingViewModel.PhotoVisibility = Visibility.Collapsed;
                });
            });
        }
    }
}
