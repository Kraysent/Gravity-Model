namespace Engine.Models
{
    public class MaterialPoint
    {
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

        public override string ToString()
        {
            return $"Mass: {Mass}, Coordinates: {Coordinates}, Velocity: {Velocity}";
        }
    }
}
