using Engine.Models;
using System;
using System.Collections.Generic;

namespace Engine.ViewModel
{
    public class Universe
    {
        /// <summary>
        /// Number of meters in the camera field of view in X axis
        /// </summary>
        public double CameraFOVX { get; set; } = 1e12;
        /// <summary>
        /// Number of meters in the camera field of view in Y axis
        /// </summary>
        public double CameraFOVY { get; set; } = 1e12;
        /// <summary>
        /// Number of seconds per one frame update
        /// </summary>
        public double DeltaTime { get; set; } = 2 * 3600;
        /// <summary>
        /// Number of frame updates per one real frame update
        /// </summary>
        public int Speed { get; set; } = 4;
        /// <summary>
        /// Gravitational constant
        /// </summary>
        public double G { get; set; } = 3.35e-11;
        /// <summary>
        /// Number of updates since start of the session
        /// </summary>
        public int Epoch { get; set; } = 0;
        /// <summary>
        /// Type of collisions in the system
        /// </summary>
        public CollisionType CollisionsType { get; set; } = CollisionType.InelasticCollisions;
        /// <summary>
        /// List of objects
        /// </summary>
        public List<MaterialPoint> Bodies { get; set; } = new List<MaterialPoint>();

        public delegate void BodyDelete(int bodyNumber);
        public delegate void BodyAdd(MaterialPoint body);
        public event BodyDelete BodyDeleted;
        public event BodyAdd BodyAdded;

        public Universe()
        {
            
        }
        
        public Universe(params MaterialPoint[] bodies) : this()
        {
            Add(bodies);
        }
        
        public void Add(params MaterialPoint[] bodies)
        {
            foreach (MaterialPoint body in bodies)
            {
                Bodies.Add(body);
                BodyAddedRaise(body);
            }
        }
        
        public void Update()
        {
            int i; 

            for (i = 0; i < Speed; i++)
            {
                UpdateField();
                Epoch++;
            }
        }

        public void UpdateField()
        {
            Vector[] forces = new Vector[Bodies.Count];
            Vector currForce, acceleration;
            int i, j;
            bool countingNeeded = true;

            /*-------Counting forces-------*/

            for (i = 0; i < Bodies.Count; i++)
            {
                forces[i] = Vector.ZeroVector();
            }

            for (i = 0; i < Bodies.Count; i++)
            {
                for (j = 0; j < Bodies.Count; j++)
                {
                    if (i == j) continue;

                    if (CollisionsType == CollisionType.InelasticCollisions)
                    {
                        /*Check if bodies collided*/
                        if (Bodies[i] is PhysicalBody && Bodies[j] is PhysicalBody)
                        {
                            if (Bodies[i].DistanceTo(Bodies[j]) < ((PhysicalBody)Bodies[i]).Diameter + ((PhysicalBody)Bodies[j]).Diameter)
                            {
                                MaterialPoint newBody = new PhysicalBody(
                                    coordinates: Bodies[i].Coordinates,
                                    mass: Bodies[i].Mass + Bodies[j].Mass,
                                    /*Momentum conservation law*/
                                    velocity: (Bodies[i].Velocity * Bodies[i].Mass + Bodies[j].Velocity * Bodies[j].Mass) / (Bodies[i].Mass + Bodies[j].Mass),
                                    /*Sum of volumes*/
                                    diameter: Math.Pow(Math.Pow(((PhysicalBody)Bodies[i]).Diameter, 3) + Math.Pow(((PhysicalBody)Bodies[j]).Diameter, 3), (double)1 / 3)
                                );

                                /*First must be deleted the last. In other cases this will cause an exception*/
                                BodyDeletedRaise(Math.Max(i, j));
                                BodyDeletedRaise(Math.Min(i, j));
                                Bodies.RemoveAt(Math.Max(i, j));
                                Bodies.RemoveAt(Math.Min(i, j));

                                BodyAddedRaise(newBody);
                                Bodies.Add(newBody);

                                countingNeeded = false;
                            }
                        }
                    }
                    else if (CollisionsType == CollisionType.ElasticCollisions)
                    {
                        /*Check if bodies collided*/
                        if (Bodies[i] is PhysicalBody && Bodies[j] is PhysicalBody)
                        {
                            if (Bodies[i].DistanceTo(Bodies[j]) < ((PhysicalBody)Bodies[i]).Diameter + ((PhysicalBody)Bodies[j]).Diameter)
                            {
                                double m1 = Bodies[i].Mass, m2 = Bodies[j].Mass;
                                Vector v1 = Bodies[i].Velocity, v2 = Bodies[j].Velocity;

                                //Momentum conservation law
                                Bodies[i].Velocity = (m1 - m2) / (m1 + m2) * (v1 - v2) + v2;
                                Bodies[j].Velocity = (2 * m1 * v1) / (m1 + m2) + v2;

                                countingNeeded = false;
                            }
                        }
                    }

                    if (countingNeeded)
                    {
                        currForce = GravitationalForce(Bodies[i], Bodies[j]);
                        forces[i] += currForce;
                        forces[j] -= currForce;
                    }
                }
            }

            /*-------Counting forces end-------*/
            
            for (i = 0; i < Bodies.Count; i++)
            {
                acceleration = forces[i] / Bodies[i].Mass;
                Bodies[i].Velocity = Bodies[i].Velocity + acceleration * DeltaTime;
                Bodies[i].Coordinates += Bodies[i].Velocity * DeltaTime;
            }
        }

        private Vector GravitationalForce(MaterialPoint b1, MaterialPoint b2)
        {
            double forceAbs;
            Vector forceOX;

            if (b1.Coordinates == b2.Coordinates)
                return Vector.ZeroVector();

            forceAbs = MaterialPoint.G * (b1.Mass * b2.Mass) / (Math.Pow(b1.DistanceTo(b2), 2));
            forceOX = new Vector(forceAbs, 0);
            forceOX = forceOX.RotateTo(b2.Coordinates - b1.Coordinates);

            return forceOX;
        }

        private void BodyAddedRaise(MaterialPoint body)
        {
            BodyAdded?.Invoke(body);
        }

        private void BodyDeletedRaise(int bodyNumber)
        {
            BodyDeleted?.Invoke(bodyNumber);
        }
    }
}
