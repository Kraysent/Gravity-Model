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
        
        public Session(params MaterialPoint[] bodies) : this()
        {
            Add(bodies);
        }

        public void UpdateField(double deltaTime)
        {
            Vector[] forces = new Vector[_bodies.Count];
            Vector currForce, acceleration;
            int i, j;

            //-------Counting forces-------//

            for (i = 0; i < _bodies.Count; i++)
            {
                forces[i] = Vector.ZeroVector();
            }

            for (i = 0; i < _bodies.Count; i++)
            {
                for (j = 0; j < _bodies.Count; j++)
                {
                    currForce = GravitationalForce(_bodies[i], _bodies[j]);

                    forces[i] += currForce;
                    forces[j] -= currForce;
                }
            }

            //-------Counting forces end-------//
            
            for (i = 0; i < _bodies.Count; i++)
            {
                acceleration = forces[i] / _bodies[i].Mass;
                _bodies[i].Velocity = _bodies[i].Velocity + acceleration * deltaTime;
                _bodies[i].Coordinates += _bodies[i].Velocity * deltaTime;
            }
        }

        public void Add(params MaterialPoint[] bodies)
        {
            foreach (MaterialPoint body in bodies)
            {
                _bodies.Add(body);
            }
        }

        public static Vector GravitationalForce(MaterialPoint b1, MaterialPoint b2)
        {
            double forceAbs;
            Vector forceOX;

            if (b1.Coordinates == b2.Coordinates)
                return Vector.ZeroVector();

            forceAbs = MaterialPoint.G * (b1.Mass * b2.Mass) / (System.Math.Pow(b1.DistanceTo(b2), 2));
            forceOX = new Vector(forceAbs, 0);
            forceOX = forceOX.RotateTo(b2.Coordinates - b1.Coordinates);

            return forceOX;
        }
    }
}
