﻿using Engine.Models;
using Engine.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gravitation_Modeling
{
    public partial class AnimationWindow : Window
    {
        private Universe _universe;
        private List<Ellipse> _bodies = new List<Ellipse>();
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isPaused = false;
        private const double _bias = 0;

        public AnimationWindow(Universe universe)
        {
            InitializeComponent();
            _universe = universe;

            Width = 1500;
            Height = 1000;
            ResizeMode = ResizeMode.CanResize;
            
            InitializeUniverse();

            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(1);
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
        
        private void Universe_BodyAdded(MaterialPoint body)
        {
            Ellipse ellipse = new Ellipse();

            if (body is PhysicalBody)
            {
                ellipse.Width = Math.Log(((PhysicalBody)body).Diameter);
                ellipse.Height = Math.Log(((PhysicalBody)body).Diameter);
            }
            else
            {
                ellipse.Width = 10;
                ellipse.Height = 10;
            }

            ellipse.Fill = Brushes.Gray;
            ellipse.Stroke = Brushes.Black;

            _bodies.Add(ellipse);
            MainCanvas.Children.Add(ellipse);
        }

        private void Universe_BodyDeleted(int bodyNumber)
        {
            MainCanvas.Children.Remove(_bodies[bodyNumber]);
            _bodies.RemoveAt(bodyNumber);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Ellipse p;
            int i;
            double xScale = Math.Min(Height, Width) / _universe.CameraFOVX;
            double yScale = Math.Min(Height, Width) / _universe.CameraFOVY;

            try
            {
                _universe.Update();

                EpochLabel.Content = $"Epoch: year {Math.Round((_universe.Epoch * _universe.DeltaTime) / (3600 * 24 * 365), 3)}";
                BodiesLabel.Content = $"Number of bodies: {_universe.Bodies.Count}";

                for (i = 0; i < _bodies.Count; i++)
                {
                    if (_universe.EnableTracers == true && _universe.Epoch % 5 == 0)
                    {
                        p = new Ellipse();
                        p.Width = 1;
                        p.Height = 1;
                        p.Fill = Brushes.Black;
                        p.Stroke = Brushes.Black;
                        Canvas.SetLeft(p, _universe.Bodies[i].Coordinates.X * xScale + _bias);
                        Canvas.SetTop(p, _universe.Bodies[i].Coordinates.Y * yScale + _bias);
                        MainCanvas.Children.Add(p);
                    }

                    Canvas.SetLeft(_bodies[i], _universe.Bodies[i].Coordinates.X * xScale + _bias - _bodies[i].Width / 2);
                    Canvas.SetTop(_bodies[i], _universe.Bodies[i].Coordinates.Y * yScale + _bias - _bodies[i].Height / 2);
                }
            }
            catch { }
        }
    }
}
