using System.Collections.Generic;

namespace Engine.Models
{
    public class PhysicalBody : MaterialPoint
    {
        public double Diameter { get; set; } /* m */

        public PhysicalBody(Vector coordinates, double mass, Vector velocity, double diameter) : base(coordinates, mass, velocity)
        {
            Diameter = diameter;
        }

        public override string ToString()
        {
            return base.ToString() + $", Diameter: {Diameter}";
        }

        public override bool Equals(object obj)
        {
            var body = obj as PhysicalBody;
            return body != null &&
                   base.Equals(obj) &&
                   Diameter == body.Diameter;
        }

        public override int GetHashCode()
        {
            var hashCode = -1971562128;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Diameter.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PhysicalBody body1, PhysicalBody body2)
        {
            return EqualityComparer<PhysicalBody>.Default.Equals(body1, body2);
        }

        public static bool operator !=(PhysicalBody body1, PhysicalBody body2)
        {
            return !(body1 == body2);
        }
    }
}
