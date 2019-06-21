namespace Engine.ViewModel
{
    public interface IDialogService
    {
        string FilePath { get; set; }

        void ShowMessage(string message, string title);

        bool OpenFileDialog(); 

        bool SaveFileDialog();
    }
}
