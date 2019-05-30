using Engine.ViewModel;
using Gravitation_Modeling;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private List<Universe> _universes = new List<Universe>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUniversesList();
            
            foreach (Universe universe in _universes)
            {
                MapsListView.Items.Add(new UniverseDescription { Name = universe.Name });
            }
        }
        
        public void InitializeUniversesList()
        {
            _universes.Add(UniverseFactory.StartSolarSystem());
            _universes.Add(UniverseFactory.StartJupiterSystem());
            _universes.Add(UniverseFactory.StartSquareSystem());
            _universes.Add(UniverseFactory.StartRandomSystem(100));
            _universes.Add(UniverseFactory.StartCircleSystem(50));
            _universes.Add(UniverseFactory.StartMultiCircleSystem(5e11, 2, 20));
            _universes.Add(UniverseFactory.StartGalaxySystem(100));
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            string mapName = ((sender as Button).DataContext as UniverseDescription).Name;

            foreach (Universe universe in _universes)
            {
                if (universe.Name == mapName)
                {
                    AnimationWindow window = new AnimationWindow(universe);
                    window.Show();
                }
            }
        }

        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            string mapName = ((sender as Button).DataContext as UniverseDescription).Name;

            foreach (Universe universe in _universes)
            {
                if (universe.Name == mapName)
                {
                    AdjustmentWindow window = new AdjustmentWindow(universe);

                    window.ShowDialog();
                }
            }
        }

        private class UniverseDescription
        {
            public string Name { get; set; }
        }
    }
}
