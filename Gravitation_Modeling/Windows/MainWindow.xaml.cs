using Engine.ViewModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Realisations;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel;

        public MainWindow()
        {
            ViewModel = new MainViewModel(new DefaultDialogService(), new JsonFileService());
            DataContext = ViewModel;
            InitializeComponent();
        }
        
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            Universe universe = (sender as Button).DataContext as Universe;
            AnimationWindow window = new AnimationWindow(universe);

            window.ShowDialog();
        }

        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            Universe universe = (sender as Button).DataContext as Universe;
            AdjustmentWindow window = new AdjustmentWindow(universe);

            window.ShowDialog();
        }
        
        private void OpenMapButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenUniverse();
        }

        private void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.CreateMapWindow window = new Windows.CreateMapWindow();

            window.ShowDialog();
        }
    }
}
