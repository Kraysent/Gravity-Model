﻿using Engine.Models;
using System;
using System.Collections.Generic;

namespace Engine.ViewModel
{
    public class SessionFactory
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);

        public static Session StartSolarSystem()
        {
            return new Session(
                new MaterialPoint(coordinates: new Vector(0, 0), mass: 1.98885e30, velocity: new Vector(0, 0)), /*Sun*/
                new MaterialPoint(coordinates: new Vector(0.460001e11, 0), mass: 3.33022e23, velocity: new Vector(0, -5.3712e4)), /*Mercury*/
                new MaterialPoint(coordinates: new Vector(1.07476e11, 0), mass: 4.8675e24, velocity: new Vector(0, -3.5139e4)), /*Venus*/
                new MaterialPoint(coordinates: new Vector(1.47098e11, 0), mass:  5.9726e24, velocity: new Vector(0, -3.0036e4)), /*Earth*/
                new MaterialPoint(coordinates: new Vector(2.06655e11, 0), mass:  6.4171e23, velocity: new Vector(0, -2.5341e4)), /*Mars*/
                new MaterialPoint(coordinates: new Vector(7.40574e11, 0), mass:  1.8986e27, velocity: new Vector(0, -1.307e4)), /*Jupiter*/
                new MaterialPoint(coordinates: new Vector(13.5357e11, 0), mass:  5.6846e26, velocity: new Vector(0, -0.969e4)) /*Saturn*/
                );
        }

        public static Session StartJupiterSystem()
        {
            return new Session(
                new MaterialPoint(coordinates: new Vector(0, 0), mass: 1.8987e27, velocity: new Vector(0, 0)), /*Jupiter*/
                new MaterialPoint(coordinates: new Vector(1.0692e9, 0), mass: 1.4819e23, velocity: new Vector(0, -1.088e4)) /*Ganimed*/
                );
        }

        public static Session StartHypSolarSystem()
        {
            return new Session(
                new MaterialPoint(coordinates: new Vector(0, 0), mass: 2e30, velocity: new Vector(0, 0)), /*Sun*/
                new MaterialPoint(coordinates: new Vector(1.5e11, 0), mass: 6e24, velocity: new Vector(0, 2.9830633e4)) /*Earth*/
                );
        }

        public static Session StartRandomSystem(int numberOfObjects)
        {
            int i;
            List<MaterialPoint> bodies = new List<MaterialPoint>();
            double coordsScale = 1e12, velocityScale = 4e4, massScale = 1e30;

            for (i = 0; i < numberOfObjects; i++)
            {
                bodies.Add(new MaterialPoint(
                    coordinates: new Vector(rnd.NextDouble() * coordsScale, rnd.NextDouble() * coordsScale),
                    mass: rnd.NextDouble() * massScale,
                    velocity: new Vector(rnd.NextDouble() * Math.Pow(-1, rnd.Next(1, 4)) * velocityScale, rnd.NextDouble() * Math.Pow(-1, rnd.Next(1, 4)) * velocityScale)
                ));
            }

            return new Session(bodies.ToArray());
        }

        public static Session StartTestSystem()
        {
            return new Session(
               new MaterialPoint(coordinates: new Vector(0, 0), mass: 2e30, velocity: new Vector(0, 0)),
               new MaterialPoint(coordinates: new Vector(1.5e12, 0), mass: 6e29, velocity: new Vector(-100000, 0))
               );
        }

        public static Session StartSquareSystem()
        {
            int i, j;
            List<MaterialPoint> bodies = new List<MaterialPoint>();

            for (j = 0; j < 10; j++)
            {
                for (i = 0; i < 10; i++)
                {
                    bodies.Add(new MaterialPoint(
                        coordinates: new Vector(1e11 * i, 1e11 * j),
                        mass: 1e30,
                        velocity: Vector.ZeroVector()
                    ));
                }
            }

            return new Session(bodies.ToArray());
        }

        public static Session StartCircleSystem(int numberOfBodies)
        {
            int i;
            double radius = 4e11, velocity = 3e4;
            List<MaterialPoint> bodies = new List<MaterialPoint>();

            for (i = 0; i < numberOfBodies; i++)
            {
                bodies.Add(new MaterialPoint(
                    coordinates: new Vector(5e11 + radius * Math.Sin(2 * Math.PI / numberOfBodies * i), 5e11 + radius * Math.Cos(2 * Math.PI / numberOfBodies * i)),
                    mass: 2e30,
                    velocity: new Vector(velocity * Math.Cos(2 * Math.PI / numberOfBodies * i), -velocity * Math.Sin(2 * Math.PI / numberOfBodies * i))
                ));
            }

            return new Session(bodies.ToArray());
        }

        public static Session StartMultiCircleSystem(double radius, int numberOfCircles, int numberOfBodies)
        {
            int i, j;
            double velocity = 5e4, center = 5e11, mass = 2e30;
            List<MaterialPoint> bodies = new List<MaterialPoint>();

            for (j = 0; j < numberOfCircles; j++)
            {
                for (i = 0; i < numberOfBodies; i++)
                {
                    bodies.Add(new MaterialPoint(
                        coordinates: new Vector(center + radius * (j + 1) / numberOfCircles  * Math.Sin(2 * Math.PI / numberOfBodies * i), center + radius * (j + 1) / numberOfCircles * Math.Cos(2 * Math.PI / numberOfBodies * i)),
                        mass: mass,
                        velocity: new Vector(velocity * Math.Cos(2 * Math.PI / numberOfBodies * i), -velocity * Math.Sin(2 * Math.PI / numberOfBodies * i))
                    ));
                }
            }

            return new Session(bodies.ToArray());
        }
        
        public static Session StartGalaxySystem(int numberOfBodies)
        {
            int i;
            Session result = new Session();
            NormalRandom rnd1 = new NormalRandom();
            double radiusScale = 1e16, currVelocity, currRadius, massScale = 2e30;
            List<MaterialPoint> bodies = new List<MaterialPoint>();

            for (i = 0; i < numberOfBodies; i++)
            {
                currRadius = Math.Abs(radiusScale * (rnd1.NextDouble() / 5 + 0.6));
                currVelocity = Math.Sqrt(MaterialPoint.G * 0.5 * massScale * numberOfBodies / radiusScale);

                bodies.Add(new MaterialPoint(
                    coordinates: new Vector(currRadius * Math.Sin(2 * Math.PI / numberOfBodies * i), currRadius * Math.Cos(2 * Math.PI / numberOfBodies * i)),
                    mass: rnd.NextDouble() * massScale,
                    velocity: new Vector(currVelocity * Math.Cos(2 * Math.PI / numberOfBodies * i), -currVelocity * Math.Sin(2 * Math.PI / numberOfBodies * i))
                ));
            }

            result = new Session(bodies.ToArray());
            result.BodyDiameter = 5e13;

            return result;
        }

        private class NormalRandom : Random
        {
            // сохранённое предыдущее значение
            double prevSample = double.NaN;

            protected override double Sample()
            {
                // есть предыдущее значение? возвращаем его
                if (!double.IsNaN(prevSample))
                {
                    double result = prevSample;
                    prevSample = double.NaN;
                    return result;
                }

                // нет? вычисляем следующие два
                // Marsaglia polar method из википедии
                double u, v, s;
                do
                {
                    u = 2 * base.Sample() - 1;
                    v = 2 * base.Sample() - 1; // [-1, 1)
                    s = u * u + v * v;
                }
                while (u <= -1 || v <= -1 || s >= 1 || s == 0);
                double r = Math.Sqrt(-2 * Math.Log(s) / s);

                prevSample = r * v;
                return r * u;
            }

            public override double NextDouble()
            {
                return Sample();
            }
        }
    }
}
