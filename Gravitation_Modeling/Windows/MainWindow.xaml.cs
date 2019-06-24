using Engine.Models;
using Engine.ViewModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Realisations;
using WPFUI.Windows;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = (MainViewModel)DataContext;
            ViewModel.DialogService = new DefaultDialogService();
            ViewModel.FileService = new JsonFileService();
        }
        
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            Universe universe = (sender as Button).DataContext as Universe;
            AnimationWindowExperiment window = new AnimationWindowExperiment(universe);
            //AnimationWindow window = new AnimationWindow(universe);

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
