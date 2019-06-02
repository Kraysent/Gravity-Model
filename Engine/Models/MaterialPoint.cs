using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Engine.Models
{
    public class MaterialPoint
    {
        [JsonIgnore]
        /* I don't know why this does not work with G = 6.67e-11. This value is emperic. */
        public const double G = 3.35e-11; /* m^3 * kg^-1 * s^-2 */ 

        public double Mass { get; set; } /* kg */
        public Vector Coordinates { get; set; } /* m, m */
        public Vector Velocity { get; set; } /* m / s, m / s */
        
        public MaterialPoint(Vector coordinates, double mass, Vector velocity)
        {
            Coordinates = coordinates;
            Mass = mass;
            Velocity = velocity;
        }

        public double DistanceTo(MaterialPoint b2) => (Coordinates - b2.Coordinates).Length;

        public static Vector GravityForce(MaterialPoint b1, MaterialPoint b2)
        {
            double forceAbs;
            Vector forceOX;

            if (b1.Coordinates == b2.Coordinates)
                return Vector.ZeroVector();

            forceAbs = G * (b1.Mass * b2.Mass) / (Math.Pow(b1.DistanceTo(b2), 2));
            forceOX = new Vector(forceAbs, 0);
            forceOX = forceOX.RotateTo(b2.Coordinates - b1.Coordinates);

            return forceOX;
        }

        public override string ToString()
        {
            return $"Mass: {Mass}, Coordinates: {Coordinates}, Velocity: {Velocity}";
        }

        public override bool Equals(object obj)
        {
            var point = obj as MaterialPoint;
            return point != null &&
                   Mass == point.Mass &&
                   EqualityComparer<Vector>.Default.Equals(Coordinates, point.Coordinates) &&
                   EqualityComparer<Vector>.Default.Equals(Velocity, point.Velocity);
        }

        public override int GetHashCode()
        {
            var hashCode = -2026078501;
            hashCode = hashCode * -1521134295 + Mass.GetHashCode();
            hashCode = hashCode * -1521134295 + Coordinates.GetHashCode();
            hashCode = hashCode * -1521134295 + Velocity.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MaterialPoint point1, MaterialPoint point2)
        {
            return EqualityComparer<MaterialPoint>.Default.Equals(point1, point2);
        }

        public static bool operator !=(MaterialPoint point1, MaterialPoint point2)
        {
            return !(point1 == point2);
        }
    }
}
