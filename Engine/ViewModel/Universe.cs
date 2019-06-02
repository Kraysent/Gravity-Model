using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ViewModel
{
    public class Universe : ICloneable
    {
        /// <summary>
        /// The name of current Universe
        /// </summary>
        public string Name { get; set; }
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
        /// Determines show tracers or not
        /// </summary>
        public bool EnableTracers { get; set; } = true;
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

                                /*Momentum conservation law*/
                                Bodies[i].Velocity = (m1 - m2) / (m1 + m2) * (v1 - v2) + v2;
                                Bodies[j].Velocity = (2 * m1 * v1) / (m1 + m2) + v2;

                                countingNeeded = false;
                            }
                        }
                    }

                    if (countingNeeded)
                    {
                        currForce = MaterialPoint.GravityForce(Bodies[i], Bodies[j]);
                        forces[i] += currForce;
                        forces[j] -= currForce;
                    }
                }
            }
            
            for (i = 0; i < Bodies.Count; i++)
            {
                acceleration = forces[i] / Bodies[i].Mass;
                Bodies[i].Velocity = Bodies[i].Velocity + acceleration * DeltaTime;
                Bodies[i].Coordinates += Bodies[i].Velocity * DeltaTime;
            }
        }
        
        private void BodyAddedRaise(MaterialPoint body)
        {
            BodyAdded?.Invoke(body);
        }

        private void BodyDeletedRaise(int bodyNumber)
        {
            BodyDeleted?.Invoke(bodyNumber);
        }

        public override bool Equals(object obj)
        {
            var universe = obj as Universe;
            return universe != null &&
                   Name == universe.Name &&
                   CameraFOVX == universe.CameraFOVX &&
                   CameraFOVY == universe.CameraFOVY &&
                   DeltaTime == universe.DeltaTime &&
                   Speed == universe.Speed &&
                   G == universe.G &&
                   Epoch == universe.Epoch &&
                   CollisionsType == universe.CollisionsType &&
                   EqualityComparer<List<MaterialPoint>>.Default.Equals(Bodies, universe.Bodies) &&
                   EnableTracers == universe.EnableTracers;
        }

        public override int GetHashCode()
        {
            var hashCode = -1389298070;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + CameraFOVX.GetHashCode();
            hashCode = hashCode * -1521134295 + CameraFOVY.GetHashCode();
            hashCode = hashCode * -1521134295 + DeltaTime.GetHashCode();
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + Epoch.GetHashCode();
            hashCode = hashCode * -1521134295 + CollisionsType.GetHashCode();

            foreach (MaterialPoint body in Bodies)
            {
                hashCode = hashCode * -1521134295 + body.GetHashCode();
            }
            
            hashCode = hashCode * -1521134295 + EnableTracers.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            Universe universe = new Universe();
            universe.Name = Name;
            universe.CameraFOVX = CameraFOVX;
            universe.CameraFOVY = CameraFOVY;
            universe.DeltaTime = DeltaTime;
            universe.Speed = Speed;
            universe.G = G;
            universe.Epoch = Epoch;
            universe.CollisionsType = CollisionsType;
            universe.EnableTracers = EnableTracers;
            universe.Bodies = Bodies.Select(item => (MaterialPoint)item.Clone()).ToList();

            return universe;
        }

        public static bool operator ==(Universe universe1, Universe universe2)
        {
            return EqualityComparer<Universe>.Default.Equals(universe1, universe2);
        }

        public static bool operator !=(Universe universe1, Universe universe2)
        {
            return !(universe1 == universe2);
        }
    }
}
