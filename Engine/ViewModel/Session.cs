using Engine.Models;
using System.Collections.Generic;

namespace Engine.ViewModel
{
    public class Session : BaseNotificationClass
    {
        private List<MaterialPoint> _bodies;

        public List<MaterialPoint> Bodies
        {
            get => _bodies;
            set
            {
                _bodies = value;
                OnPropetyChanged(nameof(Bodies));
            }
        }
        
        public Session()
        {
            Bodies = new List<MaterialPoint>();
        }
        
        public Session(params MaterialPoint[] bodies)
        {
            Bodies = new List<MaterialPoint>();
            Bodies.AddRange(bodies);
        }

        public void UpdateField(double deltaTime)
        {
            Vector[] forces = new Vector[_bodies.Count];
            Vector currForce, acceleration;
            int i, j;

            for (i = 0; i < _bodies.Count; i++)
            {
                forces[i] = Vector.ZeroVector();
            }

            for (i = 0; i < _bodies.Count; i++)
            {
                for (j = 0; j < _bodies.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    currForce = MaterialPoint.GravitationalForce(_bodies[i], _bodies[j]);

                    forces[i] += currForce;
                    forces[j] -= currForce;
                }
            }

            for (i = 0; i < _bodies.Count; i++)
            {
                acceleration = forces[i] / _bodies[i].Mass;
                _bodies[i].Velocity += acceleration * deltaTime;
                _bodies[i].Coordinates += _bodies[i].Velocity * deltaTime;
            }
        }
    }
}
