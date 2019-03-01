using static System.Math;

namespace Engine.Models
{
    public struct Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public double Length => Sqrt(Pow(X, 2) + Pow(Y, 2));
        public double Angle => AngleTo(UnitXVector());
        public Vector Unit => new Vector(X / Length, Y / Length);

        public double AngleTo(Vector v) => Acos((X * v.X + Y * v.Y) / (Length * v.Length));

        public Vector RotateTo(Vector v) => v.Unit * Length;
        
        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector operator -(Vector v1, Vector v2) => v1 + (-v2);
        public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y);
        public static Vector operator *(Vector v, double n) => new Vector(v.X * n, v.Y * n);
        public static Vector operator /(Vector v, double n)
        {
            if (n == 0)
                throw new System.DivideByZeroException("Trying to divide vector by zero.");
            return new Vector(v.X / n, v.Y / n);
        }
        public static bool operator ==(Vector v1, Vector v2) => (v1.X == v2.X && v1.Y == v2.Y);
        public static bool operator !=(Vector v1, Vector v2) => (v1.X != v2.X || v1.Y != v2.Y);

        public override bool Equals(object obj)
        {
            Vector v;

            if (obj is Vector == false)
                return false;

            v = (Vector)obj;
            return (v.X == X) && (v.Y == Y);
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"X = {X}; Y = {Y}";
        }

        public static Vector ZeroVector() => new Vector(0, 0);
        public static Vector UnitXVector() => new Vector(1, 0);
        public static Vector UnitYVector() => new Vector(0, 1);

    }
}
