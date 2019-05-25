using Engine.Models;
using System;
using System.Collections.Generic;

namespace Engine.ViewModel
{
    public class Session
    {
        public delegate void BodyDelete(int bodyNumber);
        public delegate void BodyAdd(MaterialPoint body);
        public event BodyDelete BodyDeleted;
        public event BodyAdd BodyAdded;

        public CollisionType CollisionsType { get; set; }
        public double BodyDiameter { get; set; }
        public List<MaterialPoint> Bodies { get; set; }

        public Session(double bodyDiameter = 5e9, CollisionType collisionType = CollisionType.InelasticCollisions)
        {
            BodyDiameter = bodyDiameter;
            Bodies = new List<MaterialPoint>();
        }
        
        public Session(params MaterialPoint[] bodies) : this()
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
                    if (i == j) continue;

                    if (CollisionsType == CollisionType.InelasticCollisions)
                    {
                        //Check if bodies collided
                        if (Bodies[i].DistanceTo(Bodies[j]) < BodyDiameter * 2)
                        {
                            MaterialPoint newBody = new MaterialPoint(
                                coordinates: Bodies[i].Coordinates,
                                mass: Bodies[i].Mass + Bodies[j].Mass,
                                /*Momentum conservation law*/
                                velocity: (Bodies[i].Velocity * Bodies[i].Mass + Bodies[j].Velocity * Bodies[j].Mass) / (Bodies[i].Mass + Bodies[j].Mass)
                            );

                            //First must be deleted the last. In other cases this will cause an exception
                            BodyDeletedRaise(Math.Max(i, j));
                            BodyDeletedRaise(Math.Min(i, j));
                            Bodies.RemoveAt(Math.Max(i, j));
                            Bodies.RemoveAt(Math.Min(i, j));

                            BodyAddedRaise(newBody);
                            Bodies.Add(newBody);
                        }
                        else
                        {
                            currForce = GravitationalForce(Bodies[i], Bodies[j]);
                            forces[i] += currForce;
                            forces[j] -= currForce;
                        }
                    }
                    else if (CollisionsType == CollisionType.ElasticCollisions)
                    {
                        //Check if bodies collided
                        if (Bodies[i].DistanceTo(Bodies[j]) < BodyDiameter * 2)
                        {
                            double m1 = Bodies[i].Mass, m2 = Bodies[j].Mass;
                            Vector v1 = Bodies[i].Velocity, v2 = Bodies[j].Velocity;

                            Bodies[i].Velocity = (m1 - m2) / (m1 + m2) * (v1 - v2) + v2;
                            Bodies[j].Velocity = (2 * m1 * v1) / (m1 + m2) + v2;
                        }
                        else
                        {
                            currForce = GravitationalForce(Bodies[i], Bodies[j]);
                            forces[i] += currForce;
                            forces[j] -= currForce;
                        }
                    }
                    else if (CollisionsType == CollisionType.NoCollisions)
                    {
                        currForce = GravitationalForce(Bodies[i], Bodies[j]);
                        forces[i] += currForce;
                        forces[j] -= currForce;
                    }
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

        private Vector GravitationalForce(MaterialPoint b1, MaterialPoint b2)
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
