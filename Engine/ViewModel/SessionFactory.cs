using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ViewModel
{
    public class SessionFactory
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);

        public static Session StartSolarSystem()
        {
            return new Session(
                new MaterialPoint() { Coordinates = new Vector(0, 0), Mass = 1.98885e30, Velocity = new Vector(0, 0) }, /*Sun*/
                new MaterialPoint() { Coordinates = new Vector(0.460001e11, 0), Mass = 3.33022e23, Velocity = new Vector(0, -5.3712e4) }, /*Mercury*/
                new MaterialPoint() { Coordinates = new Vector(1.07476e11, 0), Mass = 4.8675e24, Velocity = new Vector(0, -3.5139e4) }, /*Venus*/
                new MaterialPoint() { Coordinates = new Vector(1.47098e11, 0), Mass = 5.9726e24, Velocity = new Vector(0, -3.0036e4) }, /*Earth*/
                new MaterialPoint() { Coordinates = new Vector(2.06655e11, 0), Mass = 6.4171e23, Velocity = new Vector(0, -2.5341e4) }, /*Mars*/
                new MaterialPoint() { Coordinates = new Vector(7.40574e11, 0), Mass = 1.8986e27, Velocity = new Vector(0, -1.307e4) }, /*Jupiter*/
                new MaterialPoint() { Coordinates = new Vector(13.5357e11, 0), Mass = 5.6846e26, Velocity = new Vector(0, -0.969e4) } /*Saturn*/
                );
        }

        public static Session StartJupiterSystem()
        {
            return new Session(
                new MaterialPoint() { Coordinates = new Vector(0, 0), Mass = 1.8987e27, Velocity = new Vector(0, 0) }, /*Jupiter*/
                new MaterialPoint() { Coordinates = new Vector(1.0692e9, 0), Mass = 1.4819e23, Velocity = new Vector(0, -1.088e4) } /*Ganimed*/
                );
        }

        public static Session StartHypSolarSystem()
        {
            return new Session(
                new MaterialPoint() { Coordinates = new Vector(0, 0), Mass = 2e30, Velocity = new Vector(0, 0) }, /*Sun*/
                new MaterialPoint() { Coordinates = new Vector(1.5e11, 0), Mass = 6e24, Velocity = new Vector(0, 2.9830633e4) } /*Earth*/
                );
        }

        public static Session StartRandomSystem(int numberOfObjects)
        {
            int i;
            List<MaterialPoint> bodies = new List<MaterialPoint>();
            double coordsScale = 1e12, velocityScale = 4e4, massScale = 1e30;

            for (i = 0; i < numberOfObjects; i++)
            {
                bodies.Add(new MaterialPoint
                {
                    Coordinates = new Vector(rnd.NextDouble() * coordsScale, rnd.NextDouble() * coordsScale),
                    Mass = rnd.NextDouble() * massScale,
                    Velocity = new Vector(rnd.NextDouble() * Math.Pow(-1, rnd.Next(1, 4)) * velocityScale, rnd.NextDouble() * Math.Pow(-1, rnd.Next(1, 4)) * velocityScale)
                });
            }

            return new Session(bodies.ToArray());
        }

        public static Session StartTestSystem()
        {
            return new Session(
               new MaterialPoint() { Coordinates = new Vector(0, 0), Mass = 2e30, Velocity = new Vector(0, 0) },
               new MaterialPoint() { Coordinates = new Vector(1.5e12, 0), Mass = 6e29, Velocity = new Vector(-100000, 0) }
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
                    bodies.Add(new MaterialPoint
                    {
                        Coordinates = new Vector(1e11 * i, 1e11 * j),
                        Mass = 1e30,
                        Velocity = Vector.ZeroVector()
                    });
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
                bodies.Add(new MaterialPoint
                {
                    Coordinates = new Vector(5e11 + radius * Math.Sin(2 * Math.PI / numberOfBodies * i), 5e11 + radius * Math.Cos(2 * Math.PI / numberOfBodies * i)),
                    Mass = 2e30,
                    Velocity = new Vector(velocity * Math.Cos(2 * Math.PI / numberOfBodies * i), -velocity * Math.Sin(2 * Math.PI / numberOfBodies * i))
                });
            }

            return new Session(bodies.ToArray());
        }

        public static Session StartMultiCircleSystem(double radius, int numberOfCircles, int numberOfBodies)
        {
            int i, j;
            double velocity = 0;
            List<MaterialPoint> bodies = new List<MaterialPoint>();

            for (j = 0; j < numberOfCircles; j++)
            {
                for (i = 0; i < numberOfBodies; i++)
                {
                    bodies.Add(new MaterialPoint
                    {
                        Coordinates = new Vector(5e11 + radius * (j + 1) / numberOfCircles  * Math.Sin(2 * Math.PI / numberOfBodies * i), 5e11 + radius * (j + 1) / numberOfCircles * Math.Cos(2 * Math.PI / numberOfBodies * i)),
                        Mass = 2e30,
                        Velocity = new Vector(velocity * Math.Cos(2 * Math.PI / numberOfBodies * i), -velocity * Math.Sin(2 * Math.PI / numberOfBodies * i))
                    });
                }
            }

            return new Session(bodies.ToArray());
        }
    }
}
