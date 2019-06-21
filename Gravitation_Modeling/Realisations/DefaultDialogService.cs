using Engine.ViewModel;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace WPFUI.Realisations
{
    class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            WinForms.OpenFileDialog dialog = new WinForms.OpenFileDialog();

            if (dialog.ShowDialog() == WinForms.DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
            {
                FilePath = dialog.FileName;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveFileDialog()
        {
            throw new System.NotImplementedException();
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }
    }
}
