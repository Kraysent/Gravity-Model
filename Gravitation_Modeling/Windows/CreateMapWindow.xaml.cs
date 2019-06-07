using Engine.Models;
using Engine.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Vector2 = Engine.Models.Vector;
using WinForms = System.Windows.Forms;

namespace WPFUI.Windows
{
    public partial class CreateMapWindow : Window
    {
        //When started, height and width are shown as 0

        private Universe _universe = new Universe();
        private List<Ellipse> _bodies = new List<Ellipse>();
        private SelectedItem _currentItem = SelectedItem.Cursor;

        public CreateMapWindow()
        {
            InitializeComponent();
            _universe.Name = "Some map";
            _universe.EnableTracers = false;
        }
        
        private void NewBodyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentItem != SelectedItem.Body)
            {
                _currentItem = SelectedItem.Body;
            }
            else
            {
                _currentItem = SelectedItem.Cursor;
            }
        }

        private void MainRectangle_PreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            double width = MainCanvas.ActualWidth;
            double height = MainCanvas.ActualHeight;
            double mouseX, mouseY, bodyX, bodyY;
            Ellipse body;

            if (_currentItem == SelectedItem.Body)
            {
                mouseX = e.GetPosition(sender as IInputElement).X;
                mouseY = e.GetPosition(sender as IInputElement).Y;
                bodyX = (mouseX - width / 2) / width * (width / Math.Max(width, height) * _universe.CameraFOV);
                bodyY = (mouseY - height / 2) / height * (height / Math.Max(width, height) * _universe.CameraFOV);

                _universe.Add(new MaterialPoint(new Vector2(bodyX, bodyY), 1e30, new Vector2(0, 0)));

                body = new Ellipse() { Width = 10, Height = 10, Fill = Brushes.DarkGray, Stroke = Brushes.Black };
                body.PreviewMouseDown += BodyEllipse_MouseDown;
                _bodies.Add(body);
                Canvas.SetLeft(body, mouseX - 5);
                Canvas.SetTop(body, mouseY - 5);
                MainCanvas.Children.Add(body);
            }
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
        
        private void BodyEllipse_MouseDown(object sender, MouseEventArgs e)
        {
            Ellipse body = sender as Ellipse;
            double x = Canvas.GetLeft(body), bX;
            double y = Canvas.GetTop(body), bY;
            int i;

            for (i = 0; i < _bodies.Count; i++)
            {
                bX = Canvas.GetLeft(_bodies[i]);
                bY = Canvas.GetTop(_bodies[i]);
                
                if (x == bX && y == bY)
                {
                    break;
                }
            }
        }

        private void SaveMapButton_Click(object sender, RoutedEventArgs e)
        {
            WinForms.SaveFileDialog dialog = new WinForms.SaveFileDialog();
            string json;

            dialog.Filter = "Map files (*.gmap)|*.gmap";
            dialog.DefaultExt = "gmap";
            dialog.AddExtension = true;

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

        private void UpdateField()
        {
            double width = MainCanvas.ActualWidth;
            double height = MainCanvas.ActualHeight;
            double scale = Math.Min(Height, Width) / _universe.CameraFOV;
            double xBias = width / 2;
            double yBias = height / 2;

            WidthLabel.Content = $"Width: {(width / Math.Max(width, height) * _universe.CameraFOV).ToString("E3")} meters";
            HeightLabel.Content = $"Height: {(height / Math.Max(width, height) * _universe.CameraFOV).ToString("E3")} meters";

            for (int i = 0; i < _bodies.Count; i++)
            {
                Canvas.SetLeft(_bodies[i], _universe.Bodies[i].Coordinates.X * scale + xBias - _bodies[i].Width / 2);
                Canvas.SetTop(_bodies[i], _universe.Bodies[i].Coordinates.Y * scale + yBias - _bodies[i].Height / 2);
            }
        }

        private enum SelectedItem { Cursor, Body }
    }
}
