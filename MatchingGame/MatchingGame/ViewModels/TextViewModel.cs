using CommunityToolkit.Mvvm.ComponentModel;
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
        private string msg = "第一次出去玩";
        ConcurrentDictionary<string, string> ImageTextDict = new ConcurrentDictionary<string, string>();

        [ObservableProperty]
        private string _text = "";

        public TextViewModel()
        {
            HashSet<Char> charPair = new HashSet<char>();
            string[] filePaths = Directory.GetFiles("Texts");
            foreach (string path in filePaths)
            {
                ImageTextDict.TryAdd(Path.GetFileName(path), File.ReadAllText(path));
            }
        }

        public void Print()
        {
            Task.Run(() =>
            {
                foreach (string t in ImageTextDict.Values)
                {
                    foreach (var s in t)
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
            });
        }
    }
}
