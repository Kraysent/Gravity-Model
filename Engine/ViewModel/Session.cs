﻿using Engine.Models;
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
        
        public List<MaterialPoint> Bodies { get; set; }

        public Session()
        {
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
            double bodyDiameter = 5e9;

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

                    //Check if bodies collided
                    if (Bodies[i].DistanceTo(Bodies[j]) < bodyDiameter)
                    {
                        MaterialPoint newBody = new MaterialPoint
                        {
                            Coordinates = Bodies[i].Coordinates,
                            Mass = Bodies[i].Mass + Bodies[j].Mass,
                            Velocity = (Bodies[i].Velocity * Bodies[i].Mass + Bodies[j].Velocity * Bodies[j].Mass) / (Bodies[i].Mass + Bodies[j].Mass) /*Momentum conservation law*/
                        };

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

        private void BodyDeletedRaise(int bodyNumber)
        {
            BodyDeleted?.Invoke(bodyNumber);
        }

        private void BodyAddedRaise(MaterialPoint body)
        {
            BodyAdded?.Invoke(body);
        }
    }
}
