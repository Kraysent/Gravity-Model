using Engine.Factories;
using Engine.Models;
using System.Collections.ObjectModel;

namespace Engine.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Universe> Universes { get; set; } = new ObservableCollection<Universe>();
        public IDialogService DialogService { get; set; }
        public IFileService FileService { get; set; }
        
        public MainViewModel(IDialogService dialogService, IFileService fileService)
        {
            DialogService = dialogService;
            FileService = fileService;
            InitializeUniversesList();
        }
        
        public void AddUniverse(Universe universe)
        {
            Universes.Add(universe);
        }

        public void OpenUniverse()
        {
            Universe selectedUniverse;

            if (DialogService.OpenFileDialog() == true)
            {
                selectedUniverse = FileService.Open(DialogService.FilePath);

                if (selectedUniverse == null)
                {
                    DialogService.ShowMessage("Error in reading file!", "Error");
                }
                else
                {
                    AddUniverse(selectedUniverse);
                }
            }
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
    }
}
