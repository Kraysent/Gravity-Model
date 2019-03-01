namespace Engine.Models
{
    public class MaterialPoint : BaseNotificationClass
    {
        public const double G = 6.67408e-11 /* m^3 * kg^-1 * s^-2 */;

        private double _mass;
        private Vector _coordinates;

        public double Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                OnPropetyChanged(nameof(Mass));
            }
        }
        public Vector Coordinates
        {
            get => _coordinates;
            set
            {
                _coordinates = value;
                OnPropetyChanged(nameof(Coordinates));
            }
        }
        public Vector Velocity { get; set; }

        public MaterialPoint()
        {
            Velocity = new Vector(0, 0);
            Coordinates = new Vector(0, 0);
            Mass = 0;
        }

        public double DistanceTo(MaterialPoint b2) => (Coordinates - b2.Coordinates).Length;

        public static Vector GravitationalForce(MaterialPoint b1, MaterialPoint b2)
        {
            double forceAbs;
            Vector forceOX;

            if (b1.Coordinates == b2.Coordinates)
                return Vector.ZeroVector();
            
            forceAbs = G * (b1.Mass * b2.Mass) / (System.Math.Pow(b1.DistanceTo(b2), 2));
            forceOX = new Vector(forceAbs, 0);
            forceOX = forceOX.RotateTo(b2.Coordinates - b1.Coordinates);

            return forceOX;
        }
    }
}
