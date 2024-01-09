using MatchingGame.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MatchingViewModel MatchingViewModel = new MatchingViewModel();
        public static TextViewModel TextViewModel = new TextViewModel();

        public MainWindow()
        {
            InitializeComponent();

            MV.DataContext = MatchingViewModel;
            TV.DataContext = TextViewModel;
        }
    }
}