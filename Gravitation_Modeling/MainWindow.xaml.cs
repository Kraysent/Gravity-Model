﻿using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using Engine.ViewModel;
using Engine.Models;
using Vector2 = Engine.Models.Vector;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private List<Ellipse> _bodies;
        private DispatcherTimer _timer;
        private Session _session;
        private double _scale = 9.5e-10;
        private double _bias = 100;
        private double _deltaTime = 6 * 3600;
        private int _epoch;
        private bool _isPaused = false;
        
        public MainWindow()
        {
            InitializeComponent();
            AddButton.IsEnabled = false;

            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
            _epoch = 0;
            _session = SessionFactory.StartRandomSystem(15);
            _bodies = new List<Ellipse>();

            for (int i = 0; i < _session.Bodies.Count; i++)
            {
                Ellipse body = new Ellipse();
                body.Width = Math.Log10(_session.Bodies[i].Mass / 1e27) * 5;
                body.Height = Math.Log10(_session.Bodies[i].Mass / 1e27) * 5;
                body.Fill = Brushes.Gray;
                body.Stroke = Brushes.Black;

                _bodies.Add(body);
                MainCanvas.Children.Add(_bodies[i]);
            }

            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int i;

            _session.UpdateField(_deltaTime);
            _epoch++;
            EpochLabel.Content = $"Epoch: year {Math.Round((_epoch * _deltaTime) / (3600 * 24 * 365), 3)}";
            
            for (i = 0; i < _bodies.Count; i++)
            {
                if (_epoch % 5 == 0)
                {
                    Ellipse p = new Ellipse();
                    p.Width = 1;
                    p.Height = 1;
                    p.Fill = Brushes.Black;
                    p.Stroke = Brushes.Black;
                    Canvas.SetLeft(p, _session.Bodies[i].Coordinates.X * _scale + _bias);
                    Canvas.SetTop(p, _session.Bodies[i].Coordinates.Y * _scale + _bias);
                    MainCanvas.Children.Add(p);
                }
                
                Canvas.SetLeft(_bodies[i], _session.Bodies[i].Coordinates.X * _scale + _bias);
                Canvas.SetTop(_bodies[i], _session.Bodies[i].Coordinates.Y * _scale + _bias);
            }
        }

        private void Add(MaterialPoint b)
        {
            _session.Add(b);

            Ellipse body = new Ellipse();
            body.Width = 10;
            body.Height = 10;
            body.Fill = Brushes.AliceBlue;
            body.Stroke = Brushes.Black;

            _bodies.Add(body);
            MainCanvas.Children.Add(_bodies[_bodies.Count - 1]);
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            Add(new MaterialPoint
            {
                Mass = double.Parse(MassTextBox.Text),
                Coordinates = new Vector2(rnd.NextDouble() *  ActualWidth / _scale, rnd.NextDouble() * ActualHeight / _scale),
                Velocity = new Vector2(rnd.NextDouble() * 1000 / _scale * Math.Pow(-1, rnd.Next(1, 4)), rnd.NextDouble() * 1000 / _scale * Math.Pow(-1, rnd.Next(1, 4)))
            });
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
