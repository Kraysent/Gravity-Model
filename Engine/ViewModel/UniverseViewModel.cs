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

        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }
        public int NumberOfBodies
        {
            get => _numberOfBodies;
            set
            {
                _numberOfBodies = value;
                RaisePropertyChanged(nameof(NumberOfBodies));
            }
        }
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
            double scale = Math.Min(FieldHeight, FieldWidth) / CurrentUniverse.CameraFOV;
            double xBias = FieldWidth / 2;
            double yBias = FieldHeight / 2;

            CurrentUniverse.Update();
            NumberOfBodies = CurrentUniverse.Bodies.Count;
            Epoch = CurrentUniverse.Epoch / (60 * 60 * 24 * 365);

            for (int i = 0; i < Bodies.Count; i++)
            {
                Bodies[i].X = CurrentUniverse.Bodies[i].Coordinates.X * scale + xBias - Bodies[i].Width / 2;
                Bodies[i].Y = CurrentUniverse.Bodies[i].Coordinates.Y * scale + yBias - Bodies[i].Height / 2;
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
