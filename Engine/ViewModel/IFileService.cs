using Engine.Models;

namespace Engine.ViewModel
{
    public interface IFileService
    {
        Universe Open(string filename);

        void Save(string filename, Universe universe);
    }
}
