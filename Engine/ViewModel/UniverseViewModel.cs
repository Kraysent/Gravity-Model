using Engine.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace Engine.ViewModel
{
    public class UniverseViewModel : BaseNotificationClass
    {
        private Universe _currentUniverse;
        private int _numberOfBodies;
        private double _epoch;
        private string _width;
        private string _height;
        private string _title;

        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
        public string Width
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }
        public string Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }
        public int NumberOfBodies
        {
            get => _numberOfBodies;
            set
            {
                _numberOfBodies = value;
                RaisePropertyChanged(nameof(NumberOfBodies));
            }
        }
        public int FPS => 1000;
        public double Epoch
        {
            get => _epoch;
            set
            {
                _epoch = value;
                RaisePropertyChanged(nameof(Epoch));
            }
        }
        public Universe CurrentUniverse
        {
            get => _currentUniverse;
            set
            {
                _currentUniverse = value.Clone() as Universe;

                Start();
            }
        }
        public ObservableCollection<Body> Bodies { get; set; }

        public UniverseViewModel()
        {
            Bodies = new ObservableCollection<Body>();
        }

        public void Start()
        {
            CurrentUniverse.BodyAdded += Universe_BodyAdded;
            CurrentUniverse.BodyDeleted += Universe_BodyDeleted;

            foreach (MaterialPoint body in CurrentUniverse.Bodies)
            {
                Universe_BodyAdded(body);
            }
        }

        public void Update()
        {
            //It would be better to process these values only if FieldWidth/Height changed in order to reduce number of calculations
            double scale = Math.Min(FieldHeight, FieldWidth) / CurrentUniverse.CameraFOV;
            double xBias = FieldWidth / 2;
            double yBias = FieldHeight / 2;
            Width = (FieldWidth / Math.Max(FieldWidth, FieldHeight) * CurrentUniverse.CameraFOV).ToString("E3");
            Height = (FieldHeight / Math.Max(FieldWidth, FieldHeight) * CurrentUniverse.CameraFOV).ToString("E3");
            Title = CurrentUniverse.Name;

            CurrentUniverse.Update();
            NumberOfBodies = CurrentUniverse.Bodies.Count;
            Epoch = Math.Round(CurrentUniverse.Epoch / (60 * 60 * 24 * 365), 3);

            for (int i = 0; i < Bodies.Count; i++)
            {
                Bodies[i].X = CurrentUniverse.Bodies[i].Coordinates.X * scale + xBias - Bodies[i].Width / 2;
                Bodies[i].Y = CurrentUniverse.Bodies[i].Coordinates.Y * scale + yBias - Bodies[i].Height / 2;
            }
        }

        public void ChangeFOV(bool isChangedToPositive)
        {
            if (isChangedToPositive)
            {
                CurrentUniverse.CameraFOV /= 1.1;
            }
            else
            {
                CurrentUniverse.CameraFOV *= 1.1;
            }
        }

        private void Universe_BodyAdded(MaterialPoint body)
        {
            Body bodyToAdd = new Body
            {
                X = 0,
                Y = 0,
                Width = 10,
                Height = 10
            };

            Bodies.Add(bodyToAdd);
        }

        private void Universe_BodyDeleted(int bodyNumber)
        {
            Bodies.RemoveAt(bodyNumber);
        }
    }
}
