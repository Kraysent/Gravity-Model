using Engine.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel = new MainViewModel();

        public MainWindow()
        {
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
            WinForms.OpenFileDialog dialog = new WinForms.OpenFileDialog();
            string json;
            Universe selectedUniverse;

            if (dialog.ShowDialog() == WinForms.DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
            {
                try
                {
                    json = File.ReadAllText(dialog.FileName);

                    //We need to use these settings because in _universe.Bodies
                    //we have List that consists not only of MaterialPoints
                    //but also of PhysicalBody inherited from MaterialPoints.
                    //As a result, when we serialize - we serialize PhysicalBodies
                    //but when we deserialize - we deserialize MaterialPoints because
                    //Bodies is a List<MaterialPoint>, so we lose properties of Physical body.
                    //
                    //The same thing need to be done when we save universe
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    selectedUniverse = (Universe)JsonConvert.DeserializeObject(json, settings);
                    ViewModel.AddUniverse(selectedUniverse);

                }
                catch
                {
                    MessageBox.Show("Can not open the file!", "Error!");
                }
            }
        }

        private void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.CreateMapWindow window = new Windows.CreateMapWindow();

            window.ShowDialog();
        }
    }
}
