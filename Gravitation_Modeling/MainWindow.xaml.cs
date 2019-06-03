using Engine.ViewModel;
using Gravitation_Modeling;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private List<Universe> _universes = new List<Universe>();
        private List<UniverseDescription> _descriptions = new List<UniverseDescription>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUniversesList();
        }
        
        public void InitializeUniversesList()
        {
            AddUniverse(UniverseFactory.StartSolarSystem());
            AddUniverse(UniverseFactory.StartJupiterSystem());
            AddUniverse(UniverseFactory.StartSquareSystem());
            AddUniverse(UniverseFactory.StartRandomSystem(100));
            AddUniverse(UniverseFactory.StartCircleSystem(20));
            AddUniverse(UniverseFactory.StartMultiCircleSystem(5e11, 2, 20));
            AddUniverse(UniverseFactory.StartGalaxySystem(100));
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            int mapHash = ((sender as Button).DataContext as UniverseDescription).Hash;

            foreach (Universe universe in _universes)
            {
                if (universe.GetHashCode() == mapHash)
                {
                    AnimationWindow window = new AnimationWindow(universe);
                    window.Show();
                }
            }
        }

        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            int mapHash = ((sender as Button).DataContext as UniverseDescription).Hash;
            //MessageBox.Show(mapHash.ToString());
            foreach (Universe universe in _universes)
            {
                if (universe.GetHashCode() == mapHash)
                {
                    AdjustmentWindow window = new AdjustmentWindow(universe);

                    window.ShowDialog();
                }
            }
        }

        private void AddUniverse(Universe universe)
        {
            if (!_universes.Any(x => x.GetHashCode() == universe.GetHashCode()))
            {
                UniverseDescription descr = new UniverseDescription(universe.Name, universe.GetHashCode());

                _universes.Add(universe);
                _descriptions.Add(descr);
                MapsListView.Items.Add(descr);
            }
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
                    AddUniverse(selectedUniverse);

                }
                catch
                {
                    MessageBox.Show("Can not open the file!", "Error!");
                }
            }
        }

        private class UniverseDescription
        {
            public string Name { get; set; }
            public int Hash { get; set; }

            public UniverseDescription(string name, int hash)
            {
                Name = name;
                Hash = hash;
            }
        }
    }
}
