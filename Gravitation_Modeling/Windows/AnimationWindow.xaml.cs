using Engine.Models;
using Engine.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WinForms = System.Windows.Forms;

namespace WPFUI
{
    public partial class AnimationWindow : Window
    {
        private Universe _universe;
        private List<Ellipse> _bodies = new List<Ellipse>();
        private List<Ellipse> _horizontalProjections = new List<Ellipse>();
        private List<Ellipse> _verticalProjections = new List<Ellipse>();
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isPaused = false;
        private int _fps = 1000;
        
        public AnimationWindow(Universe universe)
        {
            InitializeComponent();
            Title = universe.Name;

            _universe = universe.Clone() as Universe;
            InitializeUniverse();
            
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(_fps / 1000);
            _timer.Start();
        }
        
        public void InitializeUniverse()
        {
            _universe.BodyAdded += Universe_BodyAdded;
            _universe.BodyDeleted += Universe_BodyDeleted;

            //First initialisation of objects
            for (int i = 0; i < _universe.Bodies.Count; i++)
            {
                Universe_BodyAdded(_universe.Bodies[i]);
            }
        }
        
        private void Universe_BodyAdded(MaterialPoint body)
        {
            Ellipse ellipse = new Ellipse();
            Ellipse hEllipse, vEllipse;
            Random rnd = new Random(DateTime.Now.Millisecond);
            Brush currBrush;

            if (body is PhysicalBody)
            {
                ellipse.Width = Sigma(((PhysicalBody)body).Diameter + 1e9) * 20 + 5;
                ellipse.Height = ellipse.Width;
            }
            else
            {
                ellipse.Width = 10;
                ellipse.Height = 10;
            }

            currBrush = new SolidColorBrush(Color.FromArgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));
            ellipse.Fill = currBrush;
            ellipse.Stroke = Brushes.Black;

            _bodies.Add(ellipse);
            MainCanvas.Children.Add(ellipse);
            
            hEllipse = new Ellipse() { Width = 10, Height = 10, Fill = currBrush, Stroke = Brushes.Black };
            _horizontalProjections.Add(hEllipse);
            HorizontalCanvas.Children.Add(hEllipse);

            vEllipse = new Ellipse() { Width = 10, Height = 10, Fill = currBrush, Stroke = Brushes.Black };
            _verticalProjections.Add(vEllipse);
            VerticalCanvas.Children.Add(vEllipse);
            Thread.Sleep(2);
        }

        private void Universe_BodyDeleted(int bodyNumber)
        {
            MainCanvas.Children.Remove(_bodies[bodyNumber]);
            _bodies.RemoveAt(bodyNumber);
            HorizontalCanvas.Children.Remove(_horizontalProjections[bodyNumber]);
            _horizontalProjections.RemoveAt(bodyNumber);
            VerticalCanvas.Children.Remove(_verticalProjections[bodyNumber]);
            _verticalProjections.RemoveAt(bodyNumber);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                _universe.Update();
                UpdateField();
            }
            catch { }
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WinForms.SaveFileDialog dialog = new WinForms.SaveFileDialog();
            string json;

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

                json = JsonConvert.SerializeObject(_universe, settings);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _timer.Stop();
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _universe.CameraFOV = _universe.CameraFOV / 1.1;
            }
            else if (e.Delta < 0)
            {
                _universe.CameraFOV = _universe.CameraFOV * 1.1;
            }

            UpdateField();
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateField()
        {
            int i;
            double width = MainCanvas.ActualWidth;
            double height = MainCanvas.ActualHeight;
            double scale = Math.Min(Height, width) / _universe.CameraFOV;
            double xBias = width / 2;
            double yBias = Height / 2;
            Ellipse p;

            EpochLabel.Content = $"Epoch: year {Math.Round((_universe.Epoch) / (3600 * 24 * 365), 3)}";
            BodiesLabel.Content = $"Number of bodies: {_universe.Bodies.Count}";
            FPSLabel.Content = $"FPS: {_fps}";
            FieldWidthLabel.Content = $"Width: {(width / Math.Max(width, height) * _universe.CameraFOV).ToString("E3")} meters";
            FieldHeightLabel.Content = $"Height: {(height / Math.Max(width, height) * _universe.CameraFOV).ToString("E3")} meters";

            for (i = 0; i < _bodies.Count; i++)
            {
                if (_universe.EnableTracers == true && _universe.Epoch % 5 == 0)
                {
                    p = new Ellipse();
                    p.Width = 1;
                    p.Height = 1;
                    p.Fill = Brushes.Black;
                    p.Stroke = Brushes.Black;
                    Canvas.SetLeft(p, _universe.Bodies[i].Coordinates.X * scale + xBias);
                    Canvas.SetTop(p, _universe.Bodies[i].Coordinates.Y * scale + yBias);
                    MainCanvas.Children.Add(p);
                }

                Canvas.SetLeft(_bodies[i], _universe.Bodies[i].Coordinates.X * scale + xBias - _bodies[i].Width / 2);
                Canvas.SetTop(_bodies[i], _universe.Bodies[i].Coordinates.Y * scale + yBias - _bodies[i].Height / 2);

                Canvas.SetLeft(_horizontalProjections[i], _universe.Bodies[i].Coordinates.X * scale + xBias);
                Canvas.SetTop(_horizontalProjections[i], HorizontalCanvas.ActualHeight / 2 - 5);

                Canvas.SetLeft(_verticalProjections[i], VerticalCanvas.ActualWidth / 2 - 5);
                Canvas.SetTop(_verticalProjections[i], _universe.Bodies[i].Coordinates.Y * scale + yBias);
            }
        }

        private double Sigma(double x) => 1 / (1 + Math.Exp(-x));
    }
}
