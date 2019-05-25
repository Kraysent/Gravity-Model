using Engine.Models;
using Engine.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private readonly Session _session = SessionFactory.StartMultiCircleSystem(2e11, 1, 20);
        private readonly List<Ellipse> _bodies = new List<Ellipse>();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private const double _scale = 1e-9;
        private const double _bias = 100;
        private double _deltaTime = 3600;
        private const double _massScale = 1e21;
        private const int _speed = 2;
        private int _epoch;
        private bool _isPaused;
        
        public MainWindow()
        {
            InitializeComponent();

            _session.BodyAdded += Session_BodyAdded;
            _session.BodyDeleted += Session_BodyDeleted;
            
            _epoch = 0;
            _isPaused = false;

            //First initialisation of objects
            for (int i = 0; i < _session.Bodies.Count; i++)
            {
                Session_BodyAdded(_session.Bodies[i]);
            }
            
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _timer.Start();
        }
        
        private void Session_BodyAdded(MaterialPoint body)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = Math.Log10(body.Mass / _massScale) * 2;
            ellipse.Height = Math.Log10(body.Mass / _massScale) * 2;
            ellipse.Fill = Brushes.Gray;
            ellipse.Stroke = Brushes.Black;
            
            _bodies.Add(ellipse);
            MainCanvas.Children.Add(ellipse);
        }

        private void Session_BodyDeleted(int bodyNumber)
        {
            MainCanvas.Children.Remove(_bodies[bodyNumber]);
            _bodies.RemoveAt(bodyNumber);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Ellipse p;
            int i;

            try
            {
                for (i = 0; i < _speed; i++)
                {
                    _session.UpdateField(_deltaTime);
                    _epoch++;
                }

                EpochLabel.Content = $"Epoch: year {Math.Round((_epoch * _deltaTime) / (3600 * 24 * 365), 3)}";
                BodiesLabel.Content = $"Number of bodies: {_session.Bodies.Count}";

                for (i = 0; i < _bodies.Count; i++)
                {
                    if (_epoch % 5 == 0)
                    {
                        p = new Ellipse();
                        p.Width = 1;
                        p.Height = 1;
                        p.Fill = Brushes.Black;
                        p.Stroke = Brushes.Black;
                        Canvas.SetLeft(p, _session.Bodies[i].Coordinates.X * _scale + _bias);
                        Canvas.SetTop(p, _session.Bodies[i].Coordinates.Y * _scale + _bias);
                        MainCanvas.Children.Add(p);
                    }

                    Canvas.SetLeft(_bodies[i], _session.Bodies[i].Coordinates.X * _scale + _bias - _bodies[i].Width / 2);
                    Canvas.SetTop(_bodies[i], _session.Bodies[i].Coordinates.Y * _scale + _bias - _bodies[i].Height / 2);
                }
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
    }
}
