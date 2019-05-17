using Engine.Models;
using System.Collections.Generic;

namespace Engine.ViewModel
{
    public class Session
    {
        public List<MaterialPoint> Bodies { get; set; }

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
            Vector[] forces = new Vector[Bodies.Count];
            Vector currForce, acceleration;
            int i, j;

            //-------Counting forces-------//

            for (i = 0; i < Bodies.Count; i++)
            {
                forces[i] = Vector.ZeroVector();
            }

            for (i = 0; i < Bodies.Count; i++)
            {
                for (j = 0; j < Bodies.Count; j++)
                {
                    currForce = GravitationalForce(Bodies[i], Bodies[j]);

                    forces[i] += currForce;
                    forces[j] -= currForce;
                }
            }

            //-------Counting forces end-------//
            
            for (i = 0; i < Bodies.Count; i++)
            {
                acceleration = forces[i] / Bodies[i].Mass;
                Bodies[i].Velocity = Bodies[i].Velocity + acceleration * deltaTime;
                Bodies[i].Coordinates += Bodies[i].Velocity * deltaTime;
            }
        }

        public void Add(params MaterialPoint[] bodies)
        {
            foreach (MaterialPoint body in bodies)
            {
                Bodies.Add(body);
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
