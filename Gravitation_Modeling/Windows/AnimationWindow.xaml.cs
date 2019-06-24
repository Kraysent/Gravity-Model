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
    public partial class AnimationWindow : Window
    {
        /*TODO: 
         * Different colors for Body instances (and their view)
        */

        public UniverseViewModel ViewModel { get; set; }
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isPaused = false;

        public AnimationWindow(Universe universe)
        {
            InitializeComponent();
            ViewModel = (UniverseViewModel)DataContext;
            ViewModel.CurrentUniverse = universe;
            
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ViewModel.FieldWidth != MainEllipseList.ActualWidth && ViewModel.FieldHeight != MainEllipseList.ActualHeight)
            {
                ViewModel.FieldWidth = MainEllipseList.ActualWidth;
                ViewModel.FieldHeight = MainEllipseList.ActualHeight;
            }

            try
            {
                ViewModel.Update();
            }
            catch { }
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

        private void Window_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ViewModel.ChangeFOV(true);
            }
            else
            {
                ViewModel.ChangeFOV(false);
            }
        }
    }
}
