using Engine.Models;
using Engine.ViewModel;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WinForms = System.Windows.Forms;

namespace WPFUI.Windows
{
    public partial class AnimationWindowExperiment : Window
    {
        UniverseViewModel ViewModel;
        DispatcherTimer _timer = new DispatcherTimer();
        private bool _isPaused = false;

        public AnimationWindowExperiment(Universe universe)
        {
            InitializeComponent();
            ViewModel = (UniverseViewModel)DataContext;
            ViewModel.FieldWidth = MainEllipseList.ActualWidth;
            ViewModel.FieldHeight = MainEllipseList.ActualHeight;
            ViewModel.CurrentUniverse = universe;
            
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ViewModel.FieldWidth = MainEllipseList.ActualWidth;
            ViewModel.FieldHeight = MainEllipseList.ActualHeight;
            ViewModel.Update();
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Implement as a DialogService!

            WinForms.SaveFileDialog dialog = new WinForms.SaveFileDialog();
            //string json;

            dialog.Filter = "Map files (*.gmap)|*.gmap";
            dialog.DefaultExt = "gmap";
            dialog.AddExtension = true;
            PauseButton_Click(this, new RoutedEventArgs());

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
            {
                //We need to use these settings because in _universe.Bodies
                //we have List that consists not only of MaterialPoints
                //but also of PhysicalBody inherited from MaterialPoints.
                //As a result, when we serialize - we serialize PhysicalBodies
                //but when we deserialize - we deserialize MaterialPoints because
                //Bodies is a List<MaterialPoint>, so we lose properties of Physical body.
                //
                //The same thing need to be done when we open universe
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                //json = JsonConvert.SerializeObject(_universe, settings);
                //File.WriteAllText(dialog.FileName, json);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPaused)
            {
                PauseButton.Content = "Continue";
                _timer.Stop();
                _isPaused = true;
            }
            else if (_isPaused)
            {
                PauseButton.Content = "Pause";
                _timer.Start();
                _isPaused = false;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _timer.Stop();
        }
    }
}
