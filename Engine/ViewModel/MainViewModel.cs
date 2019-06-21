using System.Collections.ObjectModel;

namespace Engine.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Universe> Universes { get; set; } = new ObservableCollection<Universe>();

        public MainViewModel()
        {
            InitializeUniversesList();
        }

        private void InitializeUniversesList()
        {
            AddUniverse(UniverseFactory.StartSolarSystem());
            AddUniverse(UniverseFactory.StartJupiterSystem());
            AddUniverse(UniverseFactory.StartSquareSystem());
            AddUniverse(UniverseFactory.StartRandomSystem(100));
            AddUniverse(UniverseFactory.StartCircleSystem(20));
            AddUniverse(UniverseFactory.StartMultiCircleSystem(5e11, 2, 20));
            AddUniverse(UniverseFactory.StartGalaxySystem(100));
        }

        public void AddUniverse(Universe universe)
        {
            Universes.Add(universe);
        }
    }
}
